using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryContractGateways;
using BucuriaDarului.Gateway.BeneficiaryGateways;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;


namespace BucuriaDarului.Gateway.BeneficiaryContractGateways
{
    public class BeneficiaryContractUpdateGateway : IBeneficiaryContractUpdateGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();
        public Beneficiary GetBeneficiary(string beneficiaryId)
        {
            return SingleBeneficiaryReturnerGateway.ReturnBeneficiary(beneficiaryId);
        }

        public List<BeneficiaryContract> GetContractsOfBeneficiary()
        {
            return ListBeneficiaryContractGateway.GetListBeneficiaryContracts();
        }

        public void UpdateBeneficiaryContract(BeneficiaryContract beneficiaryContract)
        {

            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var beneficiaryContractCollection = dbContext.Database.GetCollection<BeneficiaryContract>("BeneficiaryContracts");
            var filter = Builders<BeneficiaryContract>.Filter.Eq("Id", beneficiaryContract.Id);
            var modifiedIdGateway = new ModifiedIDGateway();
            modifiedIdGateway.AddIDtoModifications(beneficiaryContract.Id);
            beneficiaryContractCollection.FindOneAndReplace(filter, beneficiaryContract);
        }

      
        public List<ModifiedIDs> ReturnModificationList()
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var modifiedCollection = dbContext.Database.GetCollection<ModifiedIDs>("ModifiedIDS");
            var modifiedIDs = modifiedCollection.AsQueryable().ToList();
            return modifiedIDs;
        }

        public void AddBeneficiaryContractToModifiedList(string beforeEditingBeneficiaryContractString)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            BsonDocument.TryParse(beforeEditingBeneficiaryContractString, out var documentAsBson);
            var auxiliaryCollection = dbContext.Database.GetCollection<BsonDocument>("Auxiliary");
            auxiliaryCollection.InsertOne(documentAsBson);
        }
    }
}
