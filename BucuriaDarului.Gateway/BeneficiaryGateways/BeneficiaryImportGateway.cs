using System.Collections.Generic;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryGateways;
using MongoDB.Driver;

namespace BucuriaDarului.Gateway.BeneficiaryGateways
{
    public class BeneficiaryImportGateway : IBeneficiaryImportGateway
    {
        private readonly MongoDBGateway dbContext = new MongoDBGateway();


        public List<Beneficiary> GetBenficiariesList()
        {
            return ListBeneficiariesGateway.GetListOfBeneficiaries();
        }

        public void Insert(List<Beneficiary> beneficiaries)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var beneficiaryCollection = dbContext.Database.GetCollection<Beneficiary>("Beneficiaries");
            var modifiedIdGateway = new ModifiedIDGateway();
            foreach (var beneficiary in beneficiaries)
            {
                var filter = Builders<Beneficiary>.Filter.Eq("Id", beneficiary.Id);
                if (beneficiaryCollection.Find(filter).FirstOrDefault() == null)
                {
                    beneficiaryCollection.InsertOne(beneficiary);
                    modifiedIdGateway.AddIDtoModifications(beneficiary.Id);
                }
            }
        }

        public void InsertBeneficiaryContracts(List<BeneficiaryContract> contracts)
        {
            foreach (var contract in contracts)
            {
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
                var beneficiaryContractCollection = dbContext.Database.GetCollection<BeneficiaryContract>("BeneficiaryContracts");
                beneficiaryContractCollection.InsertOne(contract);
                var modifiedIdGateway = new ModifiedIDGateway();
                modifiedIdGateway.AddIDtoModifications(contract.Id);

            }
        }

        public void Update(List<Beneficiary> beneficiaries)
        {
            foreach (var beneficiary in beneficiaries)
            {
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
                var beneficiaryCollection = dbContext.Database.GetCollection<Beneficiary>("Beneficiaries");
                var filter = Builders<Beneficiary>.Filter.Eq("Id", beneficiary.Id);
                var modifiedIdGateway = new ModifiedIDGateway();
                modifiedIdGateway.AddIDtoModifications(beneficiary.Id);
                beneficiaryCollection.FindOneAndReplace(filter, beneficiary);
            }
        }
    }
}