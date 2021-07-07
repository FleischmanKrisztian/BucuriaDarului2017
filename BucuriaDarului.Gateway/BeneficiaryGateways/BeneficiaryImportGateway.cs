using System.Collections.Generic;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryGateways;
using MongoDB.Driver;

namespace BucuriaDarului.Gateway.BeneficiaryGateways
{
    public class BeneficiaryImportGateway : IBeneficiaryImportGateway
    {
        private readonly MongoDBGateway dbContext = new MongoDBGateway();

        public void Insert(List<Beneficiary> beneficiaries)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var beneficiaryCollection = dbContext.Database.GetCollection<Beneficiary>("Beneficiaries");
            var modifiedIdGateway = new ModifiedIDGateway();
            foreach (var beneficiary in beneficiaries)
            {
                var filter = Builders<Beneficiary>.Filter.Eq("_id", beneficiary.Id);
                if (beneficiaryCollection.Find(filter).FirstOrDefault() == null)
                {
                    beneficiaryCollection.InsertOne(beneficiary);
                    modifiedIdGateway.AddIDtoModifications(beneficiary.Id);
                }
            }
        }
    }
}