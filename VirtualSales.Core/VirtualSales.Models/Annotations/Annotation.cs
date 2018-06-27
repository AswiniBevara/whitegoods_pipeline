using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualSales.Models.Annotations
{
    /// <summary>
    /// This class represents different types of annotations. Dimensions for points are always stored in values
    /// from 0.0 - 1.0, this makes it so that the values can be easily sized on either client or agent side.
    /// </summary>
    public class Annotation
    {
        public AnnotationType Type { get; set; }
        public List<AnnotationPoint> Points { get; set; }
        public Guid ToolId { get; set; }
        public int PageNumber { get; set; }

        public override string ToString()
        {
            var pts = string.Join(",", Points.Select(p => String.Format("({0},{1})", p.X, p.Y)).ToArray());
            return String.Format("{0} - {1}", Type.ToString(), pts);
        }
    }
}
