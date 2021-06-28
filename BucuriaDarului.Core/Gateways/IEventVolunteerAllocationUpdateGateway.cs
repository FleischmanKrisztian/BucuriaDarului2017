using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways
{
    public interface IEventVolunteerAllocationUpdateGateway
    {
        List<Volunteer> GetListOfVolunteers();

        void UpdateEvent(string eventId, Event @event);

        Event ReturnEvent(string eventId);
    }
}
