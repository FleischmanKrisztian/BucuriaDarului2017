using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using BucuriaDarului.Core.Gateways.EventGateways;
using MongoDB.Driver;

namespace BucuriaDarului.Gateway.EventGateways

{
    public class EventCreateGateway : IEventCreateGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public void Insert(Event @event)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var eventCollection = dbContext.Database.GetCollection<Event>("Events");
            eventCollection.InsertOne(@event);
            var modifiedIDGateway = new ModifiedIDGateway();
            modifiedIDGateway.AddIDtoModifications(@event.Id);
        }
    }
}