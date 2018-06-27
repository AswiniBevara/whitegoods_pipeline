using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualSales.Models.Annotations;

namespace VirtualSales.Core.Models.Annotations
{
    /// <summary>
    /// The AnnotationBuilder is specific to a surface size and points generated from a surface with said size.
    /// When a point comes in, it is converted into a point in the range (0,1) and added to the annotation
    /// based on its type. The primitive UI surface that is used to generate new annotations would use this.
    /// </summary>
    public class AnnotationBuilder
    {
        private AnnotationType type;
        private Annotation annotation;
        private AnnotationSurfaceSize surfaceSize;

        public static AnnotationBuilder Create(AnnotationType type, AnnotationSurfaceSize size, int currentPage, Guid toolId)
        {
            return new AnnotationBuilder(type, size, toolId, currentPage);
        }

        private AnnotationBuilder(AnnotationType type, AnnotationSurfaceSize size, Guid toolId, int currentPage)
        {
            this.surfaceSize = size;
            this.type = type;

            this.annotation = new Annotation()
            {
                Type = type,
                Points = new List<AnnotationPoint>(),
                PageNumber = currentPage,
                ToolId = toolId
            };
        }

        public void OnNextPoint(AnnotationPoint point)
        {
            // translate points from surface size to 0-1 range size
            var pt = new AnnotationPoint { X = point.X / surfaceSize.Width, Y = point.Y / surfaceSize.Height };

            if (pt.X < 0 || pt.Y < 0 || pt.X > 1 || pt.Y > 1) return;  // reject this point if outside bounds

            if (type == AnnotationType.AdHoc)
                this.annotation.Points.Add(pt); // ad hoc always just adds more points
            else
            {
                // the rest will only have at most two points, the first point and the last point touched
                if (this.annotation.Points.Count <= 1)
                    this.annotation.Points.Add(pt);
                else
                    this.annotation.Points[1] = pt;
            }
        }

        // the annotation object being built by this instance of the builder
        public Annotation Annotation
        {
            get { return this.annotation; }
        }
    }
}
