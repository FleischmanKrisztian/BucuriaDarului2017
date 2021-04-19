﻿using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Finalaplication.LocalDatabaseManager
{
    public class BeneficiaryContractManager
    {
        private MongoDBContextLocal dBContextLocal = new MongoDBContextLocal();
        private ModifiedDocumentManager modifiedDocumentManager = new ModifiedDocumentManager();

        internal void AddBeneficiaryContractToDB(Beneficiarycontract beneficiarycontract)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dBContextLocal.DatabaseLocal.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            modifiedDocumentManager.AddIDtoString(beneficiarycontract._id);
            benecontractcollection.InsertOne(beneficiarycontract);
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

        internal void UpdateBeneficiaryContract(Beneficiarycontract contractupdate, string id)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dBContextLocal.DatabaseLocal.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            var filter = Builders<Beneficiarycontract>.Filter.Eq("_id", id);
            contractupdate._id = id;
            modifiedDocumentManager.AddIDtoString(id);
            benecontractcollection.FindOneAndReplace(filter, contractupdate);
        }

        internal void DeleteBeneficiaryContract(string id)
        {
            modifiedDocumentManager.AddIDtoDeletionString(id);
            IMongoCollection<Beneficiarycontract> benecontractcollection = dBContextLocal.DatabaseLocal.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            benecontractcollection.DeleteOne(Builders<Beneficiarycontract>.Filter.Eq("_id", id));
        }

        internal void DeleteAllContracts(string id)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dBContextLocal.DatabaseLocal.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            var beneficiarycontractlist = benecontractcollection.Find(zz => zz.OwnerID == id).ToList();
            for (int i = 0; i < beneficiarycontractlist.Count(); i++)
            {
                modifiedDocumentManager.AddIDtoDeletionString(beneficiarycontractlist[i]._id);
            }
            benecontractcollection.DeleteMany(zzz => zzz.OwnerID == id);
        }
    }
}