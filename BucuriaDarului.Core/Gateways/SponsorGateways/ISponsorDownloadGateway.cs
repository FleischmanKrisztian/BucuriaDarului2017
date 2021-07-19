using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways.SponsorGateways
{
   public interface ISponsorDownloadGateway
    {
        List<Sponsor> GetListOfSponsors();
    }
}
