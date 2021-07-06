using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.EventGateways;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.EventGateways
{
    public class EventsImportDataGateway : IEventsImportDataGateway
    {
        private readonly MongoDBGateway dbContext = new MongoDBGateway();

        public void Insert(List<Event> events)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var eventCollection = dbContext.Database.GetCollection<Event>("Events");
            eventCollection.InsertMany(events);
            var modifiedIDGateway = new ModifiedIDGateway();
            foreach (var eve in events)
            {
                modifiedIDGateway.AddIDtoModifications(eve.Id);
            }
        }
    }
}