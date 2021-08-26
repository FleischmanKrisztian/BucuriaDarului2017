using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.SponsorGateways;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.SponsorGateways
{
    public class SponsorContractExpirationGateway : IListDisplaySponsorContractsGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public List<Sponsor> GetListSponsorContracts()
        {
            List<Sponsor> sponsors = ListSponsorsGateway.GetListOfSponsors();
            return sponsors;
        }
    }
}