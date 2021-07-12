using System.Collections.Generic;
using System.Linq;
using BucuriaDarului.Core;
using MongoDB.Driver;

namespace BucuriaDarului.Web.DatabaseManager
{
    public class BeneficiaryManager
    {
        public MongoDBContext dbContext;
        private ModifiedDocumentManager modifiedDocumentManager = new ModifiedDocumentManager();

        public BeneficiaryManager(string SERVER_NAME, int SERVER_PORT, string DATABASE_NAME)
        {
            dbContext = new MongoDBContext(SERVER_NAME, SERVER_PORT, DATABASE_NAME);
        }

        internal void AddBeneficiaryToDB(Beneficiary beneficiary)
        {
            IMongoCollection<Beneficiary> beneficiarycollection = dbContext.Database.GetCollection<Beneficiary>("Beneficiaries");
            modifiedDocumentManager.AddIDtoString(beneficiary.Id);
            beneficiarycollection.InsertOne(beneficiary);
        }

        internal Beneficiary GetOneBeneficiary(string id)
        {
            IMongoCollection<Beneficiary> beneficiarycollection = dbContext.Database.GetCollection<Beneficiary>("Beneficiaries");
            var filter = Builders<Beneficiary>.Filter.Eq("Id", id);
            Beneficiary beneficiary = beneficiarycollection.Find(filter).FirstOrDefault();
            return beneficiary;
        }

        internal List<Beneficiary> GetListOfBeneficiaries()
        {
            IMongoCollection<Beneficiary> beneficiarycollection = dbContext.Database.GetCollection<Beneficiary>("Beneficiaries");
            List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable().ToList();
            return beneficiaries;
        }

        internal void UpdateABeneficiary(Beneficiary beneficiarytopdate, string id)
        {
            IMongoCollection<Beneficiary> Beneficiarycollection = dbContext.Database.GetCollection<Beneficiary>("Beneficiaries");
            var filter = Builders<Beneficiary>.Filter.Eq("Id", id);
            beneficiarytopdate.Id = id;
            modifiedDocumentManager.AddIDtoString(id);
            Beneficiarycollection.FindOneAndReplace(filter, beneficiarytopdate);
        }

        internal void DeleteBeneficiary(string id)
        {
            modifiedDocumentManager.AddIDtoDeletionString(id);
            IMongoCollection<Beneficiary> beneficiarycollection = dbContext.Database.GetCollection<Beneficiary>("Beneficiaries");
            beneficiarycollection.DeleteOne(Builders<Beneficiary>.Filter.Eq("Id", id));
        }
    }
}