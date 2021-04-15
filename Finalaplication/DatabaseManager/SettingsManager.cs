using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace Finalaplication.DatabaseManager
{
    public class SettingsManager
    {
        private MongoDBContextLocal dBContextLocal = new MongoDBContextLocal();

        internal Settings GetSettingsItem()
        {
            IMongoCollection<Settings> settingcollection = dBContextLocal.DatabaseLocal.GetCollection<Settings>("Settings");
            Settings set = settingcollection.AsQueryable<Settings>().SingleOrDefault();

            return set;
        }

        internal void AddSettingsToDB(Settings settings)
        {
            IMongoCollection<Settings> settingcollection = dBContextLocal.DatabaseLocal.GetCollection<Settings>("Settings");
            try
            {
                settings._id = Guid.NewGuid().ToString();
                settingcollection.InsertOne(settings);
            }
            catch
            {
                Console.WriteLine("There was an error adding Settings");
            }
        }

        internal void UpdateSettings(Settings settingtoupdate)
        {
            IMongoCollection<Settings> settingcollection = dBContextLocal.DatabaseLocal.GetCollection<Settings>("Settings");
            var filter = Builders<Settings>.Filter.Eq("_id", settingtoupdate._id);
            settingcollection.FindOneAndReplace(filter, settingtoupdate);
        }
    }
}