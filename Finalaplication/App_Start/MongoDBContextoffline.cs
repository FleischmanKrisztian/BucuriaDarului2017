using MongoDB.Driver;
using System.Configuration;
using System;

namespace Finalaplication.App_Start
{
    public class MongoDBContextoffline
    {
        public IMongoDatabase databaseoffline;

        public MongoDBContextoffline()
        {
            var clientoffline = new MongoClient();
            databaseoffline = clientoffline.GetDatabase("BucuriaDaruluiOffline");
        }
    }
}
