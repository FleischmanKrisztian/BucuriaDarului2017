using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.EventGateways;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.EventGateways
{
    public class EventsImportDataGateway : IEventsImportDataGateway
    {
        private readonly MongoDBGateway dbContext = new MongoDBGateway();

        public  Event GetEvent(string eventId)
        {
            return SingleEventReturnerGateway.ReturnEvent(eventId);
        }

        public List<Event> GetEvents()
        {
            return ListEventsGateway.GetListOfEvents();
        }

        public void Insert(List<Event> events)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var eventCollection = dbContext.Database.GetCollection<Event>("Events");
            var modifiedIdGateway = new ModifiedIDGateway();
            foreach (var eve in events)
            {
                var filter = Builders<Event>.Filter.Eq("Id", eve.Id);
                if (eventCollection.Find(filter).FirstOrDefault() == null)
                {
                    eventCollection.InsertOne(eve);
                    modifiedIdGateway.AddIDtoModifications(eve.Id);
                }
            }
        }

        public void Update(List<Event> events)
        {
            foreach (var @event in events)
            {
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
                var eventCollection = dbContext.Database.GetCollection<Event>("Events");
                var filter = Builders<Event>.Filter.Eq("Id", @event.Id);
                var modifiedIdGateway = new ModifiedIDGateway();
                modifiedIdGateway.AddIDtoModifications(@event.Id);
                eventCollection.FindOneAndReplace(filter, @event);
            }
        }
    }
}