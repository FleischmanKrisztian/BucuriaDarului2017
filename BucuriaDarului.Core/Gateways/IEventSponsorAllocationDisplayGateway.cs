using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways
{
    public interface IEventSponsorAllocationDisplayGateway
    {
        List<Event> GetListOfEvents();
        List<Sponsor> GetListOfSponsors();
        void UpdateEvent(string eventId, Event eventToUpdate);
        Event ReturnEvent(string eventId);
    }
}
