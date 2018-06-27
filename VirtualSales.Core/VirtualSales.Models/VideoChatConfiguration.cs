using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualSales.Models
{
    public class VideoChatConfiguration
    {

        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        public string ApiKey { get; set; }
        public int AppId { get; set; }
        public long Expires { get; set; }
        public string ScopeId { get; set; }
        public uint MaxWidth { get; set; }
        public uint MaxHeight { get; set; }
        public uint MaxFramesPerSecond { get; set; }
        public uint MaxBitRate { get; set; }
        public string Salt { get; set; }
        public Guid MeetingId { get; set; }
    }
}
