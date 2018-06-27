using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualSales.Core.Models;
using VirtualSales.Core.SignalR;

namespace VirtualSales.Core.AppServices
{
    class ClientMainService : IMainService
    {
        private readonly IClientConnection _connection;
        private readonly ISharedDataService _data;
        private readonly ISettings _settings;
        private readonly IPropertyChangedReceiver _receiver;
        private readonly IPropertyChangedTransmitter _transmitter;
        private readonly INavigationService _navigation;


        public ClientMainService(IClientConnection connection,
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
            // Register these two objects for change tranmsission
            _receiver.ProxyChangesToInstance(_data.MeetingState);
            _receiver.ProxyChangesToInstance(_data.Person);
            _receiver.ProxyChangesToInstance(_data.AnnotationsModel);
            await _connection.Connect();
        }
    }
}
