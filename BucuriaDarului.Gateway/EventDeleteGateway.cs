using BucuriaDarului.Core;
using MongoDB.Driver;
using System;

namespace BucuriaDarului.Gateway
{
    public class EventDeleteGateway
    {
        private MongoDBGateway dBContext = new MongoDBGateway();

        public Event ReturnEvent(string id)
        {
            dBContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Event> eventcollection = dBContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("_id", id);
            return eventcollection.Find(filter).FirstOrDefault();
        }

        public object DeleteEvent(string id)
        {
            dBContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Event> eventcollection = dBContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("_id", id);
            return eventcollection.DeleteOne(filter);
        }
    }
}
