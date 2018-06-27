using System;
using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using VirtualSales.Models;

namespace VirtualSales.Core.ViewModels
{
    public class ClientInfoViewModel : ReactiveObject
    {
        private readonly ClientInfo _client;
        private string _firstName, _lastName, _phoneNumber;

        public ClientInfoViewModel(ClientInfo client)
        {
            _client = client;
            
            // Create a derived property. When either first or last name changes, store
            // the combination in fullname
            _fullName = this.WhenAnyValue(
                                x => x.FirstName,
                                x => x.LastName,
                                (f, l) => f + " " + l)
                            .ToProperty(this, x => x.FullName);


            FirstName = _client.FirstName;
            LastName = _client.LastName;
            PhoneNumber = _client.PhoneNumber;
        }

        private ObservableAsPropertyHelper<string> _fullName;
        public string FullName
        {
            get { return _fullName.Value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            private set { this.RaiseAndSetIfChanged(ref _firstName, value); }
        }
        public string LastName
        {
            get { return _lastName; }
            private set { this.RaiseAndSetIfChanged(ref _lastName, value); }
        }
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            private set { this.RaiseAndSetIfChanged(ref _phoneNumber, value); }
        }
    }
}