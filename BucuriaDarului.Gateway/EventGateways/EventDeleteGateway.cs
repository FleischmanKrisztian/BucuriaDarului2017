using BucuriaDarului.Core;
using MongoDB.Driver;

namespace BucuriaDarului.Gateway.EventGateways
{
    public class EventDeleteGateway
    {
        public static void DeleteEvent(string id)
        {
            MongoDBGateway dbContext = new MongoDBGateway();
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Event> eventCollection = dbContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("Id", id);
            eventCollection.DeleteOne(filter);
        }
    }
}