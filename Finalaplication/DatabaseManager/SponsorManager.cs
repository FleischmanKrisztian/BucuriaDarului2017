using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Finalaplication.DatabaseHandler
{
    public class SponsorManager
    {
        private MongoDBContext dbcontext = new MongoDBContext();

        internal void AddSponsorToDB(Sponsor sponsor)
        {
            IMongoCollection<Sponsor> Sponsorcollection = dbcontext.database.GetCollection<Sponsor>("Sponsors");
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
            IMongoCollection<Sponsor> Sponsorcollection = dbcontext.database.GetCollection<Sponsor>("Sponsors");
            var filter = Builders<Sponsor>.Filter.Eq("_id", ObjectId.Parse(id));
            Sponsor returnSponsor = Sponsorcollection.Find(filter).FirstOrDefault();
            return returnSponsor;
        }
        internal List<Sponsor> GetListOfSponsors()
        {
            IMongoCollection<Sponsor> Sponsorcollection = dbcontext.database.GetCollection<Sponsor>("Sponsors");
            List<Sponsor> Sponsors = Sponsorcollection.AsQueryable().ToList();
            return Sponsors;
        }

        internal void UpdateAnSponsor(FilterDefinition<Sponsor> filter ,UpdateDefinition<Sponsor> Sponsortoupdate)
        {
            IMongoCollection<Sponsor> Sponsorcollection = dbcontext.database.GetCollection<Sponsor>("Sponsors");
            Sponsorcollection.UpdateOne(filter, Sponsortoupdate);
        }

        
        internal void UpdateAnSponsor(Sponsor sponsorupdate, string id)
        {
            IMongoCollection<Sponsor> sponsorcollection = dbcontext.database.GetCollection<Sponsor>("Sponsors");
            var filter = Builders<Sponsor>.Filter.Eq("_id", ObjectId.Parse(id));
            sponsorupdate.SponsorID = id;
            sponsorcollection.FindOneAndReplace(filter, sponsorupdate);
        }

        internal void DeleteAnSponsor(string id)
        {
            IMongoCollection<Sponsor> Sponsorcollection = dbcontext.database.GetCollection<Sponsor>("Sponsors");
            Sponsorcollection.DeleteOne(Builders<Sponsor>.Filter.Eq("_id", ObjectId.Parse(id)));
        }
    }
}
