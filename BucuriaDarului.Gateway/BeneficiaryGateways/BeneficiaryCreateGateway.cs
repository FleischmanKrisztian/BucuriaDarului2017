using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryGateways;

namespace BucuriaDarului.Gateway.BeneficiaryGateways

{
    public class BeneficiaryCreateGateway : IBeneficiaryCreateGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public void Insert(Beneficiary beneficiary)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var beneficiaryCollection = dbContext.Database.GetCollection<Beneficiary>("Beneficiaries");
            beneficiaryCollection.InsertOne(beneficiary);
            var modifiedIDGateway = new ModifiedIDGateway();
            modifiedIDGateway.AddIDtoModifications(beneficiary.Id);
        }
    }
}