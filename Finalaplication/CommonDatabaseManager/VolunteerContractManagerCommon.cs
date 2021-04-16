using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finalaplication.CommonDatabaseManager
{
    public class VolunteerContractManagerCommon
    {
        private MongoDBContextCommon dbContextCommon = new MongoDBContextCommon();

        internal void DeleteAVolunteersContracts(string id)
        {
            IMongoCollection<Volcontract> contractcollection = dbContextCommon.DatabaseCommon.GetCollection<Volcontract>("Contracts");
            contractcollection.DeleteMany(Builders<Volcontract>.Filter.Eq("OwnerID", id));
        }

        internal void DeleteAVolContract(string id)
        {
            IMongoCollection<Volcontract> contractcollection = dbContextCommon.DatabaseCommon.GetCollection<Volcontract>("Contracts");
            contractcollection.DeleteOne(Builders<Volcontract>.Filter.Eq("_id", id));
        }

        internal void AddVolunteerContractToDB(Volcontract contract)
        {
            IMongoCollection<Volcontract> volcontractcollection = dbContextCommon.DatabaseCommon.GetCollection<Volcontract>("Contracts");
            try
            {
                contract._id = Guid.NewGuid().ToString();
                volcontractcollection.InsertOne(contract);
            }
            catch
            {
                Console.WriteLine("There was an error adding Sponsor");
            }
        }

        internal Volcontract GetVolunteerContract(string id)
        {
            IMongoCollection<Volcontract> volcontractcollection = dbContextCommon.DatabaseCommon.GetCollection<Volcontract>("Contracts");
            var filter = Builders<Volcontract>.Filter.Eq("_id", id);
            Volcontract returnVolContract = volcontractcollection.Find(filter).FirstOrDefault();
            return returnVolContract;
        }

        internal List<Volcontract> GetListOfVolunteersContracts()
        {
            IMongoCollection<Volcontract> volcontractcollection = dbContextCommon.DatabaseCommon.GetCollection<Volcontract>("Contracts");
            List<Volcontract> contracts = volcontractcollection.AsQueryable().ToList();
            return contracts;
        }

        internal void UpdateVolunteerContract(FilterDefinition<Volcontract> filter, UpdateDefinition<Volcontract> contract_toupdate)
        {
            IMongoCollection<Volcontract> volcontractcollection = dbContextCommon.DatabaseCommon.GetCollection<Volcontract>("Contracts");
            volcontractcollection.UpdateOne(filter, contract_toupdate);
        }

        internal void UpdateVolunteerContract(Volcontract contractupdate, string id)
        {
            IMongoCollection<Volcontract> volcontractcollection = dbContextCommon.DatabaseCommon.GetCollection<Volcontract>("Contracts");
            var filter = Builders<Volcontract>.Filter.Eq("_id", id);
            contractupdate._id = id;
            volcontractcollection.FindOneAndReplace(filter, contractupdate);
        }
    }
}
