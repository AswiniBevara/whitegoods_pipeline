using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualSales.Models.Annotations
{
    /// <summary>
    /// Values are always between 0.0 and 1.0 to ensure proper dimensions on either client or agent side,
    /// without need of sharing screen size.
    /// </summary>
    public class AnnotationPoint
    {
        public float X { get; set; }
        public float Y { get; set; }
    }
}
