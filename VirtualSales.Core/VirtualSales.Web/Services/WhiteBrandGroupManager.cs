using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace VirtualSales.Web.Services
{
    public class WhiteBrandGroupManager
    {
        private readonly Dictionary<string, HashSet<string>> _groupMembershipTracker = new Dictionary<string, HashSet<string>>();

        public async Task AddToGroup(IGroupManager groupManager, string group, string connection)
        {
            await groupManager.Add(connection, group);
            if (!_groupMembershipTracker.ContainsKey(group))
            {
                _groupMembershipTracker[group] = new HashSet<string>();
            }
            _groupMembershipTracker[group].Add(connection);
        }

        public async Task RemoveFromGroup(IGroupManager groupManager, string group, string connection)
        {
            await groupManager.Remove(connection, group);
            if (!_groupMembershipTracker.ContainsKey(group)) return;
            var set = _groupMembershipTracker[group];

            set.Remove(connection);

            if (set.Count == 0)
            {
                _groupMembershipTracker.Remove(group);
            }
        }

        public void ClearGroup(IGroupManager groupManager, string group)
        {
            if (!_groupMembershipTracker.ContainsKey(group)) return;
            var set = _groupMembershipTracker[group];
            _groupMembershipTracker.Remove(group);

            foreach (var connectionId in set)
            {
                groupManager.Remove(connectionId, group);
            }
        }

        public void Clear()
        {
            _groupMembershipTracker.Clear();
        }

        public void ClearGroups(IGroupManager groups)
        {
            foreach (var groupName in _groupMembershipTracker.Keys.ToList())
            {
                ClearGroup(groups, groupName);
            }
        }
    }
}