using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using VirtualSales.Models;
using VirtualSales.Web.Controllers;
using VirtualSales.Web.Services;

namespace VirtualSales.Web.Hubs
{
    public class MeetingHub : Hub
    {
        private static HeartbeatService _heartbeatService = new HeartbeatService();

        private static readonly MeetingInfoRepository MeetingInfoRepository;
        private static readonly MeetingRepository MeetingRepository;
        private static readonly WhiteBrandGroupManager GroupManager;

   //     private readonly IRemoteBlobStorage _blogStorage = new S3Storage();
        private readonly IPdfDocumentTemplateProvider _templateProvider = new PdfDocumentTemplateProvider();

        static MeetingHub()
        {
            MeetingInfoRepository = new MeetingInfoRepository();
            MeetingRepository = new MeetingRepository(MeetingInfoRepository);
            GroupManager = new WhiteBrandGroupManager();
        }
        
        public override Task OnDisconnected()
        {
            var agentMeeting = MeetingInfoRepository.FindMeetingForAgent(Context.ConnectionId);
            var clientMeeting = MeetingInfoRepository.FindMeetingForClient(Context.ConnectionId);

            // nothing to do here, just do the base class stuff
            if (agentMeeting == null && clientMeeting == null) return base.OnDisconnected();

            if (agentMeeting != null)
            {
                Debug.WriteLine("Agent disonnected from meeting {0} at connection {1}", new []  { agentMeeting.Id.ToString(), Context.ConnectionId });
                EndMeeting(agentMeeting.Id);
            }
            else if (clientMeeting != null)
            {
                Debug.WriteLine("Client disonnected from meeting {0} at connection {1}", new[] { clientMeeting.Id.ToString(), Context.ConnectionId });
                LeaveMeeting(clientMeeting.Id);
            }
            return base.OnDisconnected();
        }

        private static IEnumerable<MeetingStatus> BuildMeetingsStatus()
        {
            return MeetingInfoRepository.AllMeetings.Select(p => MeetingRepository.BuildMeetingStatus(p, MeetingInfoRepository.GetMeetingInfo(p)));
        } 

        private static void UpdateMeetingStatusToClients(IHub hub)
        {
            var meetingStati = BuildMeetingsStatus();
            
            Debug.WriteLine("Meeting Status Update:");
            foreach (var m in meetingStati)
            {
                Debug.WriteLine("Meeting {0} [HAsAgent:{1}] [HasClient:{2}] [MeetingStarted:{3}]", m.MeetingId, m.AgentJoined, m.ClientJoined, m.MeetingStarted);
            }
            hub.Clients.All.meetingStatusUpdated(meetingStati);
        }
        private static void UpdateMeetingStatusToClients(IHubContext context)
        {
            var meetingStati = BuildMeetingsStatus();

            Debug.WriteLine("Meeting Status Update:");
            foreach (var m in meetingStati)
            {
                Debug.WriteLine("Meeting {0} [HasAgent:{1}] [HasClient:{2}] [MeetingStarted:{3}]", m.MeetingId, m.AgentJoined, m.ClientJoined, m.MeetingStarted);
            }
            context.Clients.All.meetingStatusUpdated(meetingStati);
        }

        public void NotifyPdfAvailable(Guid meetingId, string key)
        {
            var room = meetingId.ToString();
            Clients.Group(room).onPdfAvailable(key);
        }

        public async Task GenerateAndSaveIllustration(Guid meetingId, IllustrationRequest illustrationRequest)
        {
            var pdfService = new PdfService(_templateProvider, illustrationRequest);
            var key = pdfService.GeneratedId;
            var stream = await pdfService.CreateForm1();

            var bytedata = stream.ReadToEnd();
       //     _blogStorage.StoreValue(key, new MemoryStream(bytedata));
            PDFDocController.InsertDocument(key, bytedata);
            NotifyPdfAvailable(meetingId, key);
        }

        public Task<IEnumerable<Meeting>> AgentLogin(string authToken)
        {
            if (string.IsNullOrWhiteSpace(authToken))
                throw new HubException("Access denied.");

            return Task.FromResult(MeetingRepository.GetMeetings());
        }

        public void SendPropertyChange(ChangedMessage message)
        {
            var meeting = MeetingInfoRepository.FindMeetingForAgent(Context.ConnectionId);
            if (meeting == null) return; // means this connection is not an agent

            var room = meeting.Id.ToString();

            try
            {
                Debug.WriteLine(string.Format("Propagating changed to meeting {0} property name {1}", room, message.PropertyName));
                Clients.Group(room).onPropertyChanged(message);
            }
            catch (Exception)
            {
                Debug.WriteLine("Error calling onPropertyChanged");
            }
        }

        public Task<Meeting> GetMeetingDetails(Guid meetingId)
        {
            var details = MeetingRepository.GetMeeting(meetingId);
            return Task.FromResult(details);
        }

        public async Task StopObserveRoom(Guid meetingId)
        {
            var room = meetingId.ToString();
            await GroupManager.RemoveFromGroup(Groups, room, Context.ConnectionId);
        }

        public async Task ObserveRoom(Guid meetingId)
        {
            var room = meetingId.ToString();
            Debug.WriteLine("Connection {0} observing room {1}", new[] { Context.ConnectionId, room });
            await GroupManager.AddToGroup(Groups, room, Context.ConnectionId);

            var meeting = MeetingInfoRepository.GetMeetingInfo(meetingId);
            if (meeting != null && meeting.HasClient)
            {
                Clients.Caller.clientJoined();
            }
            else
            {
                Clients.Caller.clientLeft();
            }
        }

        public Task<Meeting> GetMeetingDetailsFromPrefix(string prefix)
        {
            var details = MeetingRepository.GetMeetings().FirstOrDefault(p =>
                                                                    {
                                                                        var guidstr = p.Id.ToString().ToUpper();
                                                                        return guidstr.StartsWith(prefix.ToUpper());
                                                                    });
            return Task.FromResult(details);
        }

        public async Task<VideoChatConfiguration> StartMeeting(Guid meetingId)
        {
            var meeting = MeetingInfoRepository.GetMeetingInfo(meetingId);
            if (meeting == null) return null;
            if (meeting.HasAgent) return null;
            if (meeting.HasMeetingStarted) return null;


            var room = meetingId.ToString();
            Debug.WriteLine("Starting meeting {0} by agent {1}", new object[] {room, Context.ConnectionId});
            var config = new VideoChatConfiguration
            {
                AppId = 416,
                ApiKey = "<insert ADL key here>",
                ScopeId = room,
                MaxWidth = 240,
                MaxHeight = 320,
                MaxFramesPerSecond = 15,
                MaxBitRate = 1024,
                Expires = VideoChatConfiguration.CurrentTimeMillis() + (5*60),
                MeetingId = meetingId
            };
            config.Salt = config.ScopeId;

            meeting.ConnectAgent(Context.ConnectionId);
            meeting.StartMeeting(config);

            // add caller to meeting room
            await GroupManager.AddToGroup(Groups, room, Context.ConnectionId);
            if (meeting.HasClient)
            {
                // fire off the joined message to the agent as the client is already waiting in the room
                try
                {
                    Debug.WriteLine("Meeting {0} has client, sending client joined to agent", new object[] { room });

                    Clients.Caller.clientJoined();
                }
                catch (Exception)
                {
                    Debug.WriteLine("Error calling clientJoined");
                }
                // Tell the client to start with the following config
                try
                {
                    Debug.WriteLine("Meeting {0} has client, sending start meeting to client {1}", new object[] { room, meeting.ClientConnectionId });
                    Clients.Group(room, Context.ConnectionId).startMeeting(config);
                }
                catch (Exception)
                {
                    Debug.WriteLine("Error calling startMeeting");
                }
            }

            UpdateMeetingStatusToClients(this);
            return config;
        }

        public async void JoinMeeting(Guid meetingId)
        {
            // client joining the room

            var meeting = MeetingInfoRepository.GetMeetingInfo(meetingId);
            if (meeting == null) return;
            if (meeting.HasClient) return;

            var room = meetingId.ToString();
            Debug.WriteLine("Joining meeting {0} by client {1}", new object[] { room, Context.ConnectionId });

            try
            {
                await GroupManager.AddToGroup(Groups, room, Context.ConnectionId);
                Debug.WriteLine("Sending client joined");

                Clients.Group(room, Context.ConnectionId).clientJoined();
            }
            catch (Exception)
            {
                Debug.WriteLine("Error calling clientJoined");
            }

            meeting.ConnectClient(Context.ConnectionId);

            if (meeting.HasMeetingStarted)
            {
                try
                {
                    Debug.WriteLine("Meeting {0} has started, sending started to client {1}", new object[] { room, Context.ConnectionId });
                    Clients.Caller.startMeeting(meeting.ChatConfig);
                }
                catch (Exception)
                {
                    Debug.WriteLine("Error calling startMeeting");
                }                
            }

            UpdateMeetingStatusToClients(this);
        }

        public void LeaveMeeting(Guid meetingId)
        {
            var meeting = MeetingInfoRepository.GetMeetingInfo(meetingId);
            if (meetingId == Guid.Empty)
            {
                meeting = MeetingInfoRepository.FindMeetingForClient(Context.ConnectionId); 
                // if the meeting id is not included, still see if the current connection is a client in any meeting
            }

            if (meeting == null) return;
            if (meeting.ClientConnectionId != Context.ConnectionId) return; // disallow if this connection is not the client

            var room = meeting.Id.ToString();

            Debug.WriteLine("Client leaving meeting {0} on connection {1}", new object[] { room, Context.ConnectionId });

            try
            {
                Clients.Group(room, Context.ConnectionId).clientLeft();
            }
            catch (Exception)
            {
                Debug.WriteLine("Error calling clientLeft");
            }

            meeting.DisconnectClient();
            UpdateMeetingStatusToClients(this);
        }

        private static void EndMeetingImpl(IHubContext context, Guid meetingId)
        {
            var room = meetingId.ToString();

            Debug.WriteLine("Ending meeting {0}", new object[] { room });

            try
            {
                Debug.WriteLine("Notifying meeting {0} ended to client {1}", new object[] { room });
                context.Clients.Group(room).endMeeting();
            }
            catch (Exception)
            {
                Debug.WriteLine("Error calling endMeeting");
            }

            Debug.WriteLine("CLEARING GROUP " + room);
            MeetingInfoRepository.RemoveMeeting(meetingId);
            GroupManager.ClearGroup(context.Groups, room);
        }
        private static async Task EndMeetingImpl(IHub hub, string connectionId, Guid meetingId)
        {
            var meeting = MeetingInfoRepository.GetMeetingInfo(meetingId);
            var room = meetingId.ToString();

            Debug.WriteLine("Ending meeting {0} by agent {1}", new object[] { room, connectionId });

            try
            {
                Debug.WriteLine("Notifying meeting {0} ended to client {1}", new object[] { room, meeting.ClientConnectionId });
                hub.Clients.Group(room, connectionId).endMeeting();
                await GroupManager.RemoveFromGroup(hub.Groups, room, connectionId);
            }
            catch (Exception)
            {
                Debug.WriteLine("Error calling endMeeting");
            }

            try
            {
                hub.Clients.Caller.clientLeft();
            }
            catch (Exception)
            {
                Debug.WriteLine("Error calling clientLeft");
            }

            Debug.WriteLine("CLEARING GROUP " + room);
            MeetingInfoRepository.RemoveMeeting(meetingId);
            GroupManager.ClearGroup(hub.Groups, room);
            UpdateMeetingStatusToClients(hub);
        }

        public async Task EndMeeting(Guid meetingId)
        {
            var meeting = MeetingInfoRepository.GetMeetingInfo(meetingId);
            if (meetingId == Guid.Empty)
            {
                meeting = MeetingInfoRepository.FindMeetingForAgent(Context.ConnectionId);
                // if the meeting id is not included, still see if the current connection is a client in any meeting
            }
            if (meeting == null) return;
            if (meeting.AgentConnectionId != Context.ConnectionId) return; // disallow if this connection is not the agent

            await EndMeetingImpl(this, Context.ConnectionId, meeting.Id);
        }

        public static void ClearAsync()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<MeetingHub>();

            // need to end all of the current meetings
            foreach (var id in MeetingInfoRepository.AllMeetings.ToList())
            {
                EndMeetingImpl(hubContext, id);
            }

            MeetingInfoRepository.Clear();
            GroupManager.ClearGroups(hubContext.Groups);
            UpdateMeetingStatusToClients(hubContext);

        }
    }
}