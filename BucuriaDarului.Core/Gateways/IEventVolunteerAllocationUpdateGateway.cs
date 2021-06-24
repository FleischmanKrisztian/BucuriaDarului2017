using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways
{
    public interface IEventVolunteerAllocationUpdateGateway
    {
        Event GetEvent(string eventId);
        List<Volunteer> GetListOfVolunteers();
        void UpdateEvent(string EventId, Event event_);
    }
}
