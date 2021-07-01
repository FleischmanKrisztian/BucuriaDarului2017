using System.Collections.Generic;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using BucuriaDarului.Core.Gateways.EventGateways;
using MongoDB.Driver;

namespace BucuriaDarului.Gateway.EventGateways
{
    public class EventsMainDisplayIndexGateway : IEventsMainDisplayIndexGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public List<Event> GetListOfEvents()
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var eventCollection = dbContext.Database.GetCollection<Event>("Events");
            var events = eventCollection.AsQueryable().ToList();
            return events;
        }
    }
}