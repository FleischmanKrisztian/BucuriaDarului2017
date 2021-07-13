using BucuriaDarului.Core;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.BeneficiaryGateways
{
    public class ListBeneficiariesGateway
    {
        public static  List<Beneficiary> GetListOfBeneficiaries()
        {
            var dbContext = new MongoDBGateway();
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var beneficiaryCollection = dbContext.Database.GetCollection<Beneficiary>("Beneficiaries");
            var beneficiaries = beneficiaryCollection.AsQueryable().ToList();
            return beneficiaries;
        }
    }
}