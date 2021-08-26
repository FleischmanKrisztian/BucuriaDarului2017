using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.SponsorGateways
{
    public interface ISponsorsMainDisplayIndexGateway
    {
        List<Sponsor> GetListOfSponsors();
    }
}