using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace VirtualSales.Core.ViewModels.Tools
{
    public abstract class ToolPage : ReactiveObject, IToolPage
    {
        private bool _isActive;

        public bool IsActive
        {
            get { return _isActive; }
            private set { this.RaiseAndSetIfChanged(ref _isActive, value); }
        }

        protected virtual void OnNavigatedTo()
        {

        }

        protected virtual void OnNavigatedAway()
        {

        }

        void IToolPage.OnNavigatedTo()
        {
            IsActive = true;

            OnNavigatedTo();
        }

        void IToolPage.OnNavigatedAway()
        {
            IsActive = false;
            OnNavigatedAway();
        }
    }
}
