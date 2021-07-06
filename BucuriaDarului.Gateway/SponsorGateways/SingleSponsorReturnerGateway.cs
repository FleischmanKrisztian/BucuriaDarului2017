using BucuriaDarului.Core;
using MongoDB.Driver;

namespace BucuriaDarului.Gateway.SponsorGateways
{
    public class SingleSponsorReturnerGateway
    {
        public static Sponsor ReturnSponsor(string id)
        {
            var dbContext = new MongoDBGateway();

            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var sponsorCollection = dbContext.Database.GetCollection<Sponsor>("Sponsors");
            var filter = Builders<Sponsor>.Filter.Eq("_id", id);
            return sponsorCollection.Find(filter).FirstOrDefault();
        }
    }
}