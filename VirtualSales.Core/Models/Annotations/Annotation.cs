using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualSales.Core.Models.Annotations
{
    public class Annotation
    {
        public AnnotationType Type { get; set; }
        public List<AnnotationPoint> Points { get; set; }
    }
}
