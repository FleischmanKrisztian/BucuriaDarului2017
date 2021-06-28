using BucuriaDarului.Core;
using MongoDB.Driver;

namespace BucuriaDarului.Gateway
{
    public class SingleEventReturnerGateway
    {
        public static Event ReturnEvent(string id)
        {
            MongoDBGateway dbContext = new MongoDBGateway();

            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Event> eventCollection = dbContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("_id", id);
            return eventCollection.Find(filter).FirstOrDefault();
        }
    }
}