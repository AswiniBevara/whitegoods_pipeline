using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using VirtualSales.Web.Hubs;

namespace VirtualSales.Web.Services
{
    public class HeartbeatService
    {
        public HeartbeatService()
        {
            Start();
        }

        public async void Start()
        {
            while (true)
            {
                var hub = GlobalHost.ConnectionManager.GetHubContext<MeetingHub>();
                try
                {
                    hub.Clients.All.onHeartbeat(DateTimeOffset.UtcNow);
                }
                catch (Exception)
                {
                    Debug.WriteLine("Error sending heartbeat");
                }

                await Task.Delay(10000);
            }
        }
    }
}