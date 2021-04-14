using MongoDB.Driver;
using System;

namespace Finalaplication.App_Start
{
    public class MongoDBContextCommon
    {
        public IMongoDatabase DatabaseCommon;

        public MongoDBContextCommon()
        {
            var clientSettings = new MongoClientSettings
            {
                Server = new MongoServerAddress(Environment.GetEnvironmentVariable(Common.VolMongoConstants.SERVER_NAME_COMMON), Int32.Parse(Environment.GetEnvironmentVariable(Common.VolMongoConstants.SERVER_PORT_COMMON))),
                ClusterConfigurator = builder =>
                {
                    builder.ConfigureCluster(settings => settings.With(serverSelectionTimeout: TimeSpan.FromSeconds(2)));
                }
            };
            var client = new MongoClient(clientSettings);
            DatabaseCommon = client.GetDatabase(Environment.GetEnvironmentVariable(Common.VolMongoConstants.DATABASE_NAME_COMMON));
        }
    }
}