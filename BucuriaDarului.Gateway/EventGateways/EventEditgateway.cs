using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.EventGateways;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.EventGateways
{
    public class EventEditGateway : IEventEditGateway
    {
        private readonly MongoDBGateway dbContext = new MongoDBGateway();

        public void Edit(Event @event)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var eventCollection = dbContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("Id", @event.Id);
            var modifiedIdGateway = new ModifiedIDGateway();
            modifiedIdGateway.AddIDtoModifications(@event.Id);
            eventCollection.FindOneAndReplace(filter, @event);
        }

        public List<ModifiedIDs> ReturnModificationList()
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var modifiedCollection = dbContext.Database.GetCollection<ModifiedIDs>("ModifiedIDS");
            var modifiedIDs = modifiedCollection.AsQueryable().ToList();
            return modifiedIDs;
        }

        public void AddEventToModifiedList(string beforeEditingEventString)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            BsonDocument.TryParse(beforeEditingEventString, out var documentAsBson);
            var auxiliaryCollection = dbContext.Database.GetCollection<BsonDocument>("Auxiliary");
            auxiliaryCollection.InsertOne(documentAsBson);
        }

        public Event ReturnEvent(string eventId)
        {
            return SingleEventReturnerGateway.ReturnEvent(eventId);
        }
    }
}