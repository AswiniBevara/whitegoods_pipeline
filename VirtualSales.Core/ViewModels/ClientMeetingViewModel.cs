using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using VirtualSales.Core.AppServices;
using VirtualSales.Core.SignalR;
using VirtualSales.Core.ViewModels.Annotations;
using System.Windows.Input;

namespace VirtualSales.Core.ViewModels
{
    public class ClientMeetingViewModel : ScreenViewModel
    {
        private readonly IClientConnection _clientConnection;
        private readonly INavigationService _navigation;
        private MeetingViewModel _meeting;
        private IDisposable _navSub;
        private IDisposable _pdfAvailableSub; 
        private ISharedDataService _sharedDataService;

        public ClientMeetingViewModel(
            IClientConnection clientConnection,
            INavigationService navigation,
            ISharedDataService sharedDataService,
            IPlatformServices platformServices)
        {
            _sharedDataService = sharedDataService;
            _clientConnection = clientConnection;
            _navigation = navigation;

            // Combine the meeting details and IsActive to know when to go back
            // We should go back when we are the active screen (IsActive == true)
            // and the Meeting Details are cleared out - will be null.
            _navSub = _clientConnection.MeetingDetails
                .CombineLatest(this.WhenAnyValue(vm => vm.IsActive),
                                (det, act) => new { Details = det, IsActive = act }) // combine into anon type
                .Where(t => t.IsActive && t.Details == null)
                .Subscribe(c => _navigation.BackCommand.Execute(null));

        
            Title = "Welcome to White Label Insurance";

            // Listen for state changes from the shared model
            // and return the non-null ids
            var stateObs = sharedDataService.MeetingState
                .WhenAnyValue(s => s.State)
                .Where(s => s.HasValue) 
                .Select(s => s.Value); // de-Nullable<T> the value since we're not null

            // Get the latest non-null meeting
            var meetingObs = this.ObservableForProperty(vm => vm.Meeting, m => m)
                .Where(v => v != null);

            // Get the latest MeetingState and combine it with the current Meeting
            meetingObs
                .CombineLatest(stateObs, Tuple.Create) // store the latest in a tuple
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(t =>
                    {
                        // for clarity, extract the tuple
                        var state = t.Item2; 
                        var meeting = t.Item1;

                        // Fet the specififed tool instance by id
                        var activeTool = meeting.Tools.First(tool => tool.ToolId == state.ToolId);

                        // Navigate to the current page within the tool
                        activeTool.GoToPage(state.PageNumber);

                        // Finally, set the tool to be the active one for the meeting
                        meeting.ActiveTool = activeTool;
                    }
                );


            var leaveMeetingCommand = new ReactiveCommand();
            leaveMeetingCommand.Subscribe(_ => _navigation.BackCommand.Execute(null));
            LeaveMeetingCommand = leaveMeetingCommand;

            _pdfAvailableSub = _clientConnection.PdfAvailable.ObserveOn(RxApp.MainThreadScheduler).Subscribe(async p =>
            {
                var result = await _clientConnection.GetIllustrationPdfAsync(p);
                await platformServices.SaveAndLaunchFile(new System.IO.MemoryStream(result), "pdf");
            });

        }
        protected override void OnNavigatedTo(NavigateDirection direction, object parameter)
        {
            base.OnNavigatedTo(direction, parameter);

            if (direction == NavigateDirection.Forward)
            {
                Meeting = (MeetingViewModel)parameter;
                Meeting.Annotations = new ClientAnnotationViewModel(_sharedDataService, Meeting);
            }
        }

        public ICommand LeaveMeetingCommand
        {
            get;
            private set;
        }

        protected override void OnNavigatedAway()
        {
            Meeting.VideoConf.Config = null;

            if (Meeting.Annotations != null)
            {
                Meeting.Annotations.Dispose();
                Meeting.Annotations = null;
            }
            Debug.WriteLine("Leaving meeting");
            _clientConnection.LeaveMeeting(Meeting.Id);
            base.OnNavigatedAway();
        }

        public MeetingViewModel Meeting
        {
            get { return _meeting; }
            private set { this.RaiseAndSetIfChanged(ref _meeting, value); }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _navSub.Dispose();
                _pdfAvailableSub.Dispose();

                _navSub = null;
                _pdfAvailableSub = null;
            }
            base.Dispose(disposing);
        }
    }
}