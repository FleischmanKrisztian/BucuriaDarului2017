using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using BucuriaDarului.Core;

namespace Finalaplication.LocalDatabaseManager
{
    public class VolunteerManager
    {
        private MongoDBContext dbContext;
        private ModifiedDocumentManager modifiedDocumentManager = new ModifiedDocumentManager();

        public VolunteerManager(string SERVER_NAME, int SERVER_PORT, string DATABASE_NAME)
        {
            dbContext = new MongoDBContext(SERVER_NAME, SERVER_PORT, DATABASE_NAME);
        }

        internal void AddVolunteerToDB(Volunteer volunteer)
        {
            IMongoCollection<Volunteer> volunteercollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            modifiedDocumentManager.AddIDtoString(volunteer.Id);
            volunteercollection.InsertOne(volunteer);
        }

        internal Volunteer GetOneVolunteer(string id)
        {
            IMongoCollection<Volunteer> volunteercollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            var filter = Builders<Volunteer>.Filter.Eq("_id", id);
            Volunteer volunteer = volunteercollection.Find(filter).FirstOrDefault();
            return volunteer;
        }

        internal List<Volunteer> GetListOfVolunteers()
        {
            IMongoCollection<Volunteer> volunteercollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            List<Volunteer> volunteers = volunteercollection.AsQueryable().ToList();
            return volunteers;
        }

        internal void UpdateAVolunteer(Volunteer volunteertopdate, string id)
        {
            IMongoCollection<Volunteer> volunteercollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            var filter = Builders<Volunteer>.Filter.Eq("_id", id);
            volunteertopdate.Id = id;
            modifiedDocumentManager.AddIDtoString(volunteertopdate.Id);
            volunteercollection.FindOneAndReplace(filter, volunteertopdate);
        }

        internal void DeleteAVolunteer(string id)
        {
            IMongoCollection<Volunteer> volunteercollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            modifiedDocumentManager.AddIDtoDeletionString(id);
            volunteercollection.DeleteOne(Builders<Volunteer>.Filter.Eq("_id", id));
        }
    }
}