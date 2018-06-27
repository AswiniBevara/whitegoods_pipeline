using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualSales.Models;

namespace VirtualSales.Web.Services
{
    public interface IPdfDocumentTemplateProvider
    {
        Task<Stream> GetTemplate(IllustrationRequest illustrationRequest);
    }
}