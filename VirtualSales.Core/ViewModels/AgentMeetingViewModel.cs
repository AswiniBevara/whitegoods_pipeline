using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using VirtualSales.Core.AppServices;
using VirtualSales.Core.Models;
using VirtualSales.Core.SignalR;
using VirtualSales.Core.Models.Annotations;
using VirtualSales.Models.Annotations;
using VirtualSales.Core.ViewModels.Annotations;

namespace VirtualSales.Core.ViewModels
{
    public class AgentMeetingViewModel : ScreenViewModel
    {
        private readonly IAgentConnection _agentConnection;
        private readonly ObservableAsPropertyHelper<bool> _isClientInMeeting;
        private IDisposable _startMeetingSub;
        private IDisposable _endMeetingSub;
        private IDisposable _titleSub;
        private IDisposable _pdfSub;
        private IDisposable _pdfAvailableSub;
        private IDisposable _meetingStateSub;
        private IDisposable _meetingStatusSub;
        private ISharedDataService _sharedDataService;

        private MeetingViewModel _meetingViewModel;
        private bool _meetingStarted;
        private string _meetingStatus;

        public NavigationPaneViewModel NavigationPane { get; private set; }
        public AgentMeetingViewModel(
            IAgentConnection agentConnection, 
            ISharedDataService sharedDataService,
            INavigationService navigation,
            IViewModelLocator locator,
            IPlatformServices platformServices)
        {
            _sharedDataService = sharedDataService;
            _agentConnection = agentConnection;
            NavigationPane = locator.NavigationPaneViewModel;

            var startcmd = new ReactiveCommand(this.WhenAnyValue(vm => vm.MeetingStarted, vm => vm.Meeting.VideoConf.VideoInitCompleted, vm => vm.Meeting, (started, video, mtg) => !started && mtg != null));
            _startMeetingSub = startcmd.RegisterAsyncTask(async _ =>
                                                                {
                                    MeetingStarted = true;
                                    Meeting.VideoConf.Config = await _agentConnection.StartMeeting(Meeting.Id);
                                }).Subscribe();

            var endcmd = new ReactiveCommand(this.WhenAnyValue(vm => vm.MeetingStarted, vm => vm.Meeting.VideoConf.VideoInitCompleted, (meetingStarted, videoStarted) => meetingStarted && videoStarted));
            _endMeetingSub = endcmd.RegisterAsyncAction(_ => navigation.BackCommand.Execute(null)).Subscribe();

            StartMeetingCommand = startcmd;
            EndMeetingCommand = endcmd;

            _agentConnection.ClientInMeeting.ToProperty(this, t => t.IsClientInMeeting, out _isClientInMeeting);

            
            var savePdfCmd = new ReactiveCommand(this.WhenAnyValue(vm => vm.MeetingStarted, vm => vm.Meeting.VideoConf.VideoInitCompleted, (meetingStarted, videoStarted) => meetingStarted && videoStarted));
            _pdfSub = savePdfCmd.RegisterAsyncTask(async _ =>
                                                         {
                                                            await _agentConnection.GenerateIllustration(Meeting.Id, _sharedDataService.Person);
                                               }).Subscribe();

            _pdfAvailableSub = _agentConnection.PdfAvailable.ObserveOn(RxApp.MainThreadScheduler).Subscribe(async p =>
                                                    {
                                                        var result = await _agentConnection.GetIllustrationPdfAsync(p);
                                                        await platformServices.SaveAndLaunchFile(new System.IO.MemoryStream(result), "pdf");
                                                    });

            SavePdfCommand = savePdfCmd;


            _titleSub = this.WhenAnyValue(
                vm => vm.Meeting.Client,
                (client) =>
                string.Format("Meeting with {0}",
                              client.FullName)
                )
                            .ObserveOn(RxApp.MainThreadScheduler)
                            .Subscribe(t => Title = t);

            MeetingStatus = GetMeetingStatusString(IsClientInMeeting, MeetingStarted);
            _meetingStatusSub = this.WhenAnyValue(
                         vm => vm.MeetingStarted,
                         vm => vm.IsClientInMeeting,
                         (started, clientIn) => GetMeetingStatusString(clientIn, started))
                         .ObserveOn(RxApp.MainThreadScheduler)
                         .Subscribe(t => MeetingStatus = t);

            //_titleSub = this.WhenAnyValue(
            //             vm => vm.Meeting.Client,
            //             vm => vm.MeetingStarted,
            //             vm => vm.IsClientInMeeting,
            //             (client, started, clientIn) =>
            //                 string.Format("Meeting with {0}. Started: {1} Client Joined: {2}",
            //                    client.FullName, started, clientIn)
            //             )
            //             .ObserveOn(RxApp.MainThreadScheduler)
            //             .Subscribe(t => Title = t);


            // When the meeting's active tool changes, set the Tool and page number
            // into the shared state so it'll be propagated.

            // Get an observable for the currently set tool
            var activeToolObs = this.WhenAnyValue(vm => vm.Meeting.ActiveTool)
                .Where(t => t != null);

            // Every time the tool changes, watch the new tool's CurrentPageNumber
            // for changes.
            //
            // When we get a change, convert that into a ToolIdAndPageNumber, bringing in
            // the owning tool id.
            var pageChangedObs = activeToolObs
                 .Select(t => t.WhenAnyValue(v => v.CurrentPageNumber, 
                                    p => new ToolIdAndPageNumber { ToolId = t.ToolId, PageNumber = p})
                        )
                 .Switch() // Only listen to the most recent sequence of property changes (active tool)
                 .Log(this, "Current Page Changed", t => string.Format("Tool: {0}, Page: {1}", t.ToolId, t.PageNumber));
         
            // Every time the tool changes, select the tool id and current page number
            var toolChangedObs = activeToolObs
                .Select(t => new ToolIdAndPageNumber { ToolId = t.ToolId, PageNumber = t.CurrentPageNumber })
                .Log(this, "Tool Changed", t => string.Format("Tool: {0}, Page: {1}", t.ToolId, t.PageNumber));

            
            // Merge them together - tool changes and current page of the tool
            _meetingStateSub = toolChangedObs
                .Merge(pageChangedObs)
                .Subscribe(t => sharedDataService.MeetingState.State = t);

        }

        private string GetMeetingStatusString(bool isClientInMeeting, bool isMeetingStarted)
        {
            if (isMeetingStarted && isClientInMeeting) return "Meeting in progress";
            if (!isClientInMeeting) return "Waiting for client to join ...";
            //if (!started && !clientIn) return "Waiting for client to join";
            else return "Waiting for meeting to start ...";
        }

        public ICommand SavePdfCommand { get; private set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _startMeetingSub.Dispose();
                _endMeetingSub.Dispose();
                _titleSub.Dispose();
                _pdfSub.Dispose();
                _meetingStateSub.Dispose();
                _meetingStatusSub.Dispose();
                _pdfAvailableSub.Dispose();

                _pdfAvailableSub = null;
                _startMeetingSub = null;
                _endMeetingSub = null;
                _titleSub = null;
                _pdfSub = null;
                _meetingStateSub = null;
                _meetingStatusSub = null;
            }

            base.Dispose(disposing);
        }

        protected override void OnNavigatedTo(NavigateDirection direction, object parameter)
        {
            if (direction == NavigateDirection.Forward)
            {
                Meeting = (MeetingViewModel)parameter;
                Meeting.Annotations = new AgentAnnotationViewModel(_sharedDataService, Meeting);
                _agentConnection.ObserveRoom(Meeting.Id);
            }
            NavigationPane.IsToolLibraryButtonHidden = false;
            NavigationPane.IsUpcomingMeetingsButtonHidden = false;
        }

        protected override async void OnNavigatedAway()
        {
            _sharedDataService.AnnotationsModel.Annotations = new ReactiveList<Annotation>(); // clear out the annotations
            NavigationPane.IsToolLibraryButtonHidden = true;
            NavigationPane.IsUpcomingMeetingsButtonHidden = true;

            // End the meeting if we navigate back
            await _agentConnection.EndMeeting(Meeting.Id);
            MeetingStarted = false;
            Meeting.VideoConf.Config = null;

            Meeting.Annotations.Dispose();
            Meeting.Annotations = null;


            base.OnNavigatedAway();
        }

        public MeetingViewModel Meeting
        {
            get { return _meetingViewModel; }
            set { this.RaiseAndSetIfChanged(ref _meetingViewModel, value); }
        }

        public bool MeetingStarted
        {
            get { return _meetingStarted; }
            private set { this.RaiseAndSetIfChanged(ref _meetingStarted, value); }
        }

        public bool IsClientInMeeting
        {
            get { return _isClientInMeeting.Value; }
        }

        public string MeetingStatus
        {
            get { return _meetingStatus; }
            set { this.RaiseAndSetIfChanged(ref _meetingStatus, value); }
        }

        public ICommand StartMeetingCommand { get; private set; }
        public ICommand EndMeetingCommand { get; private set; }
    }
}