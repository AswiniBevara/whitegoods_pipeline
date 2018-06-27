using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using VirtualSales.Core.AppServices;
namespace VirtualSales.Core.ViewModels
{
    public class LoginViewModel : ScreenViewModel
    {
        private readonly INavigationService _service;
        private string _username;
        private string _password;
        private IDisposable _loginsub; 
        
        public LoginViewModel(INavigationService service)
        {
           _service = service;


            // Enable the login command when both username and password
            // have non emtpy/whitespace values in them
            var canLogin = this.WhenAnyValue(
                lvm => lvm.Username, 
                lvm => lvm.Password, 
                (u, p) => !string.IsNullOrWhiteSpace(u) && !string.IsNullOrWhiteSpace(p));
            var cmd = new ReactiveCommand(canLogin);
            // call login when invoked
            _loginsub = cmd.RegisterAsyncTask(_ => Login()).Subscribe();

            
            LoginCommand = cmd;

            Title = "Login";

        }

        public ICommand LoginCommand { get; private set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _loginsub.Dispose();
                _loginsub = null;
            }
            base.Dispose(disposing);
        }

        private async Task<bool> Login()
        {
            await Task.Delay(100);

            _service.Navigate(Screen.MeetingList);
            return true; // always return true
        }

        public string Username
        {
            get { return _username; }
            set
            {
                this.RaiseAndSetIfChanged(ref _username, value);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                this.RaiseAndSetIfChanged(ref _password, value);
            }
        }
    }
}
