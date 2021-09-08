using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.VolunteerContractGateways
{
    public class VolunteerAdditionalContractDeleteGateway : IVolunteerAdditionalContractDeleteGateway
    {
        public void Delete(string id)
        {
            var dbContext = new MongoDBGateway();
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var additionalContractCollection = dbContext.Database.GetCollection<AdditionalContractVolunteer>("VolunteerAdditionalContracts");
            var filter = Builders<AdditionalContractVolunteer>.Filter.Eq("Id", id);
            var deletedIdGateway = new DeletedIDGateway();
            deletedIdGateway.AddIDtoDeletions(id);
            additionalContractCollection.DeleteOne(filter);
        }

        
        public List<AdditionalContractVolunteer> GetListAdditionalContracts()
        {
            var dbContext = new MongoDBGateway();
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var additionalContractCollection = dbContext.Database.GetCollection<AdditionalContractVolunteer>("VolunteerAdditionalContracts");
            List<AdditionalContractVolunteer> contracts = additionalContractCollection.AsQueryable().ToList();
            return contracts;
        }
    }
}