using BucuriaDarului.Core.Gateways;
using MongoDB.Driver;
using System;

namespace BucuriaDarului.Gateway
{
    public class MongoDBGateway : IMongoDBGateway
    {
        public IMongoDatabase Database;

        public void ConnectToDB(string SERVER_NAME, int SERVER_PORT, string DATABASE_NAME)
        {
            var clientSettings = new MongoClientSettings
            {
                Server = new MongoServerAddress(SERVER_NAME, SERVER_PORT),
                ClusterConfigurator = builder =>
                {
                    builder.ConfigureCluster(settings => settings.With(serverSelectionTimeout: TimeSpan.FromSeconds(2)));
                }
            };
            var client = new MongoClient(clientSettings);
            Database = client.GetDatabase(DATABASE_NAME);
        }
    }
}