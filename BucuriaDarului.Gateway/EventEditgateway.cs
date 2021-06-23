using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway
{
    public class EventEditGateway : IEventEditGateway
    {
        private MongoDBGateway dBContext = new MongoDBGateway();

        public Event ReturnEvent(string id)
        {
            dBContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Event> eventcollection = dBContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("_id", id);
            return eventcollection.Find(filter).FirstOrDefault();
        }

        public void Edit(Event @event)
        {
            dBContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Event> eventcollection = dBContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("_id", @event._id);
            ModifiedIDGateway modifiedIDGateway = new ModifiedIDGateway();
            modifiedIDGateway.AddIDtoModifications(@event._id); ;
            eventcollection.FindOneAndReplace(filter, @event);
        }

        public List<ModifiedIDs> ReturnModificationList()
        {
            dBContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<ModifiedIDs> modifiedcollection = dBContext.Database.GetCollection<ModifiedIDs>("ModifiedIDS");
            List<ModifiedIDs> modifiedIDs = modifiedcollection.AsQueryable().ToList();
            return modifiedIDs;
        }

        public void AddEventToModifiedList(string beforeEditingEventString)
        {
            dBContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            BsonDocument documentAsBson;
            BsonDocument.TryParse(beforeEditingEventString, out documentAsBson);
            IMongoCollection<BsonDocument> AuxiliaryCollection = dBContext.Database.GetCollection<BsonDocument>("Auxiliary");
            AuxiliaryCollection.InsertOne(documentAsBson);
        }
    }
}