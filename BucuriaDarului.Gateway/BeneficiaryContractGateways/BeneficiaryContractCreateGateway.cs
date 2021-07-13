using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryContractGateways;
using BucuriaDarului.Gateway.BeneficiaryGateways;

namespace BucuriaDarului.Gateway.BeneficiaryContractGateways
{
    public class BeneficiaryContractCreateGateway : IBeneficiaryContractCreateGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public Beneficiary GetBeneficiary(string idOfBeneficiary)
        {
            return SingleBeneficiaryReturnerGateway.ReturnBeneficiary(idOfBeneficiary);
        }

        public void Insert(BeneficiaryContract beneficiaryContract)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var beneficiaryContractCollection = dbContext.Database.GetCollection<BeneficiaryContract>("BeneficiaryContracts");
            beneficiaryContractCollection.InsertOne(beneficiaryContract);
            var modifiedIdGateway = new ModifiedIDGateway();
            modifiedIdGateway.AddIDtoModifications(beneficiaryContract.Id);
        }
    }
}