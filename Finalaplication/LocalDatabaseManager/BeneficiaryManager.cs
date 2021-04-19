using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Finalaplication.LocalDatabaseManager
{
    public class BeneficiaryManager
    {
        private MongoDBContextLocal dBContextLocal = new MongoDBContextLocal();
        private ModifiedDocumentManager modifiedDocumentManager = new ModifiedDocumentManager();

        internal void AddBeneficiaryToDB(Beneficiary beneficiary)
        {
            IMongoCollection<Beneficiary> beneficiarycollection = dBContextLocal.DatabaseLocal.GetCollection<Beneficiary>("Beneficiaries");
            modifiedDocumentManager.AddIDtoString(beneficiary._id);
            beneficiarycollection.InsertOne(beneficiary);
        }

        internal Beneficiary GetOneBeneficiary(string id)
        {
            IMongoCollection<Beneficiary> beneficiarycollection = dBContextLocal.DatabaseLocal.GetCollection<Beneficiary>("Beneficiaries");
            var filter = Builders<Beneficiary>.Filter.Eq("_id", id);
            Beneficiary beneficiary = beneficiarycollection.Find(filter).FirstOrDefault();
            return beneficiary;
        }

        internal List<Beneficiary> GetListOfBeneficiaries()
        {
            IMongoCollection<Beneficiary> beneficiarycollection = dBContextLocal.DatabaseLocal.GetCollection<Beneficiary>("Beneficiaries");
            List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable().ToList();
            return beneficiaries;
        }

        internal void UpdateABeneficiary(Beneficiary beneficiarytopdate, string id)
        {
            IMongoCollection<Beneficiary> Beneficiarycollection = dBContextLocal.DatabaseLocal.GetCollection<Beneficiary>("Beneficiaries");
            var filter = Builders<Beneficiary>.Filter.Eq("_id", id);
            beneficiarytopdate._id = id;
            modifiedDocumentManager.AddIDtoString(id);
            Beneficiarycollection.FindOneAndReplace(filter, beneficiarytopdate);
        }

        internal void DeleteBeneficiary(string id)
        {
            modifiedDocumentManager.AddIDtoDeletionString(id);
            IMongoCollection<Beneficiary> beneficiarycollection = dBContextLocal.DatabaseLocal.GetCollection<Beneficiary>("Beneficiaries");
            beneficiarycollection.DeleteOne(Builders<Beneficiary>.Filter.Eq("_id", id));
        }
    }
}