using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.EventGateways
{
    public interface IEventVolunteerAllocationDisplayGateway
    {
        List<Volunteer> GetListOfVolunteers();

        Event ReturnEvent(string eventId);
    }
}
