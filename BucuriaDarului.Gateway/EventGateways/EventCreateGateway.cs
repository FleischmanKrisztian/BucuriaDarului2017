using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using MongoDB.Driver;
using System;

namespace BucuriaDarului.Gateway

{
    public class EventCreateGateway : IEventCreateGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public void Insert(Event @event)
        {
            //@event.Id = Guid.NewGuid().ToString();
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Event> eventCollection = dbContext.Database.GetCollection<Event>("Events");
            eventCollection.InsertOne(@event);
            ModifiedIDGateway modifiedIDGateway = new ModifiedIDGateway();
            modifiedIDGateway.AddIDtoModifications(@event.Id);
        }
    }
}