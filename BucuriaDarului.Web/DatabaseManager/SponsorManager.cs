using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Finalaplication.LocalDatabaseManager
{
    public class SponsorManager
    {
        private MongoDBContext dbContext;
        private ModifiedDocumentManager modifiedDocumentManager = new ModifiedDocumentManager();

        public SponsorManager(string SERVER_NAME, int SERVER_PORT, string DATABASE_NAME)
        {
            dbContext = new MongoDBContext(SERVER_NAME, SERVER_PORT, DATABASE_NAME);
        }

        internal void AddSponsorToDB(Sponsor sponsor)
        {
            IMongoCollection<Sponsor> Sponsorcollection = dbContext.Database.GetCollection<Sponsor>("Sponsors");
            modifiedDocumentManager.AddIDtoString(sponsor._id);
            Sponsorcollection.InsertOne(sponsor);
        }

        internal Sponsor GetOneSponsor(string id)
        {
            IMongoCollection<Sponsor> Sponsorcollection = dbContext.Database.GetCollection<Sponsor>("Sponsors");
            var filter = Builders<Sponsor>.Filter.Eq("_id", id);
            Sponsor returnSponsor = Sponsorcollection.Find(filter).FirstOrDefault();
            return returnSponsor;
        }

        internal List<Sponsor> GetListOfSponsors()
        {
            IMongoCollection<Sponsor> Sponsorcollection = dbContext.Database.GetCollection<Sponsor>("Sponsors");
            List<Sponsor> Sponsors = Sponsorcollection.AsQueryable().ToList();
            return Sponsors;
        }

        internal void UpdateSponsor(Sponsor sponsorupdate, string id)
        {
            IMongoCollection<Sponsor> sponsorcollection = dbContext.Database.GetCollection<Sponsor>("Sponsors");
            var filter = Builders<Sponsor>.Filter.Eq("_id", id);
            sponsorupdate._id = id;
            modifiedDocumentManager.AddIDtoString(id);
            sponsorcollection.FindOneAndReplace(filter, sponsorupdate);
        }

        internal void DeleteSponsor(string id)
        {
            modifiedDocumentManager.AddIDtoDeletionString(id);
            IMongoCollection<Sponsor> Sponsorcollection = dbContext.Database.GetCollection<Sponsor>("Sponsors");
            Sponsorcollection.DeleteOne(Builders<Sponsor>.Filter.Eq("_id", id));
        }
    }
}