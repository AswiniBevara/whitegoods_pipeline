using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using VirtualSales.Core.AppServices;
using VirtualSales.Core.SignalR;
using VirtualSales.Models;

namespace VirtualSales.Core.ViewModels
{
    public class MeetingListViewModel : ScreenViewModel
    {
        private readonly ReactiveList<MeetingViewModel> _meetings = new ReactiveList<MeetingViewModel>();

        private IDisposable _enterMeetingSub;
        private IDisposable _meetingUpdateSub;
        private IDisposable _meetingStatusSub;

        private MeetingViewModel _selectedMeeting;

        private void UpdateStatuses(IEnumerable<MeetingStatus> statuses)
        {
            foreach (var meeting in _meetings)
            {
                var status = statuses.FirstOrDefault(p => p.MeetingId == meeting.Id);
                meeting.PopulateMeetingStatus(status);
            }
        }
        public MeetingListViewModel(IAgentConnection connection,
                                    INavigationService navigation,
                                    IMeetingViewModelFactory meetingFactory)
        {
            // Enable the command once a meeting has been selected
            var enter = new ReactiveCommand(this.WhenAnyValue(v => v.SelectedMeeting, sm => sm != null));
            
            // Call the navigate method passing in the selected meeting
            _enterMeetingSub = enter
                .RegisterAsyncAction(_ => navigation.Navigate(Screen.AgentMeeting, SelectedMeeting))
                .Subscribe();
            EnterMeetingCommand = enter;

            _meetingStatusSub = connection.MeetingStatus.ObserveOn(RxApp.MainThreadScheduler).Subscribe(UpdateStatuses);

            // When we get a new list of meetings, add them to our list while suppressing changes
            _meetingUpdateSub = connection.Meetings
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(meetings =>
                            {
                                using (_meetings.SuppressChangeNotifications())
                                {
                                    var toAdd = meetings
                                        .Select(meetingFactory.Create)
                                        .ToArray();

                                    _meetings.Clear();
                                    _meetings.AddRange(toAdd);
                                }
                            });

            Title = "Upcoming Appointments";
        }

        public IReactiveList<MeetingViewModel> Meetings
        {
            get { return _meetings; }
        }

        public MeetingViewModel SelectedMeeting
        {
            get { return _selectedMeeting; }
            set { this.RaiseAndSetIfChanged(ref _selectedMeeting, value); }
        }

        public ICommand EnterMeetingCommand { get; private set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _enterMeetingSub.Dispose();
                _meetingUpdateSub.Dispose();
                _meetingStatusSub.Dispose();

                _enterMeetingSub = null;
                _meetingUpdateSub = null;
                _meetingStatusSub = null;
            }

            base.Dispose(disposing);
        }
    }
}