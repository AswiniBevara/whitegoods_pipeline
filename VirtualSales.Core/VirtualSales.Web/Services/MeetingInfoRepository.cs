using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtualSales.Web.Services
{
    public class MeetingInfoRepository
    {
        private readonly Dictionary<Guid, MeetingInfo> _collection = new Dictionary<Guid, MeetingInfo>();

        public IEnumerable<Guid> AllMeetings
        {
            get { return _collection.Keys; }
        }

        public MeetingInfo GetMeetingInfo(Guid id)
        {
            if (!_collection.ContainsKey(id))
            {
                _collection[id] = new MeetingInfo(id);
            }
            return _collection[id];
        }

        public void RemoveMeeting(Guid id)
        {
            _collection.Remove(id);
        }

        public MeetingInfo FindMeetingForAgent(string agentConnectionId)
        {
            var result = _collection.Values.FirstOrDefault(p => p.AgentConnectionId == agentConnectionId);
            return result;
        }

        public MeetingInfo FindMeetingForClient(string clientConnectionId)
        {
            var result = _collection.Values.FirstOrDefault(p => p.ClientConnectionId == clientConnectionId);
            return result;
        }

        public void Clear()
        {
            _collection.Clear();
        }
    }
}