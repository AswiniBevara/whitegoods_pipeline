using System.Diagnostics;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VirtualSales.Core.AppServices;
using VirtualSales.Core.Models.Annotations;
using VirtualSales.Models.Annotations;

namespace VirtualSales.Core.ViewModels.Annotations
{
    public class ClientAnnotationViewModel : UserAnnotationViewModel
    {
        private readonly ObservableAsPropertyHelper<IEnumerable<AnnotationViewModel>> _clientAnnotations;
        private IDisposable _annotationsChangedSub;

        public ClientAnnotationViewModel(ISharedDataService sharedDataService, MeetingViewModel meeting) : base(sharedDataService, meeting)
        {
            // create the client annotations property, a mapped version of the agent's properties based on the screen sizes.
            _annotationsChangedSub = _clientAnnotations = AnnotationsModel
                                                              .WhenAnyValue(v => v.Annotations, v => v.Select(p => CreateAnnotationViewModel(p)))
                                                              .ToProperty(this, v => v.Annotations);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _annotationsChangedSub.Dispose();

                _annotationsChangedSub = null;
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// readonly property based on the contents of the shared AnnotationsModel, which will be updated
        /// as messages from the agent come in
        /// </summary>
        public override IEnumerable<AnnotationViewModel> Annotations
        {
            get
            {
                if (_clientAnnotations == null || _clientAnnotations.Value == null) return null;                
                return _clientAnnotations.Value.Where(IsInCurrentPage);
            }
        }
    }
}
