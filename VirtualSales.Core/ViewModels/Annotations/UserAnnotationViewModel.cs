using ReactiveUI;
using System.Reactive.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualSales.Core.AppServices;
using VirtualSales.Core.Models;
using VirtualSales.Core.Models.Annotations;
using VirtualSales.Models.Annotations;

namespace VirtualSales.Core.ViewModels.Annotations
{
    /// <summary>
    ///  Base class for the annotations view model for both the client and agent app
    /// </summary>
    public abstract class UserAnnotationViewModel : ReactiveObject
    {
        private AnnotationsModel _annotationsModel;
        public MeetingViewModel Meeting { get; private set; }
        private IDisposable _sizeChangedSub;
        private IDisposable _collectionRefreshSub;

        public UserAnnotationViewModel(ISharedDataService sharedDataService, MeetingViewModel meeting)
        {
            Meeting = meeting;
            AnnotationsModel = sharedDataService.AnnotationsModel;
            SurfaceSize = new AnnotationSurfaceSize();

            var activeToolObs = Meeting.WhenAnyValue(vm => vm.ActiveTool)
               .Where(t => t != null);

            var pageChangedObs = activeToolObs
                             .Select(t => t.WhenAnyValue(v => v.CurrentPageNumber,
                                                p => new ToolIdAndPageNumber { ToolId = t.ToolId, PageNumber = p })
                                    )
                             .Switch(); // Only listen to the most recent sequence of property changes (active tool)

            // Every time the tool changes, select the tool id and current page number
            var toolChangedObs = activeToolObs
                .Select(t => new ToolIdAndPageNumber { ToolId = t.ToolId, PageNumber = t.CurrentPageNumber });
               
            // Merge them together - tool changes and current page of the tool
            _collectionRefreshSub = toolChangedObs
                .Merge(pageChangedObs)
                .Subscribe(t => this.RaisePropertyChanged("Annotations"));
            // whenever the suface size's width or height changes, re-bind the annotations
            // on the agent's side, this will send stuff over the wire. need to figure out how to solve this.
            _sizeChangedSub = this.SurfaceSize
                .WhenAnyValue(p => p.Width, p => p.Height, (w, h) => true)
                .Subscribe(p => this.RaisePropertyChanged("Annotations"));
        }

        public AnnotationBuilder CreateBuilder(AnnotationType type)
        {
            return AnnotationBuilder.Create(type, SurfaceSize, Meeting.ActiveTool.CurrentPageNumber, Meeting.ActiveTool.ToolId);
        }
        
        public AnnotationViewModel CreateAnnotationViewModel(Annotation annotation)
        {
            return AnnotationViewModel.Create(annotation, SurfaceSize);
        }

        /// <summary>
        /// the shared annotations model
        /// </summary>
        public AnnotationsModel AnnotationsModel
        {
            get { return _annotationsModel; }
            private set { this.RaiseAndSetIfChanged(ref _annotationsModel, value); }
        }

        /// <summary>
        /// The app's annotation surface size, whether it is the client or the agent. this property is passed
        /// into various parts of the application so shouldn't be re-instantiated. instead the Height/Width 
        /// properties can be changed.
        /// </summary>
        public AnnotationSurfaceSize SurfaceSize
        {
            get;
            private set;
        }

        /// <summary>
        /// the annotations collection the underlying view should bind to 
        /// </summary>
        public abstract IEnumerable<AnnotationViewModel> Annotations
        {
            get;
        }

        private bool _disposed;
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                Dispose(true);

                GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _sizeChangedSub.Dispose();
                _collectionRefreshSub.Dispose();

                _sizeChangedSub = null;
                _collectionRefreshSub = null;
            }
        }

        protected bool IsInCurrentPage(AnnotationViewModel annotation)
        {
            return IsInCurrentPage(annotation.Annotation);
        }
        protected bool IsInCurrentPage(Annotation annotation)
        {
            var activeToolId = Meeting.ActiveTool.ToolId;
            var currentPage = Meeting.ActiveTool.CurrentPageNumber;
            return annotation.ToolId == activeToolId && annotation.PageNumber == currentPage;
        }
    }
}
