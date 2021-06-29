using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway
{
    public class EventEditGateway : IEventEditGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public void Edit(Event @event)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Event> eventCollection = dbContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("_id", @event.Id);
            ModifiedIDGateway modifiedIDGateway = new ModifiedIDGateway();
            modifiedIDGateway.AddIDtoModifications(@event.Id);
            eventCollection.FindOneAndReplace(filter, @event);
        }

        public List<ModifiedIDs> ReturnModificationList()
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<ModifiedIDs> modifiedcollection = dbContext.Database.GetCollection<ModifiedIDs>("ModifiedIDS");
            List<ModifiedIDs> modifiedIDs = modifiedcollection.AsQueryable().ToList();
            return modifiedIDs;
        }

        public void AddEventToModifiedList(string beforeEditingEventString)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            BsonDocument documentAsBson;
            BsonDocument.TryParse(beforeEditingEventString, out documentAsBson);
            IMongoCollection<BsonDocument> AuxiliaryCollection = dbContext.Database.GetCollection<BsonDocument>("Auxiliary");
            AuxiliaryCollection.InsertOne(documentAsBson);
        }

        public Event ReturnEvent(string eventId)
        {
            return SingleEventReturnerGateway.ReturnEvent(eventId);
        }
    }
}