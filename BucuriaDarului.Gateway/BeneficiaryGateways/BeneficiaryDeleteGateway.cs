using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryGateways;
using MongoDB.Driver;

namespace BucuriaDarului.Gateway.BeneficiaryGateways
{
    public class BeneficiaryDeleteGateway: IBeneficiaryDeleteGateway
    {
        MongoDBGateway dbContext = new MongoDBGateway();
        public  void DeleteBeneficiary(string id)
        {
            
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var beneficiaryCollection = dbContext.Database.GetCollection<Beneficiary>("Beneficiaries");
            var filter = Builders<Beneficiary>.Filter.Eq("_id", id);
            beneficiaryCollection.DeleteOne(filter);
        }

        public Beneficiary GetBeneficiary(string id)
        {
            return SingleBeneficiaryReturnerGateway.ReturnBeneficiary(id);
        }

        public void UpdateBeneficiary(string beneficiaryId, Beneficiary beneficiaryToUpdate)
        {

            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var beneficiaryCollection = dbContext.Database.GetCollection<Beneficiary>("Beneficiaries");
            var filter = Builders<Beneficiary>.Filter.Eq("_id", beneficiaryId);
            beneficiaryToUpdate.Id = beneficiaryId;
            var modifiedIDGateway = new ModifiedIDGateway();
            modifiedIDGateway.AddIDtoModifications(beneficiaryId);
            beneficiaryCollection.FindOneAndReplace(filter, beneficiaryToUpdate);
        }
    }
}