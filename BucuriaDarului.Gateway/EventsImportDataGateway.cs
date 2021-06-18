using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway
{
    public class EventsImportDataGateway : IEventsImportDataGateway
    {
        private MongoDBGateway dBContext = new MongoDBGateway();

        public void Insert(List<Event> events)
        {
            dBContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Event> eventcollection = dBContext.Database.GetCollection<Event>("Events");
            eventcollection.InsertMany(events);
            ModifiedIDGateway modifiedIDGateway = new ModifiedIDGateway();
            foreach (var eve in events)
            {
                modifiedIDGateway.AddIDtoModifications(eve._id);
            }
        }
    }
}