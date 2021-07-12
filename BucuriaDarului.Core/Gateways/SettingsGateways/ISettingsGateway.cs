using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways.SettingsGateways
{
    public interface ISettingsGateway
    {
        Settings GetSettings();
        void UpdateSettings(Settings settings);
    }
}
