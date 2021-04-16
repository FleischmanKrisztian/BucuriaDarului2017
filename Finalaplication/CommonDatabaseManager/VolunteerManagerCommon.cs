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
    public class VolunteerManagerCommon
    {
        private MongoDBContextCommon dbContextCommon = new MongoDBContextCommon();

        internal void AddVolunteerToDB(Volunteer volunteer)
        {
            IMongoCollection<Volunteer> volunteercollection = dbContextCommon.DatabaseCommon.GetCollection<Volunteer>("Volunteers");
            try
            {
                volunteer._id = Guid.NewGuid().ToString();
                volunteercollection.InsertOne(volunteer);
            }
            catch
            {
                Console.WriteLine("There was an error adding Volunteer!");
            }
        }

        internal Volunteer GetOneVolunteer(string id)
        {
            IMongoCollection<Volunteer> volunteercollection = dbContextCommon.DatabaseCommon.GetCollection<Volunteer>("Volunteers");
            var filter = Builders<Volunteer>.Filter.Eq("_id", id);
            Volunteer volunteer = volunteercollection.Find(filter).FirstOrDefault();
            return volunteer;
        }

        internal List<Volunteer> GetListOfVolunteers()
        {
            IMongoCollection<Volunteer> volunteercollection = dbContextCommon.DatabaseCommon.GetCollection<Volunteer>("Volunteers");
            List<Volunteer> volunteers = volunteercollection.AsQueryable().ToList();
            return volunteers;
        }

        internal void UpdateAVolunteer(Volunteer volunteertopdate, string id)
        {
            IMongoCollection<Volunteer> volunteercollection = dbContextCommon.DatabaseCommon.GetCollection<Volunteer>("Volunteers");
            var filter = Builders<Volunteer>.Filter.Eq("_id", id);
            volunteertopdate._id = id;
            volunteercollection.FindOneAndReplace(filter, volunteertopdate);
        }

        internal void DeleteAVolunteer(string id)
        {
            IMongoCollection<Volunteer> volunteercollection = dbContextCommon.DatabaseCommon.GetCollection<Volunteer>("Volunteers");
            volunteercollection.DeleteOne(Builders<Volunteer>.Filter.Eq("_id", id));
        }
    }
}
