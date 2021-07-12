using System.Collections.Generic;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using BucuriaDarului.Gateway.VolunteerGateways;

namespace BucuriaDarului.Gateway.VolunteerContractGateways
{
    public class VolunteerContractIndexDisplayGateway : IVolunteerContractMainDisplayGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public List<VolunteerContract> GetListVolunteerContracts()
        {

            List<VolunteerContract> contracts = ListVolunteerContractGateway.GetListVolunteerContracts();
            return contracts;
        }

        public Volunteer GetVolunteer(string idOfVolunteer)
        {
            return SingleVolunteerReturnerGateway.ReturnVolunteer(idOfVolunteer);
        }
    }
}