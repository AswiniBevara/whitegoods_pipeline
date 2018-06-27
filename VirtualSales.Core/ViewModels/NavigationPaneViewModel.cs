using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Reactive.Linq;

namespace VirtualSales.Core.ViewModels
{
    public class NavigationPaneViewModel : ReactiveObject
    {
        private bool _isToolLibraryButtonHidden;
        private bool _isUpcomingMeetingsButtonHidden;

        public NavigationPaneViewModel(INavigationService navigation)
        {
            IsToolLibraryButtonHidden = true;
            IsUpcomingMeetingsButtonHidden = true;

            var invokeToolLibraryCommand = new ReactiveCommand(this.WhenAnyValue(p => p.IsToolLibraryButtonHidden, p => !p));
            invokeToolLibraryCommand.ObserveOn(RxApp.MainThreadScheduler).Subscribe(p =>
            {
                if (ShowToolLibraryAction != null && !IsToolLibraryButtonHidden)
                {
                    HideNavPaneCommand.Execute(null);
                    ShowToolLibraryAction();
                }
            });
            InvokeToolLibraryCommand = invokeToolLibraryCommand;

            var invokeUpcomingMeetingsCommand = new ReactiveCommand(this.WhenAnyValue(p => p.IsUpcomingMeetingsButtonHidden, p => !p));
            invokeUpcomingMeetingsCommand.ObserveOn(RxApp.MainThreadScheduler).Subscribe(p =>
            {
                if (!IsUpcomingMeetingsButtonHidden)
                {
                    HideNavPaneCommand.Execute(null);
                    navigation.BackCommand.Execute(null);
                }
            });
            InvokeUpcomingMeetingsCommand = invokeUpcomingMeetingsCommand;

            var showSettingsCommand = new ReactiveCommand();
            //showSettingsCommand.RegisterAsync(async p =>
            //{

            //});
            ShowSettingsCommand = showSettingsCommand;

            var showPrivacyPolicyCommand = new ReactiveCommand();
            //showPrivacyPolicyCommand.RegisterAsync(async p =>
            //{

            //});
            ShowPrivacyPolicyCommand = showPrivacyPolicyCommand;

            var hideNavPaneCommand = new ReactiveCommand();
            hideNavPaneCommand.ObserveOn(RxApp.MainThreadScheduler)
                                    .Subscribe(p =>
                                               {
                                                   if (HideNavPaneAction != null) HideNavPaneAction();
                                               });
            HideNavPaneCommand = hideNavPaneCommand;

            var logoutCommand = new ReactiveCommand();
            logoutCommand.ObserveOn(RxApp.MainThreadScheduler).Subscribe(p => navigation.PopToRoot());
            LogoutCommand = logoutCommand;
        }

        public Action ShowToolLibraryAction { get; set; }
        public Action HideNavPaneAction { get; set; }

        public bool IsUpcomingMeetingsButtonHidden
        {
            get { return _isUpcomingMeetingsButtonHidden; }
            set { this.RaiseAndSetIfChanged(ref _isUpcomingMeetingsButtonHidden, value); }            
        }

        public bool IsToolLibraryButtonHidden
        {
            get { return _isToolLibraryButtonHidden; }
            set { this.RaiseAndSetIfChanged(ref _isToolLibraryButtonHidden, value); }
        }

        public ICommand HideNavPaneCommand { get; private set; }
        public ICommand InvokeToolLibraryCommand { get; private set; }
        public ICommand InvokeUpcomingMeetingsCommand { get; private set; }
        public ICommand ShowSettingsCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }
        public ICommand ShowPrivacyPolicyCommand { get; private set; }
    }
}
