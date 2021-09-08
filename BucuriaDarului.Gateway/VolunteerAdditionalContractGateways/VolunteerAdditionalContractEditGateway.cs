using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.VolunteerContractGateways
{
    public class VolunteerAdditionalContractEditGateway : IVolunteerAdditionalContractEditGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public void Update(AdditionalContractVolunteer additionalContract)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var modifiedIdGateway = new ModifiedIDGateway();
            var additionalContractCollection = dbContext.Database.GetCollection<AdditionalContractVolunteer>("VolunteerAdditionalContracts");
            var filter = Builders<AdditionalContractVolunteer>.Filter.Eq("Id", additionalContract.Id);
            modifiedIdGateway.AddIDtoModifications(additionalContract.Id);
            additionalContractCollection.FindOneAndReplace(filter, additionalContract);
        }

        public List<AdditionalContractVolunteer> GetListOfAdditionalContracts(string id)
        {
            var dbContext = new MongoDBGateway();
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var additionalContractCollection = dbContext.Database.GetCollection<AdditionalContractVolunteer>("VolunteerAdditionalContracts");
            var filter = Builders<AdditionalContractVolunteer>.Filter.Eq("OwnerID", id);
            return additionalContractCollection.Find(filter).ToList();
        }

        public void AddVAdditionalContractToModifiedList(string beforeEditingAdditionalContract)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            BsonDocument.TryParse(beforeEditingAdditionalContract, out var documentAsBson);
            var auxiliaryCollection = dbContext.Database.GetCollection<BsonDocument>("Auxiliary");
            auxiliaryCollection.InsertOne(documentAsBson);
        }

    }
}