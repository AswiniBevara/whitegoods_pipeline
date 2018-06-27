using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;

namespace VirtualSales.Core.ViewModels.Tools
{
    public interface IToolViewModel : IReactiveNotifyPropertyChanged
    {
        ICommand NextPageCommand { get; }
        ICommand PreviousPageCommand { get; }
        IReadOnlyReactiveList<IToolPage> Pages { get; }
        IToolPage CurrentPage { get; }
        string Name { get; }
        Guid ToolId { get; }
        int CurrentPageNumber { get; }

        /// <summary>
        /// used the the receiver to push the current page to the meeting
        /// </summary>
        void GoToPage(int index);
    }
}
