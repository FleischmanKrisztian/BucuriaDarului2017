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
            return SingleSettingReturnerGateway.GetSettingItem();
        }

        public void UpdateSettings(Settings settings)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var settingCollection = dbContext.Database.GetCollection<Settings>("Settings");
            var filter = Builders<Settings>.Filter.Eq("Id", settings.Id);
            settingCollection.FindOneAndReplace(filter, settings);
        }
    }
}