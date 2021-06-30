using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway
{
    public class BeneficiaryImportGateway : IBeneficiaryImportGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public void Insert(List<Beneficiary> beneficiaries)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Beneficiary> beneficiaryCollection = dbContext.Database.GetCollection<Beneficiary>("Beneficiaries");
            beneficiaryCollection.InsertMany(beneficiaries);
            ModifiedIDGateway modifiedIDGateway = new ModifiedIDGateway();
            foreach (var beneficiary in beneficiaries)
            {
                modifiedIDGateway.AddIDtoModifications(beneficiary.Id);
            }
        }
    }
}