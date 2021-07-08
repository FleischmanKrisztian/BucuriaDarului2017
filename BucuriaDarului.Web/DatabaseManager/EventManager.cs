using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Finalaplication.LocalDatabaseManager
{
    public class EventManager
    {
        private MongoDBContext dbContext;
        private ModifiedDocumentManager modifiedDocumentManager = new ModifiedDocumentManager();

        public EventManager(string SERVER_NAME, int SERVER_PORT, string DATABASE_NAME)
        {
            dbContext = new MongoDBContext(SERVER_NAME, SERVER_PORT, DATABASE_NAME);
        }

        internal void AddEventToDB(Event ev)
        {
            IMongoCollection<Event> eventCollection = dbContext.Database.GetCollection<Event>("Events");
            modifiedDocumentManager.AddIDtoString(ev.Id);
            eventCollection.InsertOne(ev);
        }

        internal Event GetOneEvent(string id)
        {
            IMongoCollection<Event> eventCollection = dbContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("Id", id);
            Event returnevent = eventCollection.Find(filter).FirstOrDefault();
            return returnevent;
        }

        internal List<Event> GetListOfEvents()
        {
            IMongoCollection<Event> eventCollection = dbContext.Database.GetCollection<Event>("Events");
            List<Event> events = eventCollection.AsQueryable().ToList();
            return events;
        }

        internal void UpdateAnEvent(Event eventtoupdate, string id)
        {
            IMongoCollection<Event> eventCollection = dbContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("Id", id);
            eventtoupdate.Id = id;
            modifiedDocumentManager.AddIDtoString(id);
            eventCollection.FindOneAndReplace(filter, eventtoupdate);
        }

        internal void DeleteAnEvent(string id)
        {
            modifiedDocumentManager.AddIDtoDeletionString(id);
            IMongoCollection<Event> eventCollection = dbContext.Database.GetCollection<Event>("Events");
            eventCollection.DeleteOne(Builders<Event>.Filter.Eq("Id", id));
        }
    }
}