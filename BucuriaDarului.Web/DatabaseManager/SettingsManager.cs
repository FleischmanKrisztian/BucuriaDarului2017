using System;
using BucuriaDarului.Web.Models;
using MongoDB.Driver;

namespace BucuriaDarului.Web.DatabaseManager
{
    public class SettingsManager
    {
        private MongoDBContext dbContext = new MongoDBContext(
           Environment.GetEnvironmentVariable(Common.Constants.SERVER_NAME_LOCAL),
           int.Parse(Environment.GetEnvironmentVariable(Common.Constants.SERVER_PORT_LOCAL)),
           Environment.GetEnvironmentVariable(Common.Constants.DATABASE_NAME_LOCAL));

        internal Settings GetSettingsItem()
        {
            IMongoCollection<Settings> settingcollection = dbContext.Database.GetCollection<Settings>("Settings");
            Settings set = settingcollection.AsQueryable<Settings>().SingleOrDefault();

            return set;
        }

        internal void AddSettingsToDB(Settings settings)
        {
            IMongoCollection<Settings> settingcollection = dbContext.Database.GetCollection<Settings>("Settings");
            settings.Id = Guid.NewGuid().ToString();
            settingcollection.InsertOne(settings);
        }

        internal void UpdateSettings(Settings settingtoupdate)
        {
            IMongoCollection<Settings> settingcollection = dbContext.Database.GetCollection<Settings>("Settings");
            var filter = Builders<Settings>.Filter.Eq("Id", settingtoupdate.Id);
            settingcollection.FindOneAndReplace(filter, settingtoupdate);
        }
    }
}