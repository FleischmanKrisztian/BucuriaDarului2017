using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finalaplication.DatabaseManager
{
    public class BeneficiaryContractManager
    {
        private MongoDBContext dbcontext = new MongoDBContext();

        internal void AddSponsorToDB(Beneficiarycontract beneficiarycontract)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dbcontext.database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            try
            {
                benecontractcollection.InsertOne(beneficiarycontract);
            }
            catch
            {
                Console.WriteLine("There was an error adding Sponsor");
            }
        }

        internal Beneficiarycontract GetBeneficiaryContract(string id)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dbcontext.database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            var filter = Builders<Beneficiarycontract>.Filter.Eq("_id", ObjectId.Parse(id));
            Beneficiarycontract returnBeneficiaryContract = benecontractcollection.Find(filter).FirstOrDefault();
            return returnBeneficiaryContract;
        }
        internal List<Beneficiarycontract> GetListOfBeneficiariesContracts()
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dbcontext.database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            List<Beneficiarycontract> contracts = benecontractcollection.AsQueryable().ToList();
            return contracts;
        }

        internal void UpdateBeneficiaryContract(FilterDefinition<Beneficiarycontract> filter, UpdateDefinition<Beneficiarycontract> contract_toupdate)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dbcontext.database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            benecontractcollection.UpdateOne(filter, contract_toupdate);
        }

        internal void DeleteBeneficiaryContract(string id)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dbcontext.database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            benecontractcollection.DeleteOne(Builders<Beneficiarycontract>.Filter.Eq("_id", ObjectId.Parse(id)));
        }

        internal void DeleteAllContracts(string id)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dbcontext.database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            benecontractcollection.DeleteMany(zzz => zzz.OwnerID == id);
        }
    }
}
