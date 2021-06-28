using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways
{
    public interface IEventVolunteerAllocationUpdateGateway
    {
        List<Volunteer> GetListOfVolunteers();
        void UpdateEvent(string EventId, Event event_);
    }
}
