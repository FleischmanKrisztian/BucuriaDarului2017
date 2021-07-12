using System.Collections.Generic;
using BucuriaDarului.Core;
using MongoDB.Driver;

namespace BucuriaDarului.Web.DatabaseManager
{
    public class VolunteerManager
    {
        private MongoDBContext dbContext;
        private ModifiedDocumentManager modifiedDocumentManager = new ModifiedDocumentManager();

        public VolunteerManager(string SERVER_NAME, int SERVER_PORT, string DATABASE_NAME)
        {
            dbContext = new MongoDBContext(SERVER_NAME, SERVER_PORT, DATABASE_NAME);
        }


        internal List<Volunteer> GetListOfVolunteers()
        {
            IMongoCollection<Volunteer> volunteercollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            List<Volunteer> volunteers = volunteercollection.AsQueryable().ToList();
            return volunteers;
        }

        
    }
}