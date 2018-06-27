using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualSales.Core.Models.Annotations;
using VirtualSales.Models.Annotations;

namespace VirtualSales.Core.ViewModels.Annotations
{
    /// <summary>
    /// Helps UIs figure out how to draw an annotation. Does all the necesary point conversion from the standard (0,1)
    /// range into the proper surface size passed in the consturctor. 
    /// 
    /// This class makes NO modifications to the Annotation objects themselves.
    /// </summary>
    public class AnnotationViewModel : ReactiveObject
    {
        public Annotation Annotation { get; private set; }
        public AnnotationType Type { get { return Annotation.Type; } }

        private AnnotationSurfaceSize _surfaceSize;

        public static AnnotationViewModel Create(Annotation annotation, AnnotationSurfaceSize surfaceSize)
        {
            return new AnnotationViewModel(annotation, surfaceSize);
        }

        private AnnotationViewModel(Annotation annotation, AnnotationSurfaceSize surfaceSize)
        {
            Annotation = annotation;
            _surfaceSize = surfaceSize;
        }

        /// <summary>
        /// Gets the point at an index and converts it to a float value
        /// </summary>
        private float ValAtIndex(int index, Func<AnnotationPoint, float> get)
        {
            return get(AnnotationPointTransformer.ConvertFromStandardRangeToSurface(Annotation.Points[index], _surfaceSize));
        }

        /// <summary>
        /// Gets the first and second value of a point (should only be used for Line, Ellipsis, Rectangle annotations)
        /// and uses a selector function to select one of the values.
        /// </summary>
        private float ValComp(Func<float, float, float> selector, Func<AnnotationPoint, float> get)
        {
            if (Annotation.Points.Count < 2) return 0;
            var pt1 = AnnotationPointTransformer.ConvertFromStandardRangeToSurface(Annotation.Points[0], _surfaceSize);
            var pt2 = AnnotationPointTransformer.ConvertFromStandardRangeToSurface(Annotation.Points[1], _surfaceSize);
            return selector(get(pt1), get(pt2));
        }

        public float MinX { get { return ValComp(Math.Min, p => p.X); } }
        public float MinY { get { return ValComp(Math.Min, p => p.Y); } }
        public float MaxX { get { return ValComp(Math.Max, p => p.X); } }
        public float MaxY { get { return ValComp(Math.Max, p => p.Y); } }

        public float Top { get { return Type == AnnotationType.Line || Type == AnnotationType.AdHoc ? 0 : MinY; } }
        public float Left { get { return Type == AnnotationType.Line || Type == AnnotationType.AdHoc ? 0 : MinX; } }

        public float X1 { get { return ValAtIndex(0, p => p.X); } }
        public float X2 { get { return ValAtIndex(Annotation.Points.Count == 1 ? 0 : 1, p => p.X); } }
        public float Y1 { get { return ValAtIndex(0, p => p.Y); } }
        public float Y2 { get { return ValAtIndex(Annotation.Points.Count == 1 ? 0 : 1, p => p.Y); } }
        public float Width { get { return MaxX - MinX; } }
        public float Height { get { return MaxY - MinY; } }

        public List<AnnotationPoint> SurfacePoints
        {
            get
            {
                return Annotation.Points.Select(p => AnnotationPointTransformer.ConvertFromStandardRangeToSurface(p, _surfaceSize)).ToList();
            }
        }

        public string PathMarkup
        {
            get
            {
                if (Annotation.Points.Count == 0) return "M 0,0";
                if (Annotation.Points.Count == 1)
                {
                    var onept = string.Format("M {0},{1} L {0},{1}", X1, Y1);
                    return onept;
                }

                var convertedPointsNotIncludingFirst = SurfacePoints.Skip(1)
                    .Select(p => string.Format("{0},{1}", p.X, p.Y));
                
                var linepathafterfirstpoint = string.Join(" ", convertedPointsNotIncludingFirst.ToArray());
                
                var result = string.Format("M {0},{1} L {2}", X1, Y1, linepathafterfirstpoint);
                return result;
            }
        }

        public void OnChanged()
        {
            // srozga: using the classic way of doing this, not a huge deal. if there's a better way to 
            // do this, great, but these properties will change depending on what has changed in the 
            // underlying Annotation object and also depending on the type of annotation. for the
            // purposes of simplicity I just raise all the changes.
            this.RaisePropertyChanged("MinX");
            this.RaisePropertyChanged("MinY");
            this.RaisePropertyChanged("MaxX");
            this.RaisePropertyChanged("MaxY");
            this.RaisePropertyChanged("X1");
            this.RaisePropertyChanged("X2");
            this.RaisePropertyChanged("Y2");
            this.RaisePropertyChanged("Y2");

            this.RaisePropertyChanged("Top");
            this.RaisePropertyChanged("Left");
            this.RaisePropertyChanged("Width");
            this.RaisePropertyChanged("Height");

            this.RaisePropertyChanged("PathMarkup");
            this.RaisePropertyChanged("SurfacePoints");
        }
    }

}
