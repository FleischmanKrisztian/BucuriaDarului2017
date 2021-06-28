using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway
{
    public class EventVolunteerAllocationUpdateGateway : IEventVolunteerAllocationUpdateGateway
    {
        private MongoDBGateway dBContext = new MongoDBGateway();

        public List<Volunteer> GetListOfVolunteers()
        {
            dBContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Volunteer> volunteerCollection = dBContext.Database.GetCollection<Volunteer>("Volunteers");
            List<Volunteer> volunteers = volunteerCollection.AsQueryable().ToList();
            return volunteers;
        }

        public void UpdateEvent(string eventId, Event eventToUpdate)
        {
            dBContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Event> eventcollection = dBContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("_id", eventId);
            eventToUpdate._id = eventId;
            ModifiedIDGateway modifiedIDGateway = new ModifiedIDGateway();
            modifiedIDGateway.AddIDtoModifications(eventId);
            eventcollection.FindOneAndReplace(filter, eventToUpdate);
        }

        public Event ReturnEvent(string eventId)
        {
            return SingleEventReturnerGateway.ReturnEvent(eventId);
        }
    }
}