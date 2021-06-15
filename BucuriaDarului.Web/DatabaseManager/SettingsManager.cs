using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Driver;
using System;

namespace Finalaplication.DatabaseManager
{
    public class SettingsManager
    {
        private MongoDBContext dBContext = new MongoDBContext(
           Environment.GetEnvironmentVariable(Common.Constants.SERVER_NAME_LOCAL),
           int.Parse(Environment.GetEnvironmentVariable(Common.Constants.SERVER_PORT_LOCAL)),
           Environment.GetEnvironmentVariable(Common.Constants.DATABASE_NAME_LOCAL));

        internal Settings GetSettingsItem()
        {
            IMongoCollection<Settings> settingcollection = dBContext.Database.GetCollection<Settings>("Settings");
            Settings set = settingcollection.AsQueryable<Settings>().SingleOrDefault();

            return set;
        }

        internal void AddSettingsToDB(Settings settings)
        {
            IMongoCollection<Settings> settingcollection = dBContext.Database.GetCollection<Settings>("Settings");
            settings._id = Guid.NewGuid().ToString();
            settingcollection.InsertOne(settings);
        }

        internal void UpdateSettings(Settings settingtoupdate)
        {
            IMongoCollection<Settings> settingcollection = dBContext.Database.GetCollection<Settings>("Settings");
            var filter = Builders<Settings>.Filter.Eq("_id", settingtoupdate._id);
            settingcollection.FindOneAndReplace(filter, settingtoupdate);
        }
    }
}