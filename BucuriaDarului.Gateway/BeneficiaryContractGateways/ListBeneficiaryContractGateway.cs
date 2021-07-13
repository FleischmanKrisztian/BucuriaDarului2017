using BucuriaDarului.Core;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.BeneficiaryContractGateways
{
    public class ListBeneficiaryContractGateway
    {
        public static List<BeneficiaryContract> GetListBeneficiaryContracts()
        {
            var dbContext = new MongoDBGateway();
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var beneficiaryContractCollection = dbContext.Database.GetCollection<BeneficiaryContract>("BeneficiaryContracts");
            List<BeneficiaryContract> contracts = beneficiaryContractCollection.AsQueryable().ToList();
            return contracts;
        }
    }
}