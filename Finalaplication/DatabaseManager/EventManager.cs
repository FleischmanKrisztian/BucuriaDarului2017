using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Finalaplication.DatabaseHandler
{
    public class EventManager
    {
        private MongoDBContext dbcontext = new MongoDBContext();
        
        internal void AddEventToDB(Event ev)
        {
            IMongoCollection<Event> eventcollection = dbcontext.database.GetCollection<Event>("Events");
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
            IMongoCollection<Event> eventcollection = dbcontext.database.GetCollection<Event>("Events");
            Event returnevent = eventcollection.AsQueryable().First();
            return returnevent;
        }
        internal List<Event> GetListOfEvents()
        {
            IMongoCollection<Event> eventcollection = dbcontext.database.GetCollection<Event>("Events");
            List <Event> events = eventcollection.AsQueryable().ToList();
            return events;
        }

        internal void UpdateAnEvent(FilterDefinition<Event> filter, UpdateDefinition<Event> eventtoupdate)
        {
            IMongoCollection<Event> eventcollection = dbcontext.database.GetCollection<Event>("Events");
            eventcollection.UpdateOne(filter, eventtoupdate);
        }

        internal void DeleteAnEvent(string id)
        {
            IMongoCollection<Event> eventcollection = dbcontext.database.GetCollection<Event>("Events");
            eventcollection.DeleteOne(Builders<Event>.Filter.Eq("_id", ObjectId.Parse(id)));
        }
    }
}
