using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualSales.Models;

namespace VirtualSales.Web.Services
{
    public class PdfService : IPdfService
    {
        private readonly IllustrationRequest _data;
        private readonly IPdfDocumentTemplateProvider _templateProvider;

        public PdfService(IPdfDocumentTemplateProvider templateProvider, IllustrationRequest data)
        {
            _data = data;
            _templateProvider = templateProvider;

            if (String.IsNullOrEmpty(_data.FirstName) || String.IsNullOrEmpty(_data.LastName))
            {
                GeneratedId = Guid.NewGuid().ToString();
            }
            else
            {
                GeneratedId = String.Format("{0}-{1}", _data.FirstName, _data.LastName).Trim();
            }
        }

        public string GeneratedId { get; private set; }

        public async Task<Stream> CreateForm1()
        {
            var docStream = await _templateProvider.GetTemplate(_data);

            // xfinium does not like the PDFs provided by White Label
            //var doc = new PdfFixedDocument(docStream);

            //(doc.Form.Fields["firstname"] as PdfTextBoxField).Text = _data.FirstName;
            //(doc.Form.Fields["lastname"] as PdfTextBoxField).Text = _data.LastName;

            var ms = new MemoryStream();
            docStream.CopyTo(ms);
            ms.Position = 0;

            return ms;
        }
    }

    public static class StreamExtensions
    {
        public static byte[] ReadToEnd(this Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                var readBuffer = new byte[4096];

                var totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        var nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            var temp = new byte[readBuffer.Length*2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                var buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }
}