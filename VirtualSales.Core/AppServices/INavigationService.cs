using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VirtualSales.Core.AppServices;

namespace VirtualSales.Core
{
    public interface INavigationService : INotifyPropertyChanged
    {
        void Navigate(Screen screen, object parameter = null);

        IScreenViewModel CurrentViewModel { get; }
        Screen Screen { get; }

        void Initialize();
        void PopToRoot();

        ICommand BackCommand { get; }

        IObservable<Tuple<Screen, IScreenViewModel, NavigateDirection>> CurrentScreen { get; }
    }
}
