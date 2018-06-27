using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace VirtualSales.Core.ViewModels.Tools
{
    public interface IToolPage : IReactiveNotifyPropertyChanged
    {
        void OnNavigatedTo();
        void OnNavigatedAway();
    }
}
