using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Finalaplication.LocalDatabaseManager
{
    public class BeneficiaryContractManager
    {
        MongoDBContext dBContext;

        public BeneficiaryContractManager(MongoDBContext mongoDBContext)
        {
            dBContext = mongoDBContext;
        }

        internal void AddBeneficiaryContractToDB(Beneficiarycontract beneficiarycontract)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dBContext.Database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            try
            {
                benecontractcollection.InsertOne(beneficiarycontract);
            }
            catch
            {
                Console.WriteLine("There was an error adding Beneficiary Contract");
            }
        }

        internal Beneficiarycontract GetBeneficiaryContract(string id)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dBContext.Database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            var filter = Builders<Beneficiarycontract>.Filter.Eq("_id", id);
            Beneficiarycontract returnBeneficiaryContract = benecontractcollection.Find(filter).FirstOrDefault();
            return returnBeneficiaryContract;
        }

        internal List<Beneficiarycontract> GetListOfBeneficiariesContracts()
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dBContext.Database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            List<Beneficiarycontract> contracts = benecontractcollection.AsQueryable().ToList();
            return contracts;
        }

        internal void UpdateBeneficiaryContract(FilterDefinition<Beneficiarycontract> filter, UpdateDefinition<Beneficiarycontract> contract_toupdate)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dBContext.Database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            benecontractcollection.UpdateOne(filter, contract_toupdate);
        }
        internal void UpdateBeneficiaryContract(Beneficiarycontract contractupdate, string id)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dBContext.Database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            var filter = Builders<Beneficiarycontract>.Filter.Eq("_id", id);
            contractupdate._id = id;
            benecontractcollection.FindOneAndReplace(filter, contractupdate);
        }

        internal void DeleteBeneficiaryContract(string id)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dBContext.Database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            benecontractcollection.DeleteOne(Builders<Beneficiarycontract>.Filter.Eq("_id", id));
        }

        internal void DeleteAllContracts(string id)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dBContext.Database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            benecontractcollection.DeleteMany(zzz => zzz.OwnerID == id);
        }
    }
}