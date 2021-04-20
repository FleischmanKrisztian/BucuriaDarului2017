using MongoDB.Driver;
using System;

namespace Finalaplication.App_Start
{
    public class MongoDBContextLocal
    {
        public IMongoDatabase DatabaseLocal;

        public MongoDBContextLocal()
        {
            var clientSettings = new MongoClientSettings
            {
                Server = new MongoServerAddress(Environment.GetEnvironmentVariable(Common.VolMongoConstants.SERVER_NAME_LOCAL), Int32.Parse(Environment.GetEnvironmentVariable(Common.VolMongoConstants.SERVER_PORT_LOCAL))),
                ClusterConfigurator = builder =>
                {
                    builder.ConfigureCluster(settings => settings.With(serverSelectionTimeout: TimeSpan.FromSeconds(2)));
                }
            };
            var client = new MongoClient(clientSettings);
            DatabaseLocal = client.GetDatabase(Environment.GetEnvironmentVariable(Common.VolMongoConstants.DATABASE_NAME_LOCAL));
        }
    }
}