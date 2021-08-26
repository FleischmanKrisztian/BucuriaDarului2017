using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.EventGateways
{
    public interface IEventSponsorAllocationUpdateGateway
    {
        List<Sponsor> GetListOfSponsors();

        void UpdateEvent(string eventId, Event @event);

        Event ReturnEvent(string eventId);
    }
}