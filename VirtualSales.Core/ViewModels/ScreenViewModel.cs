using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using VirtualSales.Core.AppServices;

namespace VirtualSales.Core.ViewModels
{
    public abstract class ScreenViewModel :  ReactiveObject, IScreenViewModel
    {
        private bool _disposed;
        private string _title;
        private bool _isActive;

        /// <summary>
        /// Called anytime the screen is about to become active.
        /// </summary>
        /// <param name="direction">If direction is Back, parameter will be null</param>
        /// <param name="parameter"></param>
        protected virtual void OnNavigatedTo(NavigateDirection direction, object parameter)
        {
            
        }

        protected virtual void OnNavigatedAway()
        {
            
        }

        void IScreenViewModel.OnNavigatedTo(NavigateDirection direction, object parameter)
        {
            IsActive = true;

            OnNavigatedTo(direction, parameter);
        }

        void IScreenViewModel.OnNavigatedAway()
        {
            IsActive = false;
            OnNavigatedAway();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                Dispose(true);

                GC.SuppressFinalize(this);
            }
        }

        public bool IsActive
        {
            get { return _isActive; }
            private set
            {
                this.RaiseAndSetIfChanged(ref _isActive, value);
            }
        }


        public string Title
        {
            get { return _title; }
            protected set
            {
                this.RaiseAndSetIfChanged(ref _title, value);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            
        }

        ~ScreenViewModel()
        {
            Dispose(false);
        }
    }
}
 