using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryContractGateways;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.BeneficiaryContractGateways
{
    public class BeneficiaryContractUpdateGateway : IBeneficiaryContractUpdateGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public void Update(BeneficiaryContract contract)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var modifiedIdGateway = new ModifiedIDGateway();
            var BeneficiaryContractCollection = dbContext.Database.GetCollection<BeneficiaryContract>("BeneficiaryContracts");
            var filter = Builders<BeneficiaryContract>.Filter.Eq("Id", contract.Id);
            modifiedIdGateway.AddIDtoModifications(contract.Id);
            BeneficiaryContractCollection.FindOneAndReplace(filter, contract);
        }

        public List<BeneficiaryContract> GetListOfBeneficiaryContracts(string id)
        {
            var dbContext = new MongoDBGateway();
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var BeneficiaryContractCollection = dbContext.Database.GetCollection<BeneficiaryContract>("BeneficiaryContracts");
            var filter = Builders<BeneficiaryContract>.Filter.Eq("OwnerID", id);
            return BeneficiaryContractCollection.Find(filter).ToList();
        }

        public void AddBeneficiaryContractToModifiedList(string beforeEditingBeneficiaryContract)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            BsonDocument.TryParse(beforeEditingBeneficiaryContract, out var documentAsBson);
            var auxiliaryCollection = dbContext.Database.GetCollection<BsonDocument>("Auxiliary");
            auxiliaryCollection.InsertOne(documentAsBson);
        }
    }
}