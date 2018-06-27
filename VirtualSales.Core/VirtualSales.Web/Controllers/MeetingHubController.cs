using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtualSales.Web.Hubs;

namespace VirtualSales.Web.Controllers
{
    public class MeetingHubController : AsyncController
    {

        public void ClearMeetingsAsync()
        {
            MeetingHub.ClearAsync();
        }

        public ActionResult ClearMeetingsCompleted(string status)
        {
            return Content("Clear Completed " + DateTime.Now, "text/plain");
        }
    }
}