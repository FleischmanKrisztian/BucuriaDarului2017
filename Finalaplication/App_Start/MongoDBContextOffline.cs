using MongoDB.Driver;
using System;

namespace Finalaplication.App_Start
{
    public class MongoDBContextOffline
    {
        public IMongoDatabase databaseoffline;

        public MongoDBContextOffline()
        {
            var clientSettings = new MongoClientSettings
            {
                Server = new MongoServerAddress(Environment.GetEnvironmentVariable(Common.VolMongoConstants.SERVER_NAME_SECONDARY),Int32.Parse(Environment.GetEnvironmentVariable(Common.VolMongoConstants.SERVER_PORT_SECONDARY))),
                ClusterConfigurator = builder =>
                {
                    builder.ConfigureCluster(settings => settings.With(serverSelectionTimeout: TimeSpan.FromSeconds(2)));
                }
            };
            var client = new MongoClient(clientSettings);
            databaseoffline = client.GetDatabase(Environment.GetEnvironmentVariable(Common.VolMongoConstants.DATABASE_NAME_SECONDARY));
        }
    }
}
