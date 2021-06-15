using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway
{
    public class EventsMainDisplayIndexGateway : IEventsMainDisplayIndexGateway
    {
        private MongoDBGateway dBContext = new MongoDBGateway();
        internal static string SERVER_NAME_LOCAL = Environment.GetEnvironmentVariable("volmongo_server");
        internal static int SERVER_PORT_LOCAL = int.Parse(Environment.GetEnvironmentVariable("volmongo_port"));
        internal static string DATABASE_NAME_LOCAL = Environment.GetEnvironmentVariable("volmongo_databasename");

        public List<Event> GetListOfEvents()
        {
            dBContext.ConnectToDB(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);
            IMongoCollection<Event> eventcollection = dBContext.Database.GetCollection<Event>("Events");
            List<Event> events = eventcollection.AsQueryable().ToList();
            return events;
        }
    }
}