using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Finalaplication.LocalDatabaseManager
{
    public class BeneficiaryContractManager
    {
        private MongoDBContext dBContext;
        private ModifiedDocumentManager modifiedDocumentManager = new ModifiedDocumentManager();

        public BeneficiaryContractManager(string SERVER_NAME, int SERVER_PORT, string DATABASE_NAME)
        {
            dBContext = new MongoDBContext(SERVER_NAME, SERVER_PORT, DATABASE_NAME);
        }

        internal void AddBeneficiaryContractToDB(Beneficiarycontract beneficiarycontract)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dBContext.Database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            modifiedDocumentManager.AddIDtoString(beneficiarycontract._id);
            benecontractcollection.InsertOne(beneficiarycontract);
        }
        internal static string SERVER_NAME_LOCAL = Environment.GetEnvironmentVariable("volmongo_server");
        internal static int SERVER_PORT_LOCAL = int.Parse(Environment.GetEnvironmentVariable("volmongo_port"));
        internal static string DATABASE_NAME_LOCAL = Environment.GetEnvironmentVariable("volmongo_databasename");
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

        internal void UpdateBeneficiaryContract(Beneficiarycontract contractupdate, string id)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dBContext.Database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            var filter = Builders<Beneficiarycontract>.Filter.Eq("_id", id);
            contractupdate._id = id;
            modifiedDocumentManager.AddIDtoString(id);
            benecontractcollection.FindOneAndReplace(filter, contractupdate);
        }

        internal void DeleteBeneficiaryContract(string id)
        {
            modifiedDocumentManager.AddIDtoDeletionString(id);
            IMongoCollection<Beneficiarycontract> benecontractcollection = dBContext.Database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            benecontractcollection.DeleteOne(Builders<Beneficiarycontract>.Filter.Eq("_id", id));
        }

        internal void DeleteAllContracts(string id)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dBContext.Database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            var beneficiarycontractlist = benecontractcollection.Find(zz => zz.OwnerID == id).ToList();
            for (int i = 0; i < beneficiarycontractlist.Count(); i++)
            {
                modifiedDocumentManager.AddIDtoDeletionString(beneficiarycontractlist[i]._id);
            }
            benecontractcollection.DeleteMany(zzz => zzz.OwnerID == id);
        }
    }
}