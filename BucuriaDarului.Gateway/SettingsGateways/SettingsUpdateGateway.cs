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
            var settingCollection = dbContext.Database.GetCollection<Settings>("Settings");
            var setting = settingCollection.AsQueryable<Settings>().SingleOrDefault();

            return setting;
        }

        public void UpdateSettings(Settings settings)
        {
            var settingCollection = dbContext.Database.GetCollection<Settings>("Settings");
            var filter = Builders<Settings>.Filter.Eq("Id", settings.Id);
            settingCollection.FindOneAndReplace(filter, settings);
        }
    }
}