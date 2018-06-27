using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtualSales.Models
{
    public class MeetingStatus
    {
        public Guid MeetingId { get; set; }
        public bool AgentJoined { get; set; }
        public bool ClientJoined { get; set; }
        public bool MeetingStarted { get; set; }
    }
}