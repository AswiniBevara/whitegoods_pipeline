using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualSales.Models.Annotations;

namespace VirtualSales.Core.Models.Annotations
{
    /// <summary>
    /// Given a point and a surface size knows how to convert from the standard (0,1) range to surface's range and 
    /// the other way around.
    /// </summary>
    public class AnnotationPointTransformer
    {
        public static AnnotationPoint ConvertFromSurfaceToStandardRange(AnnotationPoint point, AnnotationSurfaceSize surface)
        {
            return new AnnotationPoint { X = point.X / surface.Width, Y = point.Y / surface.Height };
        }
        public static AnnotationPoint ConvertFromStandardRangeToSurface(AnnotationPoint point, AnnotationSurfaceSize surface)
        {
            return new AnnotationPoint { X = point.X * surface.Width, Y = point.Y * surface.Height };
        }
    }
}