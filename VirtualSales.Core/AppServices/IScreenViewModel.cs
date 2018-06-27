using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace VirtualSales.Core.AppServices
{
    public interface IScreenViewModel : IReactiveNotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction">If direction is Back, parameter will be null</param>
        /// <param name="parameter"></param>
        void OnNavigatedTo(NavigateDirection direction, object parameter = null);

        void OnNavigatedAway();

        string Title { get; }

    }
}
