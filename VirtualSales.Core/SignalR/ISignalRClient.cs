using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualSales.Core.SignalR
{
    public interface ISignalRClient
    {
        Task Connect();
        Task<byte[]> GetIllustrationPdfAsync(string id);
        IObservable<string> PdfAvailable { get; } 
    }
}
