using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryContractGateways;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.BeneficiaryContractGateways
{
    public class BeneficiaryContractDeleteGateway : IBeneficiaryContractDeleteGateway
    {
        public void Delete(string id)
        {
            var dbContext = new MongoDBGateway();
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var beneficiaryContractCollection = dbContext.Database.GetCollection<BeneficiaryContract>("BeneficiaryContracts");
            var filter = Builders<BeneficiaryContract>.Filter.Eq("Id", id);
            var deletedIdGateway = new DeletedIDGateway();
            deletedIdGateway.AddIDtoDeletions(id);
            beneficiaryContractCollection.DeleteOne(filter);
        }

        public List<BeneficiaryContract> GetListBeneficiaryContracts()
        {
            var dbContext = new MongoDBGateway();
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var beneficiaryContractCollection = dbContext.Database.GetCollection<BeneficiaryContract>("BeneficiaryContracts");
            var contracts = beneficiaryContractCollection.AsQueryable().ToList();
            return contracts;
        }
    }
}