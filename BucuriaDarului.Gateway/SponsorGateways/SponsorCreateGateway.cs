using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.SponsorGateways;

namespace BucuriaDarului.Gateway.SponsorGateways

{
    public class SponsorCreateGateway : ISponsorCreateGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public void Insert(Sponsor sponsor)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var sponsorCollection = dbContext.Database.GetCollection<Sponsor>("Sponsor");
            sponsorCollection.InsertOne(sponsor);
            var modifiedIDGateway = new ModifiedIDGateway();
            modifiedIDGateway.AddIDtoModifications(sponsor.Id);
        }
    }
}
