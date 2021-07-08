using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.EventGateways;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.EventGateways
{
    public class EventsImportDataGateway : IEventsImportDataGateway
    {
        private readonly MongoDBGateway dbContext = new MongoDBGateway();

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
    }
}