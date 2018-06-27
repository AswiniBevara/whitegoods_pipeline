using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualSales.Web.Services
{
    public interface IPdfService
    {
        string GeneratedId { get; }
        Task<Stream> CreateForm1();
    }
}