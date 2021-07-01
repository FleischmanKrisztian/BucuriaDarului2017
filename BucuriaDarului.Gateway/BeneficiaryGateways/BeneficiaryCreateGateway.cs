using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using BucuriaDarului.Core.Gateways.BeneficiaryGateways;
using MongoDB.Driver;

namespace BucuriaDarului.Gateway.BeneficiaryGateways

{
    public class BeneficiaryCreateGateway : IBeneficiaryCreateGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public void Insert(Beneficiary beneficiary)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var eventCollection = dbContext.Database.GetCollection<Beneficiary>("Beneficiaries");
            eventCollection.InsertOne(beneficiary);
            var modifiedIDGateway = new ModifiedIDGateway();
            modifiedIDGateway.AddIDtoModifications(beneficiary.Id);
        }
    }
}