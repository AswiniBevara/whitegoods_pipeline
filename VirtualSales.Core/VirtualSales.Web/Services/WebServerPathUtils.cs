using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace VirtualSales.Web.Services
{
    public class WebServerPathUtils
    {
        public static string GetPathTo(string subPath)
        {
            var serverBasePath = (HttpContext.Current == null)
                                     ? HostingEnvironment.MapPath("~/")
                                     : HttpContext.Current.Server.MapPath("~/");

            return Path.Combine(serverBasePath, subPath);
        }
    }
}