using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.VolunteerContractGateways
{
    public class VolunteerContractExpirationGateway : IListDisplayVolunteerContractsGateway
    {
        public List<AdditionalContractVolunteer> GetListAdditionalContracts()
        {
            var additionalContracts = ListVolunteerAdditionalContractGateway.GetListAdditionalContracts();
            return additionalContracts;
        }

        public List<VolunteerContract> GetListVolunteerContracts()
        {
            var contracts = ListVolunteerContractGateway.GetListVolunteerContracts();
            return contracts;
        }
    }
}