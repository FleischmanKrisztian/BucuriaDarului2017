using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupDatabaseApp.DatabaseManagement
{
    class MongoDBContextCommon
    {
       
       public IMongoDatabase DatabaseCommon;

        public MongoDBContextCommon()
        {
            var clientSettings = new MongoClientSettings
            {
                Server = new MongoServerAddress(Environment.GetEnvironmentVariable(Common.MongoConstants.SERVER_NAME_COMMON), Int32.Parse(Environment.GetEnvironmentVariable(Common.MongoConstants.SERVER_PORT_COMMON))),
                ClusterConfigurator = builder =>
                {
                    builder.ConfigureCluster(settings => settings.With(serverSelectionTimeout: TimeSpan.FromSeconds(2)));
                }
            };
            var client = new MongoClient(clientSettings);
            DatabaseCommon = client.GetDatabase(Environment.GetEnvironmentVariable(Common.MongoConstants.DATABASE_NAME_COMMON));
        }
    }
}
