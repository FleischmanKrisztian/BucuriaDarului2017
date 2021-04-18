using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Finalaplication.LocalDatabaseManager
{
    public class EventManager
    {
        MongoDBContext dBContext;
        public EventManager(MongoDBContext mongoDBContext)
        {
            dBContext = mongoDBContext;
        }

        internal void AddEventToDB(Event ev)
        {
            IMongoCollection<Event> eventcollection = dBContext.Database.GetCollection<Event>("Events");
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
            eventcollection.FindOneAndReplace(filter, eventtoupdate);
        }

        internal void DeleteAnEvent(string id)
        {
            IMongoCollection<Event> eventcollection = dBContext.Database.GetCollection<Event>("Events");
            eventcollection.DeleteOne(Builders<Event>.Filter.Eq("_id", id));
        }
    }
}