using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.SettingsGateways;


namespace BucuriaDarului.Contexts.SettingsContexts
{
    public class SettingsUpdateContext
    {
        private readonly ISettingsUpdateGateway dataGateway;

        public SettingsUpdateContext(ISettingsUpdateGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public void Execute(string lang, int quantity)
        {
            Settings settting = dataGateway.GetSettingItem();
            settting.Quantity = quantity;
            settting.Lang = lang;
            dataGateway.UpdateSettings(settting);
        }
    }
}
