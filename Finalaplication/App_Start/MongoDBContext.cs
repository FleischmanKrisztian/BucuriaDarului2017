using MongoDB.Driver;
using System.Configuration;
using System;
using Finalaplication.Models;

namespace Finalaplication.App_Start
{
    public class MongoDBContext
    {
        public IMongoDatabase database;
        private MongoDBContextOffline dbcontextoffline;
        private IMongoCollection<Settings> settingcollection;

        public MongoDBContext(Settings set)
        {
            try
            {
                if (set.Env == "online")
                {
                    string EnvServerAddress = Environment.GetEnvironmentVariable("mongoserver");
                    string EnvDatabaseName = Environment.GetEnvironmentVariable("databasename");
                    var mongoClient = new MongoClient(EnvServerAddress);
                    database = mongoClient.GetDatabase(EnvDatabaseName);
                }
                else
                {
                    var clientSettings = new MongoClientSettings
                    {
                        Server = new MongoServerAddress("localhost", 27017),
                        ClusterConfigurator = builder =>
                        {
                            builder.ConfigureCluster(settings => settings.With(serverSelectionTimeout: TimeSpan.FromSeconds(2)));
                        }
                    };
                    var client = new MongoClient(clientSettings);
                    database = client.GetDatabase("BucuriaDaruluiOffline");
                }
            }
            catch
            {
                var client = new MongoClient();
                database = client.GetDatabase("BucuriaDaruluiOffline");

                set.Env = "offline";
                dbcontextoffline = new MongoDBContextOffline();
                settingcollection = dbcontextoffline.databaseoffline.GetCollection<Settings>("Settings");
                settingcollection.DeleteMany(x => x.Quantity >= 1);
                settingcollection.InsertOne(set);
            }
        }
    }
}