using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using VirtualSales.Core.AppServices;
using VirtualSales.Core.Models;
using VirtualSales.Core.SignalRx;
using VirtualSales.Models;

namespace VirtualSales.Core.SignalR
{
    public interface IAgentConnection : ISignalRClient
    {
        Task<VideoChatConfiguration> StartMeeting(Guid meetingId);
        Task EndMeeting(Guid meetingId);
        Task GenerateIllustration(Guid meetingId, PersonModel model);
        Task ObserveRoom(Guid meetingId);
 
        IObservable<bool> ClientInMeeting { get; }
        IObservable<IEnumerable<MeetingStatus>> MeetingStatus { get; }            
        IObservable<IEnumerable<Meeting>> Meetings { get; }
    }

    public class AgentConnection : SignalRClient, IAgentConnection
    {
        private readonly IPropertyChangedTransmitter _propertyTransmitter;

        public AgentConnection(IPropertyChangedTransmitter propertyTransmitter, ISettings settings) : base(settings) 
        {
            _propertyTransmitter = propertyTransmitter;

            _propertyTransmitter.OutgoingChanges.Subscribe(async cm =>
            {
                await ConnectedTask;
                await Proxy.Invoke("SendPropertyChange", cm);
            });

            var joined = Proxy.Observe("clientJoined").Select(_ => true);
            var left = Proxy.Observe("clientLeft").Select(_ => false);

            joined.Merge(left).Subscribe(_clientInMeeting);

            Proxy.Observe("meetingStatusUpdated").Select(p =>
                                                         {
                                                             var result = JsonConvert.DeserializeObject<List<MeetingStatus>>(p[0].ToString());
                                                             return result;
                                                         }).Subscribe(_meetingStatus);

            var obs = Observable.FromAsync(async () =>
                            {
                                await ConnectedTask;
                                var meetings = await Proxy.Invoke<IEnumerable<Meeting>>("agentLogin", "authTokenString");
                                return meetings;
                            });

            Meetings = obs;

        }

        private readonly BehaviorSubject<bool> _clientInMeeting = new BehaviorSubject<bool>(false);
        private readonly BehaviorSubject<IEnumerable<MeetingStatus>> _meetingStatus = new BehaviorSubject<IEnumerable<MeetingStatus>>(new List<MeetingStatus>());


        public async Task<VideoChatConfiguration> StartMeeting(Guid meetingId)
        {
            await ConnectedTask;
            return await Proxy.Invoke<VideoChatConfiguration>("startMeeting", meetingId);
        }

        public async Task EndMeeting(Guid meetingId)
        {
            await ConnectedTask;
            await Proxy.Invoke("endMeeting", meetingId);
        }

        public async Task ObserveRoom(Guid meetingId)
        {
            await ConnectedTask;
            await Proxy.Invoke("observeRoom", meetingId);
        }


        public IObservable<bool> ClientInMeeting
        {
            get { return _clientInMeeting; }
        }

        public IObservable<IEnumerable<MeetingStatus>> MeetingStatus { get { return _meetingStatus; } } 

        public IObservable<IEnumerable<Meeting>> Meetings { get; private set; }


        public async Task GenerateIllustration(Guid meetingId, PersonModel model)
        {
            var illustrationRequest = new IllustrationRequest
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                IsSmoker = model.IsSmoker,
                DateOfBirth = model.DateOfBirth,
                Age = model.Age,
                NumOfDependents = model.NumOfDependents,
                RetirementAge = model.RetirementAge,
                ExistingCoverage = model.ExistingCoverage,
                RequestedCoverage = model.CoverageAmountRequesting,
                AnnualIncome = model.AnnualIncome,
                AnnualIncomeGrowthPercentage = model.AnnualIncomeGrowthPercent,
                Addr1 = model.Addr1,
                Addr2 = model.Addr2,
                City = model.City,
                State = model.State, 
                Zip = model.Zip
            };

            await ConnectedTask;
            await Proxy.Invoke("generateAndSaveIllustration", meetingId, illustrationRequest);
        }
    }
}
