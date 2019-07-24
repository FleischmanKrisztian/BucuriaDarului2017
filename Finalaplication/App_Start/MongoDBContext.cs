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
            try
            {
                string EnvServerAddress = Environment.GetEnvironmentVariable("mongoserver");
                string EnvDatabaseName = Environment.GetEnvironmentVariable("databasename");
                var mongoClient = new MongoClient(EnvServerAddress);
                database = mongoClient.GetDatabase(EnvDatabaseName);
            }
            catch
            {
                var client = new MongoClient();
                database = client.GetDatabase("VolMongo");
            }
        }
    }
}