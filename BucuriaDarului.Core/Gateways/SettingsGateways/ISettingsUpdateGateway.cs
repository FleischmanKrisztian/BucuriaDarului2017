using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways.SettingsGateways
{
    public interface ISettingsUpdateGateway
    {
        Settings GetSettingItem();
        void UpdateSettings(Settings settings);
    }
}
