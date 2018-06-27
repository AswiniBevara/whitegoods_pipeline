using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using VirtualSales.Core.AppServices;
using VirtualSales.Core.SignalR;

namespace VirtualSales.Core.ViewModels
{
    public class ClientLobbyViewModel : ScreenViewModel
    {
        private readonly IClientConnection _clientConnection;
        private readonly INavigationService _navigation;
        private IDisposable _joinMeetingSub;
        private MeetingViewModel _meeting;
        private string _meetingId;
        private IDisposable _navSub;
        private IDisposable _titleSub;
        private bool _waitingForAgent;
        private bool _connectedToMeeting;
        private bool _meetingJoined;
        private string _error;
        private bool _requestInProgress;

        public ClientLobbyViewModel(IClientConnection clientConnection,
                                    INavigationService navigation,
                                    IMeetingViewModelFactory meetingFactory)
        {
            _clientConnection = clientConnection;
            _navigation = navigation;


            // This command should be available when we're not already waiting
            // but not before we get the Meeting data back.
            // Check for a non null meeting and make sure WaitingForAgent is false
            var cmd = new ReactiveCommand(this.WhenAnyValue(
                vm => vm.WaitingForAgent,
                vm => vm.Meeting,
                (wait, mtg) => !wait && mtg != null));

            // This is the navigation function.
            // We want to go to the ClientMeeting screen once the following conditions are met:
            // 1) We are the Active screen (IsActive == true)
            // 2) We have the meeting VM back (this gets set as a result of the previous call)
            // 3) We get the MeetingDetails (non-null). This means the agent hit Start Meeting
            //
            // Combine the three things we're observing into an anon type
            // Then check them against our conditions
            // If all true, then navigate
            _navSub = _clientConnection.MeetingDetails
                                       .CombineLatest(this.WhenAnyValue(vm => vm.IsActive),
                                                      this.WhenAnyValue(vm => vm.Meeting),
                                                      (det, act, mtg) => new { Details = det, IsActive = act, Meeting = mtg })
                .Where(t => t.IsActive && t.Details != null && t.Meeting != null)
                                       .Subscribe(c =>
                                       {
                                           if (c.Details.MeetingId == Meeting.Id)
                                           {
                                               Meeting.VideoConf.Config = c.Details;
                                               _navigation.Navigate(Screen.ClientMeeting, Meeting);
                                           }
                                       });

            // The command's action will set the WaitingForAgent to true so we can't click
            // Join again. It will then send the join message to the server
            _joinMeetingSub = cmd.RegisterAsyncAction(p =>
                                                      {
                                                          WaitingForAgent = true;
                                                          ConnectedToMeeting = true;
                                                          MeetingJoined = true;
                                                          _clientConnection.JoinMeeting(Meeting.Id);
                                                      }).Subscribe();

            JoinMeetingCommand = cmd;


            var connectCmd = new ReactiveCommand(this.WhenAnyValue(p => p.MeetingId, p => p.ConnectedToMeeting, p => p.RequestInProgress, (id, connected, requestInProgress) => !string.IsNullOrEmpty(id) && id.Length == 6 && !connected && !requestInProgress));
            connectCmd.RegisterAsyncAction(p =>
                                           {
                                               RequestInProgress = true;
                                               // Kick off the GetMeetingDetails call
                                               // Normally you can't await a task in a ctor, but
                                               // Rx solves that by turning it into an Observable.
                                               // When we get the meeting DTO back, turn it into a MeetingVM
                                               // and expose it as a property here.
                                               clientConnection.GetMeetingDetails(MeetingId)
                                                               .ToObservable()
                                                               .ObserveOn(RxApp.MainThreadScheduler)
                                                               .Select(p1 => p1 == null ? null : meetingFactory.Create(p1))
                                                               .Subscribe(p1 =>
                                                                          {
                                                                              Error = string.Empty;
                                                                              if (p1 == null)
                                                                              {
                                                                                  Error = "Meeting with id " + MeetingId + " does not exist.";
                                                                                  return; // nothing, meeting doesn't exist
                                                                              }
                                                                              Meeting = p1;
                                                                              RequestInProgress = false;
                                                                              JoinMeetingCommand.Execute(null);
                                                                          });


                                           });
            ConnectCommand = connectCmd;

            var disconnectCmd = new ReactiveCommand(this.WhenAnyValue(p => p.ConnectedToMeeting, p => p.RequestInProgress, (connected,requestInProgress) => connected && !requestInProgress));
            disconnectCmd.RegisterAsyncAction(p =>
                                              {
                                                  WaitingForAgent = false;
                                                  ConnectedToMeeting = false;
                                                  if (MeetingJoined)
                                                  {
                                                      _clientConnection.LeaveMeeting(Meeting.Id);
                                                      MeetingJoined = false;
                                                  }

                                                  _clientConnection.LeaveMeeting(Meeting.Id);
                                                  Meeting = null;
                                              });


            DisconnectCommand = disconnectCmd;

            // Update the title based on the waiting status. Once we press the button, go into
            // "waiting" mode.
            _titleSub = this.WhenAny(v => v.WaitingForAgent, v => v.Meeting,
                                     (waiting, meeting) =>
                                     {
                                         if (meeting.Value == null) return "Please connect to a meeting";
                                         return waiting.Value ? "Waiting for agent to start the meeting" : "Please join the meeting.";
                                     })
                            .ObserveOn(RxApp.MainThreadScheduler)
                            .Subscribe(text => Title = text);

            MeetingId = "64BDF0"; // default to the first meeting for easy testing
        }

        public string MeetingId
        {
            get { return _meetingId; }
            set { this.RaiseAndSetIfChanged(ref _meetingId, value); }
        }

        public MeetingViewModel Meeting
        {
            get { return _meeting; }
            set { this.RaiseAndSetIfChanged(ref _meeting, value); }
        }

        public bool WaitingForAgent
        {
            get { return _waitingForAgent; }
            private set { this.RaiseAndSetIfChanged(ref _waitingForAgent, value); }
        }
        public bool ConnectedToMeeting
        {
            get { return _connectedToMeeting ; }
            private set { this.RaiseAndSetIfChanged(ref _connectedToMeeting, value); }
        }
        public bool MeetingJoined
        {
            get { return _meetingJoined; }
            private set { this.RaiseAndSetIfChanged(ref _meetingJoined, value); }
        }

        public bool RequestInProgress
        {
            get { return _requestInProgress; }
            private set { this.RaiseAndSetIfChanged(ref _requestInProgress, value); }
        }
        public string Error
        {
            get { return _error; }
            private set { this.RaiseAndSetIfChanged(ref _error, value); }
        }

        public ICommand JoinMeetingCommand { get; private set; }
        public ICommand ConnectCommand { get; private set; }
        public ICommand DisconnectCommand { get; private set; }

        protected override void OnNavigatedTo(NavigateDirection direction, object parameter)
        {
            base.OnNavigatedTo(direction, parameter);

            WaitingForAgent = false; // reset status to false however we get here
            Error = string.Empty;
            MeetingJoined = false;
            RequestInProgress = false;
            ConnectedToMeeting = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _navSub.Dispose();
                _navSub = null;

                _joinMeetingSub.Dispose();
                _joinMeetingSub = null;

                _titleSub.Dispose();
                _titleSub = null;
            }
            base.Dispose(disposing);
        }
    }
}