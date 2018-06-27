using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualSales.Models
{
    public class ChangedMessage
    {
        /// <summary>
        /// Type that's being changed
        /// </summary>
        public string SourceObjectType { get; set; }

        /// <summary>
        /// JSON-encoded value of the property
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Name of the property
        /// </summary>
        public string PropertyName { get; set; }
    }
}
