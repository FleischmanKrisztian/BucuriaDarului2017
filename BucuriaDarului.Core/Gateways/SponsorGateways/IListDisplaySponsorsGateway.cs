using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.SponsorGateways
{
    public interface IListDisplaySponsorsGateway
    {
        List<Sponsor> GetListOfSponsors();
    }
}