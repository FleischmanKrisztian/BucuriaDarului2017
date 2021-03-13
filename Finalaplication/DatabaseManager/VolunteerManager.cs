using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Finalaplication.DatabaseHandler
{
    public class VolunteerManager
    {
        private MongoDBContext dbcontext = new MongoDBContext();

        internal void AddVolunteerToDB(Volunteer volunteer)
        {
            IMongoCollection<Volunteer> volunteercollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
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
            IMongoCollection<Volunteer> volunteercollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
            var filter = Builders<Volunteer>.Filter.Eq("_id", ObjectId.Parse(id));
            Volunteer volunteer = volunteercollection.Find(filter).FirstOrDefault();
            return volunteer;
        }
        internal List<Volunteer> GetListOfVolunteers()
        {
            IMongoCollection<Volunteer> volunteercollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
            List<Volunteer> volunteers = volunteercollection.AsQueryable().ToList();
            return volunteers;
        }

        internal void UpdateAVolunteer(FilterDefinition<Volunteer> filter, UpdateDefinition<Volunteer> volunteertoupdate)
        {
            IMongoCollection<Volunteer> volunteercollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
            volunteercollection.UpdateOne(filter, volunteertoupdate);
        }

        internal void DeleteAVolunteer(string id)
        {
            IMongoCollection<Volunteer> volunteercollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
            volunteercollection.DeleteOne(Builders<Volunteer>.Filter.Eq("_id", ObjectId.Parse(id)));
        }
    }
}
