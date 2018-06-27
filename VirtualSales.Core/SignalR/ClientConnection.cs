using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using VirtualSales.Core.AppServices;
using VirtualSales.Core.SignalRx;
using VirtualSales.Models;

namespace VirtualSales.Core.SignalR
{
    public interface IClientConnection : ISignalRClient
    {
        /// <summary>
        /// Will content be non-null when the meeting is in progress
        /// </summary>
        IObservable<VideoChatConfiguration> MeetingDetails { get; }
        Task JoinMeeting(Guid meetingId);
        Task LeaveMeeting(Guid meetingId);
        Task<Meeting> GetMeetingDetails(Guid meetingId);
        Task<Meeting> GetMeetingDetails(string guidPrefix);
    }

    public class ClientConnection : SignalRClient, IClientConnection
    {
        private readonly IPropertyChangedReceiver _receiver;
        private readonly Subject<VideoChatConfiguration> _vcSubject = new Subject<VideoChatConfiguration>();

        public ClientConnection(IPropertyChangedReceiver receiver, ISettings settings) : base(settings)
        {
            _receiver = receiver;

            MeetingDetails = _vcSubject;

            Proxy.Observe<ChangedMessage>("onPropertyChanged").ObserveOn(RxApp.MainThreadScheduler).Subscribe(_receiver);
            var start = Proxy.Observe<VideoChatConfiguration>("startMeeting").Log(this, "Start Meeting Received");
            var ended = Proxy.Observe("endMeeting").Log(this,"End Meeting Received").Select(_ => (VideoChatConfiguration)null);
            start.Merge(ended).Subscribe(_vcSubject);
        }


        /// <summary>
        /// Will content be non-null when the meeting is in progress
        /// </summary>
        public IObservable<VideoChatConfiguration> MeetingDetails { get; private set; }

        public async Task JoinMeeting(Guid meetingId)
        {
            await ConnectedTask;
            await Proxy.Invoke("joinMeeting", meetingId);
        }

        public async Task LeaveMeeting(Guid meetingId)
        {
            await ConnectedTask;
            await Proxy.Invoke("leaveMeeting", meetingId);           
        }

        public async Task<Meeting> GetMeetingDetails(Guid meetingId)
        {
            await ConnectedTask;
            return await Proxy.Invoke<Meeting>("getMeetingDetails", meetingId);
        }

        public async Task<Meeting> GetMeetingDetails(string guidPrefix)
        {
            await ConnectedTask;
            return await Proxy.Invoke<Meeting>("getMeetingDetailsFromPrefix", guidPrefix);            
        }
    }
}
