using System;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace BucuriaDarului.Gateway
{
    public class EventsSponsorAllocationDataGateway : IEventSponsorAllocationDisplayGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public List<Event> GetListOfEvents()
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Event> eventCollection = dbContext.Database.GetCollection<Event>("Events");
            List<Event> events = eventCollection.AsQueryable().ToList();
            return events;
        }
        public List<Sponsor> GetListOfSponsors()
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Sponsor> sponsorCollection = dbContext.Database.GetCollection<Sponsor>("Sponsors");
            List<Sponsor> sponsors = sponsorCollection.AsQueryable().ToList();
            return sponsors;
        }

        public void UpdateEvent(string eventId, Event eventToUpdate)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Event> eventCollection = dbContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("_id", eventId);
            eventToUpdate.Id = eventId;
            ModifiedIDGateway modifiedIDGateway = new ModifiedIDGateway();
            modifiedIDGateway.AddIDtoModifications(eventId);
            eventCollection.FindOneAndReplace(filter, eventToUpdate);

        }


        public Event ReturnEvent(string eventId)
        {
            return SingleEventReturnerGateway.ReturnEvent(eventId);
        }
    }
}
