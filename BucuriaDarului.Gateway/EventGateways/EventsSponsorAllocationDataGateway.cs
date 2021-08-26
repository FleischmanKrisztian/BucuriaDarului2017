using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.EventGateways;
using BucuriaDarului.Gateway.SponsorGateways;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.EventGateways
{
    public class EventsSponsorAllocationDataGateway : IEventSponsorAllocationDisplayGateway
    {
        private readonly MongoDBGateway dbContext = new MongoDBGateway();

        public List<Event> GetListOfEvents()
        {
            return ListEventsGateway.GetListOfEvents();
        }

        public List<Sponsor> GetListOfSponsors()
        {
            return ListSponsorsGateway.GetListOfSponsors();
        }

        public Event ReturnEvent(string eventId)
        {
            return SingleEventReturnerGateway.ReturnEvent(eventId);
        }
    }
}