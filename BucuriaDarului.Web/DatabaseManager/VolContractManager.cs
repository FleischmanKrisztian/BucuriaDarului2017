using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Finalaplication.LocalDatabaseManager
{
    public class VolContractManager
    {
        private MongoDBContext dBContext;
        private ModifiedDocumentManager modifiedDocumentManager = new ModifiedDocumentManager();

        public VolContractManager(string SERVER_NAME, int SERVER_PORT, string DATABASE_NAME)
        {
            dBContext = new MongoDBContext(SERVER_NAME, SERVER_PORT, DATABASE_NAME);
        }

        internal void DeleteAVolunteersContracts(string id)
        {
            IMongoCollection<Volcontract> contractcollection = dBContext.Database.GetCollection<Volcontract>("Contracts");
            var contractlist = contractcollection.Find(zz => zz.OwnerID == id).ToList();
            for (int i = 0; i < contractlist.Count(); i++)
            {
                modifiedDocumentManager.AddIDtoDeletionString(contractlist[i]._id);
            }
            contractcollection.DeleteMany(Builders<Volcontract>.Filter.Eq("OwnerID", id));
        }

        internal void DeleteAVolContract(string id)
        {
            modifiedDocumentManager.AddIDtoDeletionString(id);
            IMongoCollection<Volcontract> contractcollection = dBContext.Database.GetCollection<Volcontract>("Contracts");
            contractcollection.DeleteOne(Builders<Volcontract>.Filter.Eq("_id", id));
        }

        internal void AddVolunteerContractToDB(Volcontract contract)
        {
            IMongoCollection<Volcontract> volcontractcollection = dBContext.Database.GetCollection<Volcontract>("Contracts");
            modifiedDocumentManager.AddIDtoString(contract._id);
            volcontractcollection.InsertOne(contract);
        }

        internal Volcontract GetVolunteerContract(string id)
        {
            IMongoCollection<Volcontract> volcontractcollection = dBContext.Database.GetCollection<Volcontract>("Contracts");
            var filter = Builders<Volcontract>.Filter.Eq("_id", id);
            Volcontract returnVolContract = volcontractcollection.Find(filter).FirstOrDefault();
            return returnVolContract;
        }

        internal List<Volcontract> GetListOfVolunteersContracts()
        {
            IMongoCollection<Volcontract> volcontractcollection = dBContext.Database.GetCollection<Volcontract>("Contracts");
            List<Volcontract> contracts = volcontractcollection.AsQueryable().ToList();
            return contracts;
        }

        internal void UpdateVolunteerContract(Volcontract contractupdate, string id)
        {
            IMongoCollection<Volcontract> volcontractcollection = dBContext.Database.GetCollection<Volcontract>("Contracts");
            var filter = Builders<Volcontract>.Filter.Eq("_id", id);
            contractupdate._id = id;
            modifiedDocumentManager.AddIDtoString(id);
            volcontractcollection.FindOneAndReplace(filter, contractupdate);
        }
    }
}