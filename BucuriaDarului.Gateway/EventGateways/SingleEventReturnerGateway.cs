using BucuriaDarului.Core;
using MongoDB.Driver;

namespace BucuriaDarului.Gateway.EventGateways
{
    public class SingleEventReturnerGateway
    {
        public static Event ReturnEvent(string id)
        {
            var dbContext = new MongoDBGateway();

            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var eventCollection = dbContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("Id", id);
            return eventCollection.Find(filter).FirstOrDefault();
        }
    }
}