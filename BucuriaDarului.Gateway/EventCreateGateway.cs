using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using MongoDB.Driver;

namespace BucuriaDarului.Gateway

{
    public class EventCreateGateway : IEventCreateGateway
    {
        private MongoDBGateway dBContext = new MongoDBGateway();

        public void Insert(Event @event)
        {
            dBContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Event> eventcollection = dBContext.Database.GetCollection<Event>("Events");
            eventcollection.InsertOne(@event);
            ModifiedIDGateway modifiedIDGateway = new ModifiedIDGateway();
            modifiedIDGateway.AddIDtoModifications(@event._id);
        }
    }
}