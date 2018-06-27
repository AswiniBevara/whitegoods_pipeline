using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualSales.Core.Models.Annotations
{
    public class AnnotationSurfaceSize : ReactiveObject
    {
        private float _height;
        private float _width;

        public float Width
        {
            get { return _width; }
            set
            {
                this.RaiseAndSetIfChanged(ref _width, value);
            }
        }

        public float Height
        {
            get { return _height; }
            set
            {
                this.RaiseAndSetIfChanged(ref _height, value);
            }
        }
    }
}