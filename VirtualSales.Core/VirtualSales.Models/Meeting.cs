using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualSales.Models
{
    public class Meeting
    {
        public Guid Id { get; set; }
        public ClientInfo Client { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public MeetingStatus Status { get; set; }
    }
}
