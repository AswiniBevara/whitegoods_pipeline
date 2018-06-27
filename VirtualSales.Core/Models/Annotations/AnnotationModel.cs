using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualSales.Models.Annotations;

namespace VirtualSales.Core.Models.Annotations
{
    /// <summary>
    /// The shared data model of annotations that is created and modified on the agent's side, while being
    /// listened to on the client side.
    /// </summary>
    public class AnnotationsModel : ReactiveObject
    {
        public AnnotationsModel()
        {
            Annotations = new ReactiveList<Annotation>();
        }

        private ReactiveList<Annotation> _annotations;
        public ReactiveList<Annotation> Annotations
        {
            get { return _annotations; }
            set { this.RaiseAndSetIfChanged(ref _annotations, value); }
        }
    }


}
