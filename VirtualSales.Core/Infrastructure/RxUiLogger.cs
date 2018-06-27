#define DEBUG

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Splat;


namespace VirtualSales.Core.Infrastructure
{
    public class RxUiLogger : ILogger
    {
        public void Write(string message, LogLevel logLevel)
        {
            Debug.WriteLine("{0}: {1}", logLevel, message);
        }

        public LogLevel Level { get; set; }
    }
}
