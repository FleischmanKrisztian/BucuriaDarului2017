using BucuriaDarului.Gateway.SettingsGateways;

namespace BucuriaDarului.Contexts.SettingsContexts
{
    public class FirstStartupContext
    {
        public FirstStartupResponse Execute()
        {
            var setting = SingleSettingReturnerGateway.GetSettingItem();
            var response = new FirstStartupResponse(setting.Lang, setting.Quantity, setting.NumberOfDaysBeforBirthday, setting.NumberOfDaysBeforeExpiration);
            return response;
        }

        public class FirstStartupResponse
        {
            public string Language { get; set; }
            public int NumberOfItemsPerPage { get; set; }
            public int NumberOfDaysBeforBirthday { get; set; }
            public int NumberOfDaysBeforeExpiration { get; set; }

            public FirstStartupResponse(string language, int numberOfItemsPerPage, int numberOfDaysBeforBirthday, int numberOfDaysBeforeExpiration)
            {
                Language = language;
                NumberOfItemsPerPage = numberOfItemsPerPage;
                NumberOfDaysBeforBirthday = numberOfDaysBeforBirthday;
                NumberOfDaysBeforeExpiration = numberOfDaysBeforeExpiration;
            }
        }
    }
}