using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.SponsorGateways
{
    public interface ISponsorDownloadGateway
    {
        List<Sponsor> GetListOfSponsors();
    }
}