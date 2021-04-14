using Finalaplication.Common;
using Finalaplication.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace Finalaplication.App_Start
{
    public class MongoDBContext
    {
        public IMongoDatabase database;
        private MongoDBContextLocal dBContextLocal;
        private IMongoCollection<Settings> settingcollection;
        public bool english = false;
        public int numberofdocsperpage;

        /// <summary>
        /// Creates a new Mongo client and returns the connection.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="dbName"></param>
        /// <param name="portNum"></param>
        /// <returns></returns>
        private IMongoDatabase GetDatabaseLocalForAddressDbNameAndPort(string address, string dbName, int portNum)
        {
            MongoClient mongoClient = null;

            if (portNum == 0)
            {
                MongoUrl onlineUrl = new MongoUrl(address);
                mongoClient = new MongoClient(onlineUrl);
            }
            else
            {
                var clientSettings = new MongoClientSettings
                {
                    Server = new MongoServerAddress(address),
                    ClusterConfigurator = builder =>
                    {
                        builder.ConfigureCluster(settings => settings.With(serverSelectionTimeout: TimeSpan.FromSeconds(2)));
                    }
                };
                mongoClient = new MongoClient(clientSettings);
            }

            return mongoClient.GetDatabase(dbName);
        }

        /// <summary>
        /// Creates a MongoDb Client and retrieves the DatabaseLocal.
        ///
        /// </summary>
        /// <param name="envVarNameServer"></param>
        /// <param name="envVarDbName"></param>
        /// <param name="envVarNamePort"></param>
        /// <returns></returns>
        private IMongoDatabase GetDatabaseLocalForEnvironmentVars(
            string envVarNameServer,
            string envVarDbName,
            string envVarNamePort)
        {
            string envServerAddress = Environment.GetEnvironmentVariable(envVarNameServer);
            string envServerPort = Environment.GetEnvironmentVariable(envVarNamePort);
            int numServerPort = Convert.ToInt32(envServerPort);
            string envDatabaseLocalName = Environment.GetEnvironmentVariable(envVarDbName);

            return GetDatabaseLocalForAddressDbNameAndPort(envServerAddress, envDatabaseLocalName, numServerPort);
        }

        public MongoDBContext()
        {
            dBContextLocal = new MongoDBContextLocal();
            settingcollection = dBContextLocal.DatabaseLocal.GetCollection<Settings>("Settings");
            var totalCount = settingcollection.CountDocuments(new BsonDocument());
            if (totalCount == 0)
            {
                Settings sett = new Settings
                {
                    Lang = "English",
                    Quantity = 15
                };
                settingcollection.InsertOne(sett);
            }
            Settings set = settingcollection.AsQueryable<Settings>().SingleOrDefault();
            numberofdocsperpage = set.Quantity;
            if (set.Lang == "English")
            {
                english = true;
            }
        }
    }
}