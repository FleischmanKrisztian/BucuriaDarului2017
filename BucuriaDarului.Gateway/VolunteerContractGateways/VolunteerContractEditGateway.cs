using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.VolunteerContractGateways
{
    public class VolunteerContractEditGateway : IVolunteerContractEditGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public void Update(VolunteerContract contract)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var modifiedIdGateway = new ModifiedIDGateway();
            var volunteerContractCollection = dbContext.Database.GetCollection<VolunteerContract>("Contracts");
            var filter = Builders<VolunteerContract>.Filter.Eq("Id", contract.Id);
            modifiedIdGateway.AddIDtoModifications(contract.Id);
            volunteerContractCollection.FindOneAndReplace(filter, contract);
        }

        public List<VolunteerContract> GetListOfVolunteersContracts(string id)
        {
            var dbContext = new MongoDBGateway();
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var volunteerContractCollection = dbContext.Database.GetCollection<VolunteerContract>("Contracts");
            var filter = Builders<VolunteerContract>.Filter.Eq("OwnerID", id);
            return volunteerContractCollection.Find(filter).ToList();
        }
    }
}