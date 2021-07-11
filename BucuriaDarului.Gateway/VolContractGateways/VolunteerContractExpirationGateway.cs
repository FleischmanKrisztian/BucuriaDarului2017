using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.VolContractGateways
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