using MongoDB.Driver;
using System.Configuration;
using System;
using Finalaplication.Models;
using MongoDB.Bson;

namespace Finalaplication.App_Start
{
    public class MongoDBContext
    {
        public IMongoDatabase database;
        private MongoDBContextOffline dbcontextoffline;
        private IMongoCollection<Settings> settingcollection;

        public MongoDBContext()
        {
            try
            {
                dbcontextoffline = new MongoDBContextOffline();
                settingcollection = dbcontextoffline.databaseoffline.GetCollection<Settings>("Settings");
                var totalCount = settingcollection.CountDocuments(new BsonDocument());

                if (totalCount == 0)
                {
                    Settings sett = new Settings();
                    sett.Env = "offline";
                    sett.Lang = "English";
                    sett.Quantity = 15;
                    settingcollection.InsertOne(sett);
                }

                Settings set = settingcollection.AsQueryable<Settings>().SingleOrDefault();

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
                    Settings sett = new Settings();
                    sett.settingID = set.settingID;
                    sett.Env = "offline";
                    sett.Lang = set.Lang;
                    sett.Quantity = set.Quantity;
                    settingcollection.ReplaceOne(y => y.Env.Contains("i"), sett);
                    var client = new MongoClient();
                    database = client.GetDatabase("BucuriaDaruluiOffline");
                }
            }
            catch
            {
                Settings sett = new Settings();
                try
                {
                    Settings set = settingcollection.AsQueryable().FirstOrDefault();
                    sett.settingID = set.settingID;
                    sett.Env = "offline";
                    sett.Lang = set.Lang;
                    sett.Quantity = set.Quantity;
                    settingcollection.ReplaceOne(y => y.Env.Contains("i"), sett);
                    var client = new MongoClient();
                    database = client.GetDatabase("BucuriaDaruluiOffline");
                }
                catch
                {

                }
            }
        }
    }
}