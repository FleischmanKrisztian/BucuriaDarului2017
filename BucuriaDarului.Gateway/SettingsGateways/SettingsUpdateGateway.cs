using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.SettingsGateways;
using MongoDB.Driver;

namespace BucuriaDarului.Gateway.SettingsGateways
{
    public class SettingsUpdateGateway : ISettingsUpdateGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public Settings GetSettingItem()
        {
            IMongoCollection<Settings> settingcollection = dbContext.Database.GetCollection<Settings>("Settings");
            Settings setting = settingcollection.AsQueryable<Settings>().SingleOrDefault();

            return setting;
        }

        public void UpdateSettings(Settings settings)
        {
            IMongoCollection<Settings> settingcollection = dbContext.Database.GetCollection<Settings>("Settings");
            var filter = Builders<Settings>.Filter.Eq("Id", settings.Id);
            settingcollection.FindOneAndReplace(filter, settings);
        }
    }
}