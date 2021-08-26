using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryGateways;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.BeneficiaryGateways
{
    public class BeneficiaryEditGateway : IBeneficiaryEditGateway
    {
        private readonly MongoDBGateway dbContext = new MongoDBGateway();

        public void Edit(Beneficiary beneficiary)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var beneficiaryCollection = dbContext.Database.GetCollection<Beneficiary>("Beneficiaries");
            var filter = Builders<Beneficiary>.Filter.Eq("Id", beneficiary.Id);
            var modifiedIdGateway = new ModifiedIDGateway();
            modifiedIdGateway.AddIDtoModifications(beneficiary.Id);
            beneficiaryCollection.FindOneAndReplace(filter, beneficiary);
        }

        public List<ModifiedIDs> ReturnModificationList()
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var modifiedCollection = dbContext.Database.GetCollection<ModifiedIDs>("ModifiedIDS");
            var modifiedIDs = modifiedCollection.AsQueryable().ToList();
            return modifiedIDs;
        }

        public void AddBeneficiaryToModifiedList(string beforeEditingBeneficiaryString)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            BsonDocument.TryParse(beforeEditingBeneficiaryString, out var documentAsBson);
            var auxiliaryCollection = dbContext.Database.GetCollection<BsonDocument>("Auxiliary");
            auxiliaryCollection.InsertOne(documentAsBson);
        }

        public Beneficiary ReturnBeneficiary(string beneficiaryId)
        {
            return SingleBeneficiaryReturnerGateway.ReturnBeneficiary(beneficiaryId);
        }
    }
}