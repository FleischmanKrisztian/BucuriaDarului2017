using System;
using System.Collections.Generic;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;

namespace BucuriaDarului.Gateway
{
    //TODO: MongoDBContext trebuie sa existe in acest proiect. trebuie mutat de unde este acum.

    static class GatewayConnectionData
    {
        private static string SERVER_NAME_LOCAL = Environment.GetEnvironmentVariable("volmongo_server");
        private static int SERVER_PORT_LOCAL = int.Parse(Environment.GetEnvironmentVariable("volmongo_port"));
        private static string DATABASE_NAME_LOCAL = Environment.GetEnvironmentVariable("volmongo_databasename");
    }

    public class EventsImportDataGateway : IEventsImportDataGateway
    {

        //private MongoDBContext dBContext;
        //   private ModifiedDocumentManager modifiedDocumentManager = new ModifiedDocumentManager();

        public EventsImportDataGateway()
        {
            //dBContext = new MongoDBContext(GatewayConnectionData.SERVER_NAME_LOCAL, GatewayConnectionData.SERVER_PORT_LOCAL, GatewayConnectionData.DATABASE_NAME_LOCAL);
        }

        public void Insert(List<Event> events)
        {
            foreach (var @event in events)
            {
                //IMongoCollection<Event> eventcollection = dBContext.Database.GetCollection<Event>("Events");
                //modifiedDocumentManager.AddIDtoString(ev._id);
                //eventcollection.InsertOne(ev);
            }

        }
    }
}