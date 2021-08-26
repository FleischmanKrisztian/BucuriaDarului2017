using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.EventGateways;
using BucuriaDarului.Gateway.VolunteerGateways;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.EventGateways
{
    public class EventVolunteerAllocationUpdateGateway : IEventVolunteerAllocationUpdateGateway
    {
        private readonly MongoDBGateway dbContext = new MongoDBGateway();

        public List<Volunteer> GetListOfVolunteers()
        {
            return ListVolunteersGateway.GetListOfVolunteers();
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