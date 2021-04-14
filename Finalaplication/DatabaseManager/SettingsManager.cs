using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Driver;

namespace Finalaplication.DatabaseLocalManager
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
    }
}