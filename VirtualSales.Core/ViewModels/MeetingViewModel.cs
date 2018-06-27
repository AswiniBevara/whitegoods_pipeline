using System;
using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using VirtualSales.Core.AppServices;
using VirtualSales.Core.Models;
using VirtualSales.Core.ViewModels.Annotations;
using VirtualSales.Core.ViewModels.Tools;
using VirtualSales.Models;

namespace VirtualSales.Core.ViewModels
{

    public interface IMeetingViewModelFactory
    {
        MeetingViewModel Create(Meeting meeting);
    }

    internal class MeetingViewModelFactory : IMeetingViewModelFactory
    {
        private readonly IViewModelLocator _locator;
        private readonly ISharedDataService _sharedDataService;

        public MeetingViewModelFactory(IViewModelLocator locator, ISharedDataService sharedDataService)
        {
            _locator = locator;
            _sharedDataService = sharedDataService;
        }

        public MeetingViewModel Create(Meeting meeting)
        {
            if (meeting == null) throw new ArgumentNullException("meeting");

            return new MeetingViewModel(meeting, _locator.GetTools(), _locator.VideoConfViewModel, _sharedDataService, _locator.Mode);
        }
    }
    public class MeetingViewModel : ReactiveObject
    {
        private string _status;
        private IToolViewModel _activeTool;
        private ClientInfoViewModel _client;
        private TimeSpan _duration;
        private Guid _id;
        private DateTimeOffset _startTime;
        private VideoConfViewModel _videoConf;

        private ObservableAsPropertyHelper<string> _dayOfTheWeekProperty;
        private ObservableAsPropertyHelper<string> _dayOfTheMonthProperty;
        private ObservableAsPropertyHelper<string> _monthAndYearProperty;
        private ObservableAsPropertyHelper<string> _timeAndDurationProperty;
        private ObservableAsPropertyHelper<string> _identifierProperty;
        private UserAnnotationViewModel _annotations;
        public AppMode AppMode { get; private set; }
        public bool IsAgent { get { return AppMode == AppMode.Agent; } }

        public UserAnnotationViewModel Annotations
        {
            get { return _annotations; }
            set { this.RaiseAndSetIfChanged(ref _annotations, value); }
        }

        public MeetingViewModel(
            Meeting meeting, 
            IList<IToolViewModel> tools,
            VideoConfViewModel videoConf,
            ISharedDataService sharedData,
            AppMode appMode)
        {
            AppMode = appMode;
            Id = meeting.Id;
            _identifierProperty = this.WhenAnyValue(v => v.Id, p => p.ToString().Substring(0, 6).ToUpper()).ToProperty(this, p => p.Identifier);
            StartTime = meeting.StartTime;
            Duration = meeting.Duration;
            Client = new ClientInfoViewModel(meeting.Client);

            PopulateMeetingStatus(meeting.Status);
            Tools = tools;
            VideoConf = videoConf;

            ActiveTool = tools.FirstOrDefault();

            this.WhenAnyValue(p => p.StartTime, p => p.DateTime.ToString("dddd")).ToProperty(this, p => p.DayOfTheWeek, out _dayOfTheWeekProperty);
            this.WhenAnyValue(p => p.StartTime, p => p.DateTime.ToString("dd")).ToProperty(this, p => p.DayOfTheMonth, out _dayOfTheMonthProperty);
            this.WhenAnyValue(p => p.StartTime, p => p.DateTime.ToString("MMMM yyyy")).ToProperty(this, p => p.MonthAndYear, out _monthAndYearProperty);
            this.WhenAnyValue(p => p.StartTime, p => p.Duration, (s, d) =>
            {
                var start = s.DateTime;
                var end = s.DateTime.Add(d);

                var startStr = start.ToString(start.Minute == 0 ? " h " : "h:mm").Trim();
                var endStr = start.ToString(end.Minute == 0 ? "h tt" : "h:mm tt");

                if (start.ToString("tt") != end.ToString("tt"))
                {
                    startStr = startStr + " " + start.ToString("tt");
                }

                return string.Format("{0} - {1}", startStr, endStr);

            }).ToProperty(this, p => p.TimeAndDuration, out _timeAndDurationProperty);
        }

        public void PopulateMeetingStatus(MeetingStatus status)
        {
            if (status == null) Status = "Scheduled";
            else
            {
                if (status.MeetingStarted && status.ClientJoined) Status = "Meeting in Progress";
                else
                {
                    if (status.AgentJoined && status.ClientJoined) Status = "Waiting for agent to start meeting";
                    if (status.AgentJoined && !status.ClientJoined) Status = "Waiting for client";
                    if (status.ClientJoined && !status.AgentJoined) Status = "Waiting for agent";

                    if (!status.ClientJoined && !status.AgentJoined) Status = "Scheduled";
                }
            }

        }

        public string Status
        {
            get { return _status; }
            set { this.RaiseAndSetIfChanged(ref _status, value); }
        }

        public string TimeAndDuration {
            get { return _timeAndDurationProperty.Value; }
        }

        public string DayOfTheWeek
        {
            get { return _dayOfTheWeekProperty.Value; }
        }
        public string DayOfTheMonth
        {
            get { return _dayOfTheMonthProperty.Value; }
        }
        public string MonthAndYear
        {
            get { return _monthAndYearProperty.Value; }
        }

        public string Identifier
        {
            get { return _identifierProperty.Value; }    
        }

        public IToolViewModel ActiveTool
        {
            get { return _activeTool; }
            set { this.RaiseAndSetIfChanged(ref _activeTool, value); }
        }

        public VideoConfViewModel VideoConf
        {
            get { return _videoConf; }
            private set { this.RaiseAndSetIfChanged(ref _videoConf, value); }
        }

        public IList<IToolViewModel> Tools { get; private set; }

        public Guid Id
        {
            get { return _id; }
            private set { this.RaiseAndSetIfChanged(ref _id, value); }
        }

        public ClientInfoViewModel Client
        {
            get { return _client; }
            private set { this.RaiseAndSetIfChanged(ref _client, value); }
        }

        public DateTimeOffset StartTime
        {
            get { return _startTime; }
            private set { this.RaiseAndSetIfChanged(ref _startTime, value); }
        }

        public TimeSpan Duration
        {
            get { return _duration; }
            private set { this.RaiseAndSetIfChanged(ref _duration, value); }
        }
    }
}