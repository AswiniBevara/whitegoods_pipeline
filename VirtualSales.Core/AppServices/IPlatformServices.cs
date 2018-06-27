using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualSales.Core.AppServices
{
    public interface IPlatformServices
    {
        Task SaveAndLaunchFile(Stream stream, string fileType);
    }
}
