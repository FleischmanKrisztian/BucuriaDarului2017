using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways
{
    public interface IEnventsVolunteerAllocationGateway
    {
        Event GetEvent(string eventId);
        List<Event> GetListOfEvents();
        List<Volunteer> GetListOfVolunteers();
        void UpdateEvent(string eventId,Event eventToUpdate);

    }
}
