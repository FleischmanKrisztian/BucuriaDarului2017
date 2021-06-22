using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways
{
    public interface IEnventsMainDataVolunteerAllocationGateways
    {
        List<Event> GetListOfEvents();
        List<Volunteer> GetListOfVolunteers();
        void UpDateEvent(string eventId,Event eventToUpdate);

    }
}
