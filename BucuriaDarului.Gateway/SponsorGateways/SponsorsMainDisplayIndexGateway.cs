using System.Collections.Generic;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.SponsorGateways;
using MongoDB.Driver;

namespace BucuriaDarului.Gateway.SponsorGateways
{
    public class SponsorMainDisplayIndexGateway : ISponsorsMainDisplayIndexGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public List<BucuriaDarului.Core.Sponsor> GetListOfSponsors()
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var sponsorCollection = dbContext.Database.GetCollection<Sponsor>("Sponsors");
            // TODO: solve this exception
            var sponsors = sponsorCollection.AsQueryable().ToList();
            return sponsors;
        }
    }
}
