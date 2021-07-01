using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.EventGateways;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.EventGateways
{
    public class EventsSponsorAllocationDataGateway : IEventSponsorAllocationDisplayGateway
    {
        private readonly MongoDBGateway dbContext = new MongoDBGateway();

        public List<Event> GetListOfEvents()
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var eventCollection = dbContext.Database.GetCollection<Event>("Events");
            var events = eventCollection.AsQueryable().ToList();
            return events;
        }

        public List<Sponsor> GetListOfSponsors()
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var sponsorCollection = dbContext.Database.GetCollection<Sponsor>("Sponsors");
            var sponsors = sponsorCollection.AsQueryable().ToList();
            return sponsors;
        }

        public Event ReturnEvent(string eventId)
        {
            return SingleEventReturnerGateway.ReturnEvent(eventId);
        }
    }
}