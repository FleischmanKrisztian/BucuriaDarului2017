using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway
{
    public class EventsMainDisplayIndexGateway : IEventsMainDisplayIndexGateway
    {
        private MongoDBGateway dBContext = new MongoDBGateway();

        public List<Event> GetListOfEvents()
        {
            dBContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Event> eventCollection = dBContext.Database.GetCollection<Event>("Events");
            List<Event> events = eventCollection.AsQueryable().ToList();
            return events;
        }
    }
}