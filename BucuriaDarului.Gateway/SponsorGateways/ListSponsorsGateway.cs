using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.SponsorGateways;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.SponsorGateways
{
    public class ListSponsorsGateway 
    {
        public static List<Sponsor> GetListOfSponsors()
        {
            var dbContext = new MongoDBGateway();
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var sponsorCollection = dbContext.Database.GetCollection<Sponsor>("Sponsors");
            List<Sponsor> sponsors = sponsorCollection.AsQueryable().ToList();
            return sponsors;
        }
    }
}