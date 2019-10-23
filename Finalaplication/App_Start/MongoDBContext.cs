using Finalaplication.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace Finalaplication.App_Start
{
    public class MongoDBContext
    {
        public IMongoDatabase database;
        private MongoDBContextOffline dbcontextoffline;
        private IMongoCollection<Settings> settingcollection;

        /// <summary>
        /// Creates a new Mongo client and returns the conection.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="dbName"></param>
        /// <param name="portNum"></param>
        /// <returns></returns>
        private IMongoDatabase getDatabaseForAddressDbNameAndPort(string address, string dbName, int portNum)
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
        /// Creates a MongoDb Client and retrieves the Database.
        /// 
        /// </summary>
        /// <param name="envVarNameServer"></param>
        /// <param name="envVarDbName"></param>
        /// <param name="envVarNamePort"></param>
        /// <returns></returns>
        private IMongoDatabase getDatabaseForEnvironmentVars(
            string envVarNameServer,
            string envVarDbName,
            string envVarNamePort)
        {
            // Offline mode considered secondary
            string envServerAddress = Environment.GetEnvironmentVariable(envVarNameServer);
            string envServerPort = Environment.GetEnvironmentVariable(envVarNamePort);
            int numServerPort = 0;
            Int32.TryParse(envServerPort, out numServerPort);
            string envDatabaseName = Environment.GetEnvironmentVariable(envVarDbName);

            return getDatabaseForAddressDbNameAndPort(envServerAddress, envDatabaseName, numServerPort);
        }

        public MongoDBContext()
        {
            try
            {
                // TODO (Augustin Preda, 2019-10-23): is this still required (separate offline context?)
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

                bool useOnline = (set.Env == "online");
                try
                {
                    if (useOnline)
                    {
                        database = getDatabaseForEnvironmentVars(
                            VolMongoConstants.VOLMONGO_SERVER_NAME_MAIN,
                            VolMongoConstants.VOLMONGO_DATABASE_NAME_MAIN,
                            "");
                    }
                    else
                    {
                        // Offline mode considered secondary
                        database = getDatabaseForEnvironmentVars(
                            VolMongoConstants.VOLMONGO_SERVER_NAME_SECONDARY,
                            VolMongoConstants.VOLMONGO_DATABASE_NAME_SECONDARY,
                            VolMongoConstants.VOLMONGO_SERVER_PORT_SECONDARY);
                    }
                }
                catch (Exception e)
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
                // Write online/offline setting
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
