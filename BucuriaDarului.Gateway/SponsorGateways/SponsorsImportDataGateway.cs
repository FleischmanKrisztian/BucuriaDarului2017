using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.SponsorGateways;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.SponsorGateways
{
    public class SponsorsImportDataGateway : ISponsorsImportDataGateway
    {
        private readonly MongoDBGateway dbContext = new MongoDBGateway();

       

        public Sponsor GetSponsor(string sponsorId)
        {
            return SingleSponsorReturnerGateway.ReturnSponsor(sponsorId);
        }

        public List<Sponsor> GetSponsors()
        {
            return ListSponsorsGateway.GetListOfSponsors();
        }

        public void Insert(List<Sponsor> sponsors)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var sponsorCollection = dbContext.Database.GetCollection<Sponsor>("Sponsors");
            var modifiedIdGateway = new ModifiedIDGateway();
            foreach (var spons in sponsors)
            {
                var filter = Builders<Sponsor>.Filter.Eq("Id", spons.Id);
                if (sponsorCollection.Find(filter).FirstOrDefault() == null)
                {
                    sponsorCollection.InsertOne(spons);
                    modifiedIdGateway.AddIDtoModifications(spons.Id);
                }
            }
        }

        public void Update(List<Sponsor> sponsors)
        {
            foreach (var sponsor in sponsors)
            {
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
                var sponsorCollection = dbContext.Database.GetCollection<Sponsor>("Sponsors");
                var filter = Builders<Sponsor>.Filter.Eq("Id", sponsor.Id);
                var modifiedIdGateway = new ModifiedIDGateway();
                modifiedIdGateway.AddIDtoModifications(sponsor.Id);
                sponsorCollection.FindOneAndReplace(filter, sponsor);
            }
        }
    }
}
