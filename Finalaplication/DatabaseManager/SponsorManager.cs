using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Finalaplication.LocalDatabaseManager
{
    public class SponsorManager
    {
        MongoDBContext dBContext;
        public SponsorManager(MongoDBContext mongoDBContext)
        {
            dBContext = mongoDBContext;
        }

        internal void AddSponsorToDB(Sponsor sponsor)
        {
            IMongoCollection<Sponsor> Sponsorcollection = dBContext.Database.GetCollection<Sponsor>("Sponsors");
            try
            {
                Sponsorcollection.InsertOne(sponsor);
            }
            catch
            {
                Console.WriteLine("There was an error adding Sponsor");
            }
        }

        internal Sponsor GetOneSponsor(string id)
        {
            IMongoCollection<Sponsor> Sponsorcollection = dBContext.Database.GetCollection<Sponsor>("Sponsors");
            var filter = Builders<Sponsor>.Filter.Eq("_id", id);
            Sponsor returnSponsor = Sponsorcollection.Find(filter).FirstOrDefault();
            return returnSponsor;
        }

        internal List<Sponsor> GetListOfSponsors()
        {
            IMongoCollection<Sponsor> Sponsorcollection = dBContext.Database.GetCollection<Sponsor>("Sponsors");
            List<Sponsor> Sponsors = Sponsorcollection.AsQueryable().ToList();
            return Sponsors;
        }

        internal void UpdateSponsor(Sponsor sponsorupdate, string id)
        {
            IMongoCollection<Sponsor> sponsorcollection = dBContext.Database.GetCollection<Sponsor>("Sponsors");
            var filter = Builders<Sponsor>.Filter.Eq("_id", id);
            sponsorupdate._id = id;
            sponsorcollection.FindOneAndReplace(filter, sponsorupdate);
        }

        internal void DeleteSponsor(string id)
        {
            IMongoCollection<Sponsor> Sponsorcollection = dBContext.Database.GetCollection<Sponsor>("Sponsors");
            Sponsorcollection.DeleteOne(Builders<Sponsor>.Filter.Eq("_id", id));
        }
    }
}