using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualSales.Core.Models;
using VirtualSales.Core.SignalR;

namespace VirtualSales.Core.AppServices
{
    class AgentMainService : IMainService
    {
        private readonly IAgentConnection _connection;
        private readonly ISharedDataService _data;
        private readonly ISettings _settings;
        private readonly IPropertyChangedReceiver _receiver;
        private readonly IPropertyChangedTransmitter _transmitter;
        private readonly INavigationService _navigation;

        public AgentMainService(IAgentConnection connection, 
                                ISharedDataService data, 
                                ISettings settings,
                                IPropertyChangedTransmitter transmitter, 
                                IPropertyChangedReceiver receiver,
                                INavigationService navigation)
        {
            _connection = connection;
            _data = data;
            _settings = settings;
            _transmitter = transmitter;
            _receiver = receiver;
            _navigation = navigation;
        }

        public async Task StartApp()
        {
            _transmitter.RegisterForChanges(_data.MeetingState);
            _transmitter.RegisterForChanges(_data.Person);
            _transmitter.RegisterForChanges(_data.AnnotationsModel);
            await _connection.Connect();
        }
    }
}
