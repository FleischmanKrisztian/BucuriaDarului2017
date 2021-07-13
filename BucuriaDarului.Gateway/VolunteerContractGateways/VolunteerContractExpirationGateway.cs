using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.VolunteerContractGateways
{
    public class VolunteerContractExpirationGateway : IListDisplayVolunteerContractsGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public List<VolunteerContract> GetListVolunteerContracts()
        {
            List<VolunteerContract> contracts = ListVolunteerContractGateway.GetListVolunteerContracts();
            return contracts;
        }
    }
}