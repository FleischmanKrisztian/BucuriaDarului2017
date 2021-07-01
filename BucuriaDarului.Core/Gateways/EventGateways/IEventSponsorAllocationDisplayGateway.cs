using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.EventGateways
{
    public interface IEventSponsorAllocationDisplayGateway
    {
        List<Event> GetListOfEvents();
        List<Sponsor> GetListOfSponsors();
        Event ReturnEvent(string eventId);
    }
}
