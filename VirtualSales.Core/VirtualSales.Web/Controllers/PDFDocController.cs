using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace VirtualSales.Web.Controllers
{

    public class PDFDocController : ApiController
    {
        private static readonly IDictionary<string, Entry> PdfDocuments = new Dictionary<string, Entry>();

        public static void InsertDocument(string id, byte[] docBytes)
        {
            lock (PdfDocuments)
            {
                PdfDocuments.Values.Where(p => DateTime.Now > p.Expiration).ToList().ForEach(p => PdfDocuments.Remove(p.Id)); // remove any expired items
                PdfDocuments.Add(id, new Entry(id, docBytes));
            }
        }

        public HttpResponseMessage GetDoc(string id)
        {
            if (String.IsNullOrEmpty(id))
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            //new StreamContent(new FileStream(localFilePath, FileMode.Open, FileAccess.Read));

            lock (PdfDocuments)
            {
                Entry entry;
                if (PdfDocuments.TryGetValue(id, out entry))
                {
                    response.Content = new ByteArrayContent(entry.Data);
                    response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = id;
                }
                else
                {
                    response.Content = new StringContent("No document found for id " + id);
                }
            }

            return response;
        }

        public class Entry
        {
            public Entry(string id, byte[] data)
            {
                Id = id;
                Data = data;
                Expiration = DateTime.Now.AddMinutes(30);
            }

            public string Id { get; private set; }
            public byte[] Data { get; private set; }
            public DateTime Expiration { get; private set; }
        }

    }
}
