using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Finalaplication.LocalDatabaseManager
{
    public class EventManager
    {
        private MongoDBContext dBContext;
        private ModifiedDocumentManager modifiedDocumentManager = new ModifiedDocumentManager();

        public EventManager(string SERVER_NAME, int SERVER_PORT, string DATABASE_NAME)
        {
            dBContext = new MongoDBContext(SERVER_NAME, SERVER_PORT, DATABASE_NAME);
        }

        internal void AddEventToDB(Event ev)
        {
            IMongoCollection<Event> eventcollection = dBContext.Database.GetCollection<Event>("Events");
            modifiedDocumentManager.AddIDtoString(ev._id);
            eventcollection.InsertOne(ev);
        }

        internal Event GetOneEvent(string id)
        {
            IMongoCollection<Event> eventcollection = dBContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("_id", id);
            Event returnevent = eventcollection.Find(filter).FirstOrDefault();
            return returnevent;
        }

        internal List<Event> GetListOfEvents()
        {
            IMongoCollection<Event> eventcollection = dBContext.Database.GetCollection<Event>("Events");
            List<Event> events = eventcollection.AsQueryable().ToList();
            return events;
        }

        internal void UpdateAnEvent(Event eventtoupdate, string id)
        {
            IMongoCollection<Event> eventcollection = dBContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("_id", id);
            eventtoupdate._id = id;
            modifiedDocumentManager.AddIDtoString(id);
            eventcollection.FindOneAndReplace(filter, eventtoupdate);
        }

        internal void DeleteAnEvent(string id)
        {
            modifiedDocumentManager.AddIDtoDeletionString(id);
            IMongoCollection<Event> eventcollection = dBContext.Database.GetCollection<Event>("Events");
            eventcollection.DeleteOne(Builders<Event>.Filter.Eq("_id", id));
        }
    }
}