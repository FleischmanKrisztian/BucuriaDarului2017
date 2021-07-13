﻿using System;
using System.Collections.Generic;
using System.Linq;
using BucuriaDarului.Web.Models;
using MongoDB.Driver;

namespace BucuriaDarului.Web.DatabaseManager
{
    public class BeneficiaryContractManager
    {
        private MongoDBContext dbContext;
        private ModifiedDocumentManager modifiedDocumentManager = new ModifiedDocumentManager();

        public BeneficiaryContractManager(string SERVER_NAME, int SERVER_PORT, string DATABASE_NAME)
        {
            dbContext = new MongoDBContext(SERVER_NAME, SERVER_PORT, DATABASE_NAME);
        }

        internal void AddBeneficiaryContractToDB(Beneficiarycontract beneficiarycontract)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dbContext.Database.GetCollection<Beneficiarycontract>("BeneficiaryContracts");
            modifiedDocumentManager.AddIDtoString(beneficiarycontract.Id);
            benecontractcollection.InsertOne(beneficiarycontract);
        }
        internal static string SERVER_NAME_LOCAL = Environment.GetEnvironmentVariable("volmongo_server");
        internal static int SERVER_PORT_LOCAL = int.Parse(Environment.GetEnvironmentVariable("volmongo_port"));
        internal static string DATABASE_NAME_LOCAL = Environment.GetEnvironmentVariable("volmongo_databasename");
        internal Beneficiarycontract GetBeneficiaryContract(string id)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dbContext.Database.GetCollection<Beneficiarycontract>("BeneficiaryContracts");
            var filter = Builders<Beneficiarycontract>.Filter.Eq("Id", id);
            Beneficiarycontract returnBeneficiaryContract = benecontractcollection.Find(filter).FirstOrDefault();
            return returnBeneficiaryContract;
        }

        internal List<Beneficiarycontract> GetListOfBeneficiaryContracts()
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dbContext.Database.GetCollection<Beneficiarycontract>("BeneficiaryContracts");
            List<Beneficiarycontract> contracts = benecontractcollection.AsQueryable().ToList();
            return contracts;
        }

        internal void UpdateBeneficiaryContract(Beneficiarycontract contractupdate, string id)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dbContext.Database.GetCollection<Beneficiarycontract>("BeneficiaryContracts");
            var filter = Builders<Beneficiarycontract>.Filter.Eq("Id", id);
            contractupdate.Id = id;
            modifiedDocumentManager.AddIDtoString(id);
            benecontractcollection.FindOneAndReplace(filter, contractupdate);
        }

        internal void DeleteBeneficiaryContract(string id)
        {
            modifiedDocumentManager.AddIDtoDeletionString(id);
            IMongoCollection<Beneficiarycontract> benecontractcollection = dbContext.Database.GetCollection<Beneficiarycontract>("BeneficiaryContracts");
            benecontractcollection.DeleteOne(Builders<Beneficiarycontract>.Filter.Eq("Id", id));
        }

        internal void DeleteAllContracts(string id)
        {
            IMongoCollection<Beneficiarycontract> benecontractcollection = dbContext.Database.GetCollection<Beneficiarycontract>("BeneficiaryContracts");
            var beneficiarycontractlist = benecontractcollection.Find(zz => zz.OwnerID == id).ToList();
            for (int i = 0; i < beneficiarycontractlist.Count(); i++)
            {
                modifiedDocumentManager.AddIDtoDeletionString(beneficiarycontractlist[i].Id);
            }
            benecontractcollection.DeleteMany(zzz => zzz.OwnerID == id);
        }
    }
}