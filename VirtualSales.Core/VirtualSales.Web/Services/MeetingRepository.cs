using System;
using System.Collections.Generic;
using System.Linq;
using VirtualSales.Models;

namespace VirtualSales.Web.Services
{
    public class MeetingRepository
    {
        private readonly IEnumerable<Meeting> _meetings;
        private readonly MeetingInfoRepository _repository;

        public MeetingRepository(MeetingInfoRepository repository)
        {
            _repository = repository;
            _meetings = new[]
            {
                new Meeting
                {
                    Client = new ClientInfo
                    {
                        FirstName = "Hugo",
                        LastName = "Boss",
                        PhoneNumber = "555-555-1212"
                    },
                    StartTime = DateTimeOffset.Now.Date.AddHours(12),
                    Duration = TimeSpan.FromMinutes(30),
                    Id = new Guid("64BDF0C5-DE47-4101-A650-CA11F5E58830")
                },
                new Meeting
                {
                    Client = new ClientInfo
                    {
                        FirstName = "Donna",
                        LastName = "Karan",
                        PhoneNumber = "555-555-1212"
                    },
                    StartTime = DateTimeOffset.Now.Date.AddHours(13),
                    Duration = TimeSpan.FromMinutes(30),
                    Id = new Guid("2E2AB0CE-132E-4A48-B246-E0AB270A784C")
                },
                new Meeting
                {
                    Client = new ClientInfo
                    {
                        FirstName = "Calvin",
                        LastName = "Klein",
                        PhoneNumber = "555-555-1212"
                    },
                    StartTime = DateTimeOffset.Now.Date.AddHours(14),
                    Duration = TimeSpan.FromMinutes(30),
                    Id = new Guid("FA787157-F7F8-49AB-8FD4-50801D0F3B8B")
                },
                new Meeting
                {
                    Client = new ClientInfo
                    {
                        FirstName = "Ralph",
                        LastName = "Lauren",
                        PhoneNumber = "555-555-1212"
                    },
                    StartTime = DateTimeOffset.Now.Date.AddHours(15),
                    Duration = TimeSpan.FromMinutes(30),
                    Id = new Guid("56477A05-C0B0-4173-96EC-8D1841C90243")
                },
            };
        }

        public static MeetingStatus BuildMeetingStatus(Guid id, MeetingInfo meetingInfo)
        {
            return new MeetingStatus()
            {
                MeetingId = id,
                AgentJoined = meetingInfo != null && meetingInfo.HasAgent,
                ClientJoined = meetingInfo != null && meetingInfo.HasClient,
                MeetingStarted = meetingInfo != null && meetingInfo.HasMeetingStarted
            };
        }

        private MeetingStatus GetMeetingStatus(Guid meetingId)
        {
            var meetingInfo = _repository.GetMeetingInfo(meetingId);
            return BuildMeetingStatus(meetingId, meetingInfo);
        }

        public IEnumerable<Meeting> GetMeetings()
        {
            // update the meetings with the latest status
            foreach (var meeting in _meetings)
            {
                meeting.Status = GetMeetingStatus(meeting.Id);
            }
            return _meetings;
        }

        public Meeting GetMeeting(Guid id)
        {
            return _meetings.FirstOrDefault(p => p.Id == id);
        }
    }
}