using Finalaplication.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Configuration;

namespace Finalaplication.App_Start
{
    public class MongoDBContext
    {   
        public IMongoDatabase database;

        public MongoDBContext()
        {
            //BsonClassMap.RegisterClassMap<Beneficiary>(cm =>
            //{
            //    cm.AutoMap();
            //    cm.SetIgnoreExtraElements(true);


            //});

            //BsonClassMap.RegisterClassMap<Sponsor>(m =>
            //{
            //    m.AutoMap();
            //    m.SetIgnoreExtraElements(true);


            //});

            try
            {
                var mongoClient = new MongoClient("mongodb+srv://Krisztian:rock44ever@siemens-application-mtrya.mongodb.net/test?retryWrites=true&w=majority");
                database = mongoClient.GetDatabase("VolMongo");

              

            }
            catch
            {
                var client = new MongoClient();
                database = client.GetDatabase("Incercareax");
               
            }   
        }
    }
}