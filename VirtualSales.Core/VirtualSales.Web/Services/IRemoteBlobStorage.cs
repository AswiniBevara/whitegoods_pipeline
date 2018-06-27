using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualSales.Web.Services
{
    public interface IRemoteBlobStorage : IDisposable
    {
        Stream RetrieveValue(string key);
        void StoreValue(string key, Stream dataInputStream);
        void RemoveValue(string key);
    }
}