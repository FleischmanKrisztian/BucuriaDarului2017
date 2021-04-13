using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Driver;

namespace Finalaplication.DatabaseManager
{
    public class SettingsManager
    {
        private MongoDBContext dbcontext = new MongoDBContext();

        internal Settings GetSettingsItem()
        {
            IMongoCollection<Settings> settingcollection = dbcontext.database.GetCollection<Settings>("Settings");
            Settings set = settingcollection.AsQueryable<Settings>().SingleOrDefault();

            return set;
        }

        internal void UpdateSettingsItem_Env( Settings settings)
        {
            IMongoCollection<Settings> settingcollection = dbcontext.database.GetCollection<Settings>("Settings");
            settingcollection.ReplaceOne(y => y.Env.Contains("i"), settings);

        }
    }
}