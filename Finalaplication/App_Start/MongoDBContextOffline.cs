using MongoDB.Driver;
using System.Configuration;
using System;

namespace Finalaplication.App_Start
{
    public class MongoDBContextOffline
    {
        public IMongoDatabase databaseoffline;

        public MongoDBContextOffline()
        {
            var client = new MongoClient();
            databaseoffline = client.GetDatabase("BucuriaDaruluiOffline");
        }

    }
}