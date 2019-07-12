using MongoDB.Driver;
using System.Configuration;
using System;

namespace Finalaplication.App_Start
{
    public class MongoDBContext
    {
        public IMongoDatabase database;

        public MongoDBContext()
        {
            Environment.SetEnvironmentVariable("mongoserver", "mongodb+srv://Krisztian:rock44ever@siemens-application-mtrya.mongodb.net/test?retryWrites=true&w=majority", EnvironmentVariableTarget.Machine);
            try
            {
                var mongoClient = new MongoClient("mongodb+srv://Krisztian:rock44ever@siemens-application-mtrya.mongodb.net/test?retryWrites=true&w=majority");
                database = mongoClient.GetDatabase("VolMongo");
            }
            catch
            {
                var client = new MongoClient("");
                database = client.GetDatabase("Incercareax");
            }   
        }
    }
}