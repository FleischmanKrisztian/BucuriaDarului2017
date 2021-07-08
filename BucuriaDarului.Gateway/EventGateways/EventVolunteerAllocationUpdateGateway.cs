using System.Collections.Generic;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using BucuriaDarului.Core.Gateways.EventGateways;
using MongoDB.Driver;

namespace BucuriaDarului.Gateway.EventGateways
{
    public class EventVolunteerAllocationUpdateGateway : IEventVolunteerAllocationUpdateGateway
    {
        private readonly MongoDBGateway dbContext = new MongoDBGateway();

        public List<Volunteer> GetListOfVolunteers()
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Volunteer> volunteerCollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            List<Volunteer> volunteers = volunteerCollection.AsQueryable().ToList();
            return volunteers;
        }

        public void UpdateEvent(string eventId, Event eventToUpdate)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Event> eventCollection = dbContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("Id", eventId);
            eventToUpdate.Id = eventId;
            ModifiedIDGateway modifiedIdGateway = new ModifiedIDGateway();
            modifiedIdGateway.AddIDtoModifications(eventId);
            eventCollection.FindOneAndReplace(filter, eventToUpdate);
        }

        public Event ReturnEvent(string eventId)
        {
            return SingleEventReturnerGateway.ReturnEvent(eventId);
        }
    }
}