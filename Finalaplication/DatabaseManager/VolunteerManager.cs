using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Finalaplication.LocalDatabaseManager
{
    public class VolunteerManager
    {
        MongoDBContext dBContext;
        public VolunteerManager(MongoDBContext mongoDBContext)
        {
            dBContext = mongoDBContext;
        }

        internal void AddVolunteerToDB(Volunteer volunteer)
        {
            IMongoCollection<Volunteer> volunteercollection = dBContext.Database.GetCollection<Volunteer>("Volunteers");
            try
            {
                volunteercollection.InsertOne(volunteer);
            }
            catch
            {
                Console.WriteLine("There was an error adding Volunteer!");
            }
        }

        internal Volunteer GetOneVolunteer(string id)
        {
            IMongoCollection<Volunteer> volunteercollection = dBContext.Database.GetCollection<Volunteer>("Volunteers");
            var filter = Builders<Volunteer>.Filter.Eq("_id", id);
            Volunteer volunteer = volunteercollection.Find(filter).FirstOrDefault();
            return volunteer;
        }

        internal List<Volunteer> GetListOfVolunteers()
        {
            IMongoCollection<Volunteer> volunteercollection = dBContext.Database.GetCollection<Volunteer>("Volunteers");
            List<Volunteer> volunteers = volunteercollection.AsQueryable().ToList();
            return volunteers;
        }

        internal void UpdateAVolunteer(Volunteer volunteertopdate, string id)
        {
            IMongoCollection<Volunteer> volunteercollection = dBContext.Database.GetCollection<Volunteer>("Volunteers");
            var filter = Builders<Volunteer>.Filter.Eq("_id", id);
            volunteertopdate._id = id;
            volunteercollection.FindOneAndReplace(filter, volunteertopdate);
        }

        internal void DeleteAVolunteer(string id)
        {
            IMongoCollection<Volunteer> volunteercollection = dBContext.Database.GetCollection<Volunteer>("Volunteers");
            volunteercollection.DeleteOne(Builders<Volunteer>.Filter.Eq("_id", id));
        }
    }
}