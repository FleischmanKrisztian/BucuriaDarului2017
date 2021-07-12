using System.Collections.Generic;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;

namespace BucuriaDarului.Gateway.VolunteerContractGateways
{
    public class VolunteerContractExpirationGateway : IListDisplayVolunterContractsGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public List<VolunteerContract> GetListVolunteerContracts()
        {

            List<VolunteerContract> contracts = ListVolunteerContractGateway.GetListVolunteerContracts();
            return contracts;
        }
    }
}