using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.SponsorGateways;
using BucuriaDarului.Gateway.EventGateways;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.SponsorGateways
{
    public class SponsorDeleteGateway: ISponsorDeleteGateway
    {
        public void DeleteSponsor(string id)
        {
            
                MongoDBGateway dbContext = new MongoDBGateway();
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
                IMongoCollection<Sponsor> sponsorsCollection = dbContext.Database.GetCollection<Sponsor>("Sponsors");
                var filter = Builders<Sponsor>.Filter.Eq("Id", id);
                var deletedIdGateway = new DeletedIDGateway();
                deletedIdGateway.AddIDtoDeletions(id);
                sponsorsCollection.DeleteOne(filter);
            
        }

        public Event GetEvent(string idEvent)
        {
            return SingleEventReturnerGateway.ReturnEvent(idEvent);
        }

        public List<Event> GetEvents()
        {
            return ListEventsGateway.GetListOfEvents();
        }

        public Sponsor GetSponsor(string sponsorId)
        {
            return SingleSponsorReturnerGateway.ReturnSponsor(sponsorId);
        }

        public void UpdateEvent(string eventId, Event eventToUpdate)
        {
            MongoDBGateway dbContext = new MongoDBGateway();
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var eventCollection = dbContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("Id", eventId);
            eventToUpdate.Id = eventId;
            var modifiedIDGateway = new ModifiedIDGateway();
            modifiedIDGateway.AddIDtoModifications(eventId);
            eventCollection.FindOneAndReplace(filter, eventToUpdate);
        }
    }
}
