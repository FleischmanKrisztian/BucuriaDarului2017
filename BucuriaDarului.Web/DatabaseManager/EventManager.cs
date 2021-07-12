using BucuriaDarului.Web.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Web.DatabaseManager
{
    public class EventManager
    {
        private MongoDBContext dbContext;
        private ModifiedDocumentManager modifiedDocumentManager = new ModifiedDocumentManager();

        public EventManager(string SERVER_NAME, int SERVER_PORT, string DATABASE_NAME)
        {
            dbContext = new MongoDBContext(SERVER_NAME, SERVER_PORT, DATABASE_NAME);
        }

        internal List<Event> GetListOfEvents()
        {
            IMongoCollection<Event> eventCollection = dbContext.Database.GetCollection<Event>("Events");
            List<Event> events = eventCollection.AsQueryable().ToList();
            return events;
        }
    }
}