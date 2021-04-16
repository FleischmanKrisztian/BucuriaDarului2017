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
        private MongoDBContextLocal dBContextLocal = new MongoDBContextLocal();

        internal void AddBeneficiaryContractToDB(Beneficiarycontract beneficiarycontract)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dBContextLocal.DatabaseLocal.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            try
            {
                beneficiarycontract._id = Guid.NewGuid().ToString();
                benecontractcollection.InsertOne(beneficiarycontract);
            }
            catch
            {
                Console.WriteLine("There was an error adding Beneficiary Contract");
            }
        }

        internal Beneficiarycontract GetBeneficiaryContract(string id)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dBContextLocal.DatabaseLocal.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            var filter = Builders<Beneficiarycontract>.Filter.Eq("_id", id);
            Beneficiarycontract returnBeneficiaryContract = benecontractcollection.Find(filter).FirstOrDefault();
            return returnBeneficiaryContract;
        }

        internal List<Beneficiarycontract> GetListOfBeneficiariesContracts()
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dBContextLocal.DatabaseLocal.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            List<Beneficiarycontract> contracts = benecontractcollection.AsQueryable().ToList();
            return contracts;
        }

        internal void UpdateBeneficiaryContract(FilterDefinition<Beneficiarycontract> filter, UpdateDefinition<Beneficiarycontract> contract_toupdate)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dBContextLocal.DatabaseLocal.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            benecontractcollection.UpdateOne(filter, contract_toupdate);
        }
        internal void UpdateBeneficiaryContract(Beneficiarycontract contractupdate, string id)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dBContextLocal.DatabaseLocal.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            var filter = Builders<Beneficiarycontract>.Filter.Eq("_id", ObjectId.Parse(id));
            contractupdate._id = id;
            benecontractcollection.FindOneAndReplace(filter, contractupdate);
        }

        internal void DeleteBeneficiaryContract(string id)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dBContextLocal.DatabaseLocal.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            benecontractcollection.DeleteOne(Builders<Beneficiarycontract>.Filter.Eq("_id", id));
        }

        internal void DeleteAllContracts(string id)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dBContextLocal.DatabaseLocal.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            benecontractcollection.DeleteMany(zzz => zzz.OwnerID == id);
        }
    }
}