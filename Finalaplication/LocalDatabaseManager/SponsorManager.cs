using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Finalaplication.DatabaseManager
{
    public class SponsorManager
    {
        private MongoDBContextLocal dBContextLocal = new MongoDBContextLocal();

        internal void AddSponsorToDB(Sponsor sponsor)
        {
            IMongoCollection<Sponsor> Sponsorcollection = dBContextLocal.DatabaseLocal.GetCollection<Sponsor>("Sponsors");
            try
            {
                sponsor._id = Guid.NewGuid().ToString();
                Sponsorcollection.InsertOne(sponsor);
            }
            catch
            {
                Console.WriteLine("There was an error adding Sponsor");
            }
        }

        internal Sponsor GetOneSponsor(string id)
        {
            IMongoCollection<Sponsor> Sponsorcollection = dBContextLocal.DatabaseLocal.GetCollection<Sponsor>("Sponsors");
            var filter = Builders<Sponsor>.Filter.Eq("_id", ObjectId.Parse(id));
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
            var filter = Builders<Sponsor>.Filter.Eq("_id", ObjectId.Parse(id));
            sponsorupdate._id = id;
            sponsorcollection.FindOneAndReplace(filter, sponsorupdate);
        }

        internal void DeleteSponsor(string id)
        {
            IMongoCollection<Sponsor> Sponsorcollection = dBContextLocal.DatabaseLocal.GetCollection<Sponsor>("Sponsors");
            Sponsorcollection.DeleteOne(Builders<Sponsor>.Filter.Eq("_id", ObjectId.Parse(id)));
        }
    }
}