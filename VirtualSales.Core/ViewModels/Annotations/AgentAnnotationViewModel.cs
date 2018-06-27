using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualSales.Core.AppServices;
using VirtualSales.Core.Models.Annotations;
using VirtualSales.Models.Annotations;

namespace VirtualSales.Core.ViewModels.Annotations
{
    /// <summary>
    /// represents the annotations view model in the agent app. 
    /// </summary>
    public class AgentAnnotationViewModel : UserAnnotationViewModel
    {
        private readonly ReactiveList<AnnotationViewModel> _annotations;
        private ObservableAsPropertyHelper<bool> _containsAnnotationsForCurrentPage;
        private bool _isEditing;

        private IDisposable _isEditingSub;

        /// <summary>
        /// This field and property is true when the user is creating an annotation. It is used to ensure that the
        /// annotations are sent to the client only after the user is done creating an annotation.
        /// </summary>
        public bool IsEditing
        {
            get { return _isEditing; }
            set { this.RaiseAndSetIfChanged(ref _isEditing, value); }
        }

        public AnnotationToolViewModel AnnotationTools { get; private set; }

        /// <summary>
        /// this view model has its own annotatations collection because we don't want changes to be propagated
        /// to the shared model until we force it to.
        /// </summary>
        public override IEnumerable<AnnotationViewModel> Annotations
        {
            get { return _annotations.Where(IsInCurrentPage); }
        }


        private bool AreThereAnnotationInTheCurrentPage()
        {
            return _annotations.Where(IsInCurrentPage).Count() > 0;
        }

        public AgentAnnotationViewModel(ISharedDataService sharedDataService, MeetingViewModel meeting)
            : base(sharedDataService, meeting)
        {
            _annotations = new ReactiveList<AnnotationViewModel>();

            var annotationsChangedObs = _annotations
                .WhenAnyObservable(p => p.Changed)
                .Select(p => AreThereAnnotationInTheCurrentPage());

            var activeToolObs = Meeting.WhenAnyValue(vm => vm.ActiveTool).Where(t => t != null);
            var pageChangedObs = activeToolObs
                             .Select(t => t.WhenAnyValue(v => v.CurrentPageNumber, p => AreThereAnnotationInTheCurrentPage()))
                             .Switch(); // Only listen to the most recent sequence of property changes (active tool)
            var toolChangedObs = activeToolObs.Select(t => AreThereAnnotationInTheCurrentPage());

            _containsAnnotationsForCurrentPage = toolChangedObs.Merge(pageChangedObs).Merge(annotationsChangedObs)
                .ToProperty(this, p => p.ContainsAnnotationsForCurrentPage); 

            AnnotationTools = new AnnotationToolViewModel(this);

            // when the IsEditing flag changes to false, it means an edit has just completed and we can send
            // the updates annotations to the client
            _isEditingSub = this.WhenAnyValue(p => p.IsEditing)
                .Where(p => !p)
                .Subscribe(_ => UpdateAnnotationModelAnnotations());
        }

        /// <summary>
        /// updates the shared data model, so that the new annotations flow over to the client
        /// </summary>
        private void UpdateAnnotationModelAnnotations()
        {
            AnnotationsModel.Annotations = new ReactiveList<Annotation>(_annotations.Select(p => p.Annotation));
        }

        public void AddInProgressAnnotation(AnnotationViewModel annotation)
        {
            _annotations.Add(annotation);
            if (IsInCurrentPage(annotation))
            {
                this.RaisePropertyChanged("Annotations"); // rebinds to UI
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _isEditingSub.Dispose();
                _containsAnnotationsForCurrentPage.Dispose();
                AnnotationTools.Dispose();

                _isEditingSub = null;
                _containsAnnotationsForCurrentPage = null;
                AnnotationTools = null;
            }
            base.Dispose(disposing);
        }

        public bool ContainsAnnotationsForCurrentPage
        {
            get
            {
                return _containsAnnotationsForCurrentPage.Value;
            }
        }

        /// <summary>
        /// Undoes the last annotation by the Agent
        /// </summary>
        public void UndoAnnotation()
        {
            if (AnnotationsModel.Annotations.Count == 0) return;

            // find the last annotation for this page/tool

            var last = _annotations.LastOrDefault(IsInCurrentPage);
            if (last == null) return;

            _annotations.Remove(last);
            AnnotationsModel.Annotations.Remove(last.Annotation);

            this.RaisePropertyChanged("Annotations"); // rebinds to UI
            AnnotationsModel.RaisePropertyChanged("Annotations"); // do this to ensure annotations are sent
        }
    }
}
