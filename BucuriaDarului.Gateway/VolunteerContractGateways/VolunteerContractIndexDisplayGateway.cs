using System.Collections.Generic;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using BucuriaDarului.Gateway.VolunteerGateways;

namespace BucuriaDarului.Gateway.VolunteerContractGateways
{
    public class VolunteerContractIndexDisplayGateway : IVolunteerContractMainDisplayGateway
    {
        public List<VolunteerContract> GetListVolunteerContracts()
        {
            var contracts = ListVolunteerContractGateway.GetListVolunteerContracts();
            return contracts;
        }

        public Volunteer GetVolunteer(string idOfVolunteer)
        {
            return SingleVolunteerReturnerGateway.ReturnVolunteer(idOfVolunteer);
        }
    }
}