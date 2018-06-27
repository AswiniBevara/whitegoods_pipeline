using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualSales.Core.Models.Annotations
{
    public class ClientAnnotationSetupModel : ReactiveObject
    {
        public ClientAnnotationSetupModel()
        {
            _screenSize = new AnnotationSurfaceSize();
        }
        private AnnotationSurfaceSize _screenSize;
        public AnnotationSurfaceSize ScreenSize
        {
            get { return _screenSize; }
            set { this.RaiseAndSetIfChanged(ref _screenSize, value); }
        }
    }
}
