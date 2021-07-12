using BucuriaDarului.Core.Gateways.SettingsGateways;
using BucuriaDarului.Gateway.SettingsGateways;

namespace BucuriaDarului.Contexts.SettingsContexts
{
    public class FirstStartupContext
    {
        public FirstStartupResponse Execute()
        {
            var setting = SingleSettingReturnerGateway.GetSettingItem();
            var response = new FirstStartupResponse(setting.Lang, setting.Quantity);
            return response;
        }

        public class FirstStartupResponse
        {
            public string Language { get; set; }
            public int NumberOfItemsPerPage { get; set; }

            public FirstStartupResponse(string language, int numberOfItemsPerPage)
            {
                Language = language;
                NumberOfItemsPerPage = numberOfItemsPerPage;
            }
        }
    }
}