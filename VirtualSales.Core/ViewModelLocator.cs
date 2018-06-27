using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using TinyIoC;
using VirtualSales.Core.AppServices;
using VirtualSales.Core.ViewModels;
using VirtualSales.Core.ViewModels.Tools;

namespace VirtualSales.Core
{
    public abstract class ViewModelLocator : IViewModelLocator
    {

        private readonly TinyIoCContainer _container;

        protected TinyIoCContainer Container
        {
            get { return _container; }
        }

        public AppMode Mode { get; private set; }

        protected ViewModelLocator(bool isAgent)
        {
            _container = new TinyIoCContainer();
            var assms = new [] { typeof(ViewModelLocator).GetTypeInfo().Assembly };
            var typeinf = typeof(IToolViewModel).GetTypeInfo();
            if (isAgent)
            {
                var src = typeof(ClientMainService);
                _container.AutoRegister(assms, t => t != src && !typeinf.IsAssignableFrom(t.GetTypeInfo())); // skip the client service in agent mode
                Mode = AppMode.Agent;
            }
            else
            {
                var arc = typeof(AgentMainService);
                _container.AutoRegister(assms, t => t != arc && !typeinf.IsAssignableFrom(t.GetTypeInfo())); // skip the agent service in client mode
                Mode = AppMode.Client; 
            }

            _container.Register<IViewModelLocator>(this);

            var tools = new []
            {
                //typeof(SampleToolViewModel),
                typeof(TypesOfInsuranceViewModel),
                typeof(DividendInterestRatesViewModel),
                typeof(BasicInformationToolViewModel),
                typeof(LifetimeEconomicValueViewModel),
                //typeof(AnotherSampleToolViewModel),
             
            };

            
            _container.RegisterMultiple<IToolViewModel>(tools).AsMultiInstance();


            InitializeTypes();

            _container.Resolve<INavigationService>().Initialize();
        }

        public IList<IToolViewModel> GetTools()
        {
            var tools = _container.ResolveAll<IToolViewModel>();

            return new ReactiveList<IToolViewModel>(tools.ToList());
        }

        protected abstract void InitializeTypes();


        public T GetService<T>() where T : class
        {
            return _container.Resolve<T>();
        }

        public IMainService MainService
        {
            get { return _container.Resolve<IMainService>(); }
        }

        public ISharedDataService SharedDataService
        {
            get { return _container.Resolve<ISharedDataService>(); }
        }

        public AgentMeetingViewModel AgentMeetingViewModel
        {
            get { return _container.Resolve<AgentMeetingViewModel>(); }
        }

        private NavigationPaneViewModel _navigationPaneViewModel;
        public NavigationPaneViewModel NavigationPaneViewModel
        {
            get
            {
                if (_navigationPaneViewModel == null)
                    _navigationPaneViewModel = _container.Resolve<NavigationPaneViewModel>();

                return _navigationPaneViewModel;
            }
        }

        public MeetingListViewModel MeetingListViewModel
        {
            get { return _container.Resolve<MeetingListViewModel>(); }
        }

        public LoginViewModel LoginViewModel
        {
            get { return _container.Resolve<LoginViewModel>(); }
        }

        public ClientLobbyViewModel LobbyViewModel
        {
            get { return _container.Resolve<ClientLobbyViewModel>(); }
        }

        public INavigationService NavigationService
        {
            get { return _container.Resolve<INavigationService>(); }
        }

        public ClientMeetingViewModel ClientMeetingViewModel
        {
            get { return _container.Resolve<ClientMeetingViewModel>(); }
        }

        public VideoConfViewModel VideoConfViewModel
        {
            get { return _container.Resolve<VideoConfViewModel>(); }
        }

    }

    public enum AppMode
    {
        Client,
        Agent
    }
    public interface IViewModelLocator
    {
        AppMode Mode { get; }
        IMainService MainService { get; }
        MeetingListViewModel MeetingListViewModel { get; }
        LoginViewModel LoginViewModel { get; }
        AgentMeetingViewModel AgentMeetingViewModel { get; }
        ClientMeetingViewModel ClientMeetingViewModel { get; }
        ClientLobbyViewModel LobbyViewModel { get; }
        VideoConfViewModel VideoConfViewModel { get; }
        NavigationPaneViewModel NavigationPaneViewModel { get; }

        ISharedDataService SharedDataService { get; }
        INavigationService NavigationService { get; }

        T GetService<T>() where T : class;

        IList<IToolViewModel> GetTools();
    }
}
