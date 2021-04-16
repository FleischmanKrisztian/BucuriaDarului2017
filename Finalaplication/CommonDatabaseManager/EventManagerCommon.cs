using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finalaplication.CommonDatabaseManager
{
    public class EventManagerCommon
    {
        private MongoDBContextCommon dbContextCommon = new MongoDBContextCommon();
        internal void AddEventToDB(Event ev)
        {
            IMongoCollection<Event> eventcollection = dbContextCommon.DatabaseCommon.GetCollection<Event>("Events");
            try
            {
                eventcollection.InsertOne(ev);
            }
            catch
            {
                Console.WriteLine("There was an error adding Event");
            }
        }

        internal Event GetOneEvent(string id)
        {
            IMongoCollection<Event> eventcollection = dbContextCommon.DatabaseCommon.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("_id", id);
            Event returnevent = eventcollection.Find(filter).FirstOrDefault();
            return returnevent;
        }

        internal List<Event> GetListOfEvents()
        {
            IMongoCollection<Event> eventcollection = dbContextCommon.DatabaseCommon.GetCollection<Event>("Events");
            List<Event> events = eventcollection.AsQueryable().ToList();
            return events;
        }

        internal void UpdateAnEvent(Event eventtoupdate, string id)
        {
            IMongoCollection<Event> eventcollection = dbContextCommon.DatabaseCommon.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("_id", id);
            eventtoupdate._id = id;
            eventcollection.FindOneAndReplace(filter, eventtoupdate);
        }

        internal void DeleteAnEvent(string id)
        {
            IMongoCollection<Event> eventcollection = dbContextCommon.DatabaseCommon.GetCollection<Event>("Events");
            eventcollection.DeleteOne(Builders<Event>.Filter.Eq("_id", id));
        }
    }
}
