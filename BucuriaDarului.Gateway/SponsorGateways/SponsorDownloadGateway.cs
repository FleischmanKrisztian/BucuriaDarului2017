using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.SponsorGateways;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.SponsorGateways
{
    public class SponsorDownloadGateway : ISponsorDownloadGateway
    {
        public List<Sponsor> GetListOfSponsors()
        {
            return ListSponsorsGateway.GetListOfSponsors();
        }
    }
}