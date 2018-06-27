using System;
using System.Collections.Generic;
using System.Linq;
using VirtualSales.Models;

namespace VirtualSales.Web.Services
{
    public class MeetingInfo
    {
        public MeetingInfo(Guid id)
        {
            Id = id;
        }

        public string AgentConnectionId { get; private set; }
        public string ClientConnectionId { get; private set; }

        public bool HasClient
        {
            get { return !string.IsNullOrEmpty(ClientConnectionId); }
        }

        public bool HasAgent
        {
            get { return !string.IsNullOrEmpty(AgentConnectionId); }
        }

        public bool HasMeetingStarted
        {
            get { return ChatConfig != null; }
        }

        public VideoChatConfiguration ChatConfig { get; private set; }
        public Guid Id { get; private set; }

        public bool IsClient(string connectionId)
        {
            return ClientConnectionId == connectionId;
        }

        public bool IsAgent(string connectionId)
        {
            return AgentConnectionId == connectionId;
        }

        public void ConnectAgent(string agentConnectionId)
        {
            AgentConnectionId = agentConnectionId;
        }

        public void ConnectClient(string clientConnectionId)
        {
            ClientConnectionId = clientConnectionId;
        }

        public void DisconnectClient()
        {
            ClientConnectionId = null;
        }

        public void DisconnectAgent()
        {
            AgentConnectionId = null;
        }

        public void StartMeeting(VideoChatConfiguration chatConfiguration)
        {
            ChatConfig = chatConfiguration;
        }
    }
}