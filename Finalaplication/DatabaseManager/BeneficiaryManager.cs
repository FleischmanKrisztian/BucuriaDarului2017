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
    public class BeneficiaryManager
    {
        private MongoDBContext dbcontext = new MongoDBContext();

        internal void AddBeneficiaryToDB(Beneficiary beneficiary)
        {
            IMongoCollection<Beneficiary> beneficiarycollection = dbcontext.database.GetCollection<Beneficiary>("Beneficiaries");
            try
            {
                beneficiarycollection.InsertOne(beneficiary);
            }
            catch
            {
                Console.WriteLine("There was an error adding Volunteer!");
            }
        }

        internal Beneficiary GetOneBeneficiary(string id)
        {
            IMongoCollection<Beneficiary> beneficiarycollection = dbcontext.database.GetCollection<Beneficiary>("Beneficiaries");
            var filter = Builders<Beneficiary>.Filter.Eq("_id", ObjectId.Parse(id));
            Beneficiary volunteer = beneficiarycollection.Find(filter).FirstOrDefault();
            return volunteer;
        }
        internal List<Beneficiary> GetListOfBeneficiaries()
        {
            IMongoCollection<Beneficiary> beneficiarycollection = dbcontext.database.GetCollection<Beneficiary>("Beneficiaries");
            List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable().ToList();
            return beneficiaries;
        }

        internal void UpdateBeneficiary(FilterDefinition<Beneficiary> filter, UpdateDefinition<Beneficiary> beneficiarytoupdate)
        {
            IMongoCollection<Beneficiary> beneficiarycollection = dbcontext.database.GetCollection<Beneficiary>("Beneficiaries");
            beneficiarycollection.UpdateOne(filter, beneficiarytoupdate);
        }

        internal void DeleteBeneficiary(string id)
        {
            IMongoCollection<Beneficiary> beneficiarycollection = dbcontext.database.GetCollection<Beneficiary>("Beneficiaries");
            beneficiarycollection.DeleteOne(Builders<Beneficiary>.Filter.Eq("_id", ObjectId.Parse(id)));
        }

    }
}
