using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using Splat;
using VirtualSales.Core.Infrastructure;

namespace VirtualSales.Core.AppServices
{
    public enum Screen
    {
        Lobby,
        Login,
        AgentMeeting,
        ClientMeeting,
        MeetingList,
    }

    public enum NavigateDirection
    {
        Forward,
        Back
    }

    /// <summary>
    ///  Note: The order in which properties are set on this type are important. It's critical
    /// That the CurrentViewModel prop is always set before the Screen to ensure that 
    /// Subscribers to Screen always have the correct VM available
    /// </summary>
    internal class NavigationService : ReactiveObject, INavigationService, IEnableLogger
    {
        private readonly IViewModelLocator _locator;
        private readonly ObservableStack<Tuple<Screen, IScreenViewModel>> _navigationStack = new ObservableStack<Tuple<Screen, IScreenViewModel>>();
        private readonly ISettings _settings;

        private readonly Dictionary<Screen, Func<IScreenViewModel>> _viewModelLocator = new Dictionary<Screen, Func<IScreenViewModel>>();

        private IDisposable _backCmdSub;
        private IScreenViewModel _currentViewModel;
        private IDisposable _poppedSub;
        private IDisposable _currentScreenSub;
        private Screen _screen;

        

        public NavigationService(ISettings settings,
                                 IViewModelLocator locator)
        {
            _settings = settings;
            _locator = locator;

            _viewModelLocator.Add(Screen.Login, () => locator.LoginViewModel);
            _viewModelLocator.Add(Screen.AgentMeeting, () => locator.AgentMeetingViewModel);
            _viewModelLocator.Add(Screen.MeetingList, () => locator.MeetingListViewModel);
            _viewModelLocator.Add(Screen.ClientMeeting, () => locator.ClientMeetingViewModel);
            _viewModelLocator.Add(Screen.Lobby, () => locator.LobbyViewModel);


            // Setup the backstack

            var stackChanged = Observable.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                h => _navigationStack.CollectionChanged += h,
                h => _navigationStack.CollectionChanged -= h).Select(p => p.EventArgs);

            var count = stackChanged.Select(_ => _navigationStack.Count);

            var backCmd = new ReactiveCommand(count.Select(v => v > 1));
            _backCmdSub = backCmd.RegisterAsyncAction(_ => _navigationStack.Pop(), Scheduler.Immediate).Subscribe();
            BackCommand = backCmd;

            var latestAfterPop = stackChanged
                .Where(args => args.Action == NotifyCollectionChangedAction.Remove)
                .Select(a => new
                {
                    Popped = (Tuple<Screen, IScreenViewModel>)a.OldItems[0],
                    Latest = _navigationStack.Peek()
                });


            _poppedSub = latestAfterPop
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(t =>
                           {
                               // Navigate away from the prev one
                               t.Popped.Item2.OnNavigatedAway();

                               // Nav to the prev one
                               t.Latest.Item2.OnNavigatedTo(NavigateDirection.Back);

                               CurrentViewModel = t.Latest.Item2;
                               Screen = t.Latest.Item1;

                               // Cleanup
                               t.Popped.Item2.Dispose();
                           });


            var added = stackChanged
               .Where(args => args.Action == NotifyCollectionChangedAction.Add)
               .Select(a =>
                       {
                           var t = (Tuple<Screen, IScreenViewModel>)a.NewItems[0];
                           return Tuple.Create(t.Item1, t.Item2, NavigateDirection.Forward);
                       });


            var removed = latestAfterPop.Select(t => Tuple.Create(t.Latest.Item1, t.Latest.Item2, NavigateDirection.Back));



            var connectable = added.Merge(removed).Publish();
            _currentScreenSub = connectable.Connect(); // make it hot

            CurrentScreen = connectable;
        }

        public ICommand BackCommand { get; private set; }

        public IObservable<Tuple<Screen, IScreenViewModel, NavigateDirection>> CurrentScreen
        {
            get;
            private set;
        }

        public void Initialize()
        {
            if (_settings.Mode == AppMode.Agent)
                Navigate(Screen.Login);
            else
                Navigate(Screen.Lobby);
        }

        public void Navigate(Screen screen, object parameter = null)
        {
            DoNavigate(screen, parameter);
        }

        public IScreenViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            private set { this.RaiseAndSetIfChanged(ref _currentViewModel, value); }
        }

        public Screen Screen
        {
            get { return _screen; }
            private set { this.RaiseAndSetIfChanged(ref _screen, value); }
        }

        public void PopToRoot()
        {
            _navigationStack.Clear();
            Initialize();
        }

        private void DoNavigate(Screen screen, object parameter)
        {
            var vm = _viewModelLocator[screen]();
            vm.OnNavigatedTo(NavigateDirection.Forward, parameter);

            _navigationStack.Push(Tuple.Create(screen, vm));

            CurrentViewModel = vm;
            Screen = screen;
        }
    }
}