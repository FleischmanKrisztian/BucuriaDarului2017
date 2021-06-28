using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway
{
    public class EventsMainDisplayIndexGateway : IEventsMainDisplayIndexGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public List<Event> GetListOfEvents()
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Event> eventCollection = dbContext.Database.GetCollection<Event>("Events");
            List<Event> events = eventCollection.AsQueryable().ToList();
            return events;
        }
    }
}