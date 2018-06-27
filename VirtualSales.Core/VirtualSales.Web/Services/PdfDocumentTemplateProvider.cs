using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualSales.Models;

namespace VirtualSales.Web.Services
{
    public class PdfDocumentTemplateProvider : IPdfDocumentTemplateProvider
    {
        public Task<Stream> GetTemplate(IllustrationRequest illustrationRequest)
        {
            var tcs = new TaskCompletionSource<Stream>();

            try
            {
                //var g = illustrationRequest.Gender.HasValue ? illustrationRequest.Gender.Value : Gender.Male;
                //var smoker = illustrationRequest.IsSmoker.HasValue ? illustrationRequest.IsSmoker.Value : false;
                //var age = illustrationRequest.Age.HasValue ? AgeFromBuckets(illustrationRequest.Age.Value) : 18;
                //var requested = illustrationRequest.RequestedCoverage.HasValue ? illustrationRequest.RequestedCoverage.Value : 1000000;

                //var genderString = g == Gender.Male ? "M" : "F";
                //var smokerString = smoker ? "SM" : "NS";
                //var reqString = string.Empty;
                //if (requested <= 250000) reqString = "250K";
                //else if (requested <= 500000) reqString = "500K";
                //else reqString = "1M";

                //var filename = String.Format("{0}-{1}-{2}-{3}.pdf", genderString, smokerString, age, reqString);

                var path = WebServerPathUtils.GetPathTo(Path.Combine("bin", "Documents", "formfill.pdf"));
                var f = new FileInfo(path);
                var fs = f.OpenRead();

                
                tcs.SetResult(fs);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
            return tcs.Task;
        }

        private int AgeFromBuckets(int age)
        {
            var result = 18;

            if (age < 20) result = 18;
            else if (age <= 30) result = 25;
            else if (age <= 40) result = 35;
            else if (age <= 50) result = 45;
            else if (age <= 60) result = 55;
            else result = 65;
            return result;
        }
    }
}