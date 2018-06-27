using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;

namespace VirtualSales.Core.ViewModels.Tools
{
    public abstract class ToolViewModel : ReactiveObject, IToolViewModel
    {
        private readonly ObservableAsPropertyHelper<IToolPage> _currentPage;
        private readonly ObservableAsPropertyHelper<int> _currentPageNumber;
        private readonly BehaviorSubject<int> _currentPageIndex = new BehaviorSubject<int>(-1);
        private IDisposable _currentSub;
        private string _name;
        private IDisposable _nextPageSub;
        private readonly ReactiveList<IToolPage> _pages = new ReactiveList<IToolPage>();
        private IDisposable _prevPageSub;


        protected ToolViewModel(Guid toolId)
        {
            Pages = _pages;
            ToolId = toolId;

            // Listen for the page count changing
            var currentWithCount = _pages.CountChanged
                .CombineLatest(_currentPageIndex, (count, current) => 
                    new {Count = count, Current = current})
                .Publish(); // We have multiple listenser, so publish the value (multicast)

            // Enable the next page command when the count is less than the total number of pages
            var nextPageCmd = new ReactiveCommand(currentWithCount.Select(t => 
                                                        t.Current + 1 < t.Count &&
                                                        t.Count > 1));
           
            // When the command is invoked, call GoToPage with the next page num.
            _nextPageSub = nextPageCmd
                .RegisterAsyncAction(_ =>
                    GoToPage(_currentPageIndex.Value + 1), // _currentPageIndex stores the current value
                    Scheduler.Immediate)
                .Subscribe();

            // Enable the preious page if current is greater than 0 and there's at least two pages
            var prevPageCmd = new ReactiveCommand(currentWithCount.Select(t => 
                                                        t.Current > 0 && 
                                                        t.Count > 1));
            // When the command is invoked, call GoToPage with the previous index
            _prevPageSub = prevPageCmd
                .RegisterAsyncAction(_ =>
                    GoToPage(_currentPageIndex.Value - 1), // _currentPageIndex stores the current value
                    Scheduler.Immediate)
                .Subscribe();

            NextPageCommand = nextPageCmd;
            PreviousPageCommand = prevPageCmd;

            // When the current index changes, get the current tool and expose it as a property
            // As pages flow through, 
            _currentPageIndex
                .Select(i => i >= 0 ? _pages[i] : (IToolPage)null)
                .DistinctUntilChanged()
                .Scan((prev, current) =>
                      {
                          if (prev != null)
                              prev.OnNavigatedAway();
                          if (current != null)
                              current.OnNavigatedTo();

                          // This element will become prev on the next pass
                          return current;
                      })
                .ToProperty(this, tvm => tvm.CurrentPage, out _currentPage);

            // Expose the current page index as a property
            _currentPageIndex.ToProperty(this, tvm => tvm.CurrentPageNumber, out _currentPageNumber);

            

            // Once all of our listeners are attached, turn on the stream
            _currentSub = currentWithCount.Connect();
        }

        protected void AddToolPages(params IToolPage[] pages)
        {
            using (_pages.SuppressChangeNotifications())
            {
                _pages.AddRange(pages);
            }

            MoveToFirstPage();
        }

        public ICommand NextPageCommand { get; private set; }
        public ICommand PreviousPageCommand { get; private set; }

        public Guid ToolId { get; private set; }

        public IReadOnlyReactiveList<IToolPage> Pages { get; private set; }

        public IToolPage CurrentPage
        {
            get { return _currentPage.Value; }
        }

        public int CurrentPageNumber
        {
            get { return _currentPageNumber.Value; }
        }


        public void GoToPage(int index)
        {
            if (index < 0 || index > _pages.Count)
                throw new IndexOutOfRangeException();

            // Push the index into the subject -- this will cause everything else to change
            _currentPageIndex.OnNext(index);
        }

        public string Name
        {
            get { return _name; }
            protected set { this.RaiseAndSetIfChanged(ref _name, value); }
        }

        protected void MoveToFirstPage()
        {
            if (_pages.Count == 0)
                throw new InvalidOperationException("No Pages");

            GoToPage(0);
        }
    }
}