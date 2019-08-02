using MongoDB.Driver;
using System.Configuration;
using System;

namespace Finalaplication.App_Start
{
    public class MongoDBContextoffline
    {
        public IMongoDatabase database;

        public MongoDBContextoffline()
        {

            var client = new MongoClient();
            database = client.GetDatabase("VolMongo");
        }
    }
}
