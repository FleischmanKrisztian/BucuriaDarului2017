using System.Collections.Generic;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.SponsorGateways;

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
