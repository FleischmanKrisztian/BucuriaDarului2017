using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Finalaplication.LocalDatabaseManager
{
    public class SponsorManager
    {
        private MongoDBContextLocal dBContextLocal = new MongoDBContextLocal();
        private ModifiedDocumentManager modifiedDocumentManager = new ModifiedDocumentManager();

        internal void AddSponsorToDB(Sponsor sponsor)
        {
            IMongoCollection<Sponsor> Sponsorcollection = dBContextLocal.DatabaseLocal.GetCollection<Sponsor>("Sponsors");
            modifiedDocumentManager.AddIDtoString(sponsor._id);
            Sponsorcollection.InsertOne(sponsor);
        }

        internal Sponsor GetOneSponsor(string id)
        {
            IMongoCollection<Sponsor> Sponsorcollection = dBContextLocal.DatabaseLocal.GetCollection<Sponsor>("Sponsors");
            var filter = Builders<Sponsor>.Filter.Eq("_id", id);
            Sponsor returnSponsor = Sponsorcollection.Find(filter).FirstOrDefault();
            return returnSponsor;
        }

        internal List<Sponsor> GetListOfSponsors()
        {
            IMongoCollection<Sponsor> Sponsorcollection = dBContextLocal.DatabaseLocal.GetCollection<Sponsor>("Sponsors");
            List<Sponsor> Sponsors = Sponsorcollection.AsQueryable().ToList();
            return Sponsors;
        }

        internal void UpdateSponsor(Sponsor sponsorupdate, string id)
        {
            IMongoCollection<Sponsor> sponsorcollection = dBContextLocal.DatabaseLocal.GetCollection<Sponsor>("Sponsors");
            var filter = Builders<Sponsor>.Filter.Eq("_id", id);
            sponsorupdate._id = id;
            modifiedDocumentManager.AddIDtoString(id);
            sponsorcollection.FindOneAndReplace(filter, sponsorupdate);
        }

        internal void DeleteSponsor(string id)
        {
            modifiedDocumentManager.AddIDtoDeletionString(id);
            IMongoCollection<Sponsor> Sponsorcollection = dBContextLocal.DatabaseLocal.GetCollection<Sponsor>("Sponsors");
            Sponsorcollection.DeleteOne(Builders<Sponsor>.Filter.Eq("_id", id));
        }
    }
}