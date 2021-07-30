using BucuriaDarului.Core;
using MongoDB.Driver;

namespace BucuriaDarului.Gateway.SponsorGateways
{
    public class SponsorDeleteGateway
    {
        public static void DeleteSponsor(string id)
        {
            MongoDBGateway dbContext = new MongoDBGateway();
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Sponsor> eventCollection = dbContext.Database.GetCollection<Sponsor>("Sponsors");
            var filter = Builders<Sponsor>.Filter.Eq("Id", id);
            var deletedIdGateway = new DeletedIDGateway();
            deletedIdGateway.AddIDtoDeletions(id);
            eventCollection.DeleteOne(filter);
        }
    }
}
