using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.EventGateways;
using BucuriaDarului.Gateway.SponsorGateways;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.EventGateways
{
    public class EventSponsorAllocationUpdateGateway : IEventSponsorAllocationUpdateGateway
    {
        private readonly MongoDBGateway dbContext = new MongoDBGateway();

        public List<Sponsor> GetListOfSponsors()
        {
            return ListSponsorsGateway.GetListOfSponsors();
        }

        public void UpdateEvent(string eventId, Event eventToUpdate)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var eventCollection = dbContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("Id", eventId);
            eventToUpdate.Id = eventId;
            var modifiedIDGateway = new ModifiedIDGateway();
            modifiedIDGateway.AddIDtoModifications(eventId);
            eventCollection.FindOneAndReplace(filter, eventToUpdate);
        }

        public Event ReturnEvent(string eventId)
        {
            return SingleEventReturnerGateway.ReturnEvent(eventId);
        }
    }
}