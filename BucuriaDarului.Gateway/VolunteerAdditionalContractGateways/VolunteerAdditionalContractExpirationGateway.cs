using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.VolunteerContractGateways
{
    public class VolunteerAdditionalContractExpirationGateway : IListDisplayVolunteerAdditionalContractsGateway
    {
        public List<AdditionalContractVolunteer> GetListAdditionalContracts()
        {
            var contracts = ListVolunteerAdditionalContractGateway.GetListAdditionalContracts();
            return contracts;
        }

       
    }
}