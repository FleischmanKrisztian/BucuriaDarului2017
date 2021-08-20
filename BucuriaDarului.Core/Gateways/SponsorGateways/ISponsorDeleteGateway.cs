using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.SponsorGateways
{
    public interface ISponsorDeleteGateway
    {
        void DeleteSponsor(string id);

        Sponsor GetSponsor(string sponsorId);

        List<Event> GetEvents();

        Event GetEvent(string idEvent);

        void UpdateEvent(string eventId, Event eventToUpdate);
    }
}