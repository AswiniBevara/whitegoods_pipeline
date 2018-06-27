using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualSales.Core.AppServices
{
    public interface ISettings
    {
        string GetValue(string key);

        AppMode Mode { get; }
    }
}
