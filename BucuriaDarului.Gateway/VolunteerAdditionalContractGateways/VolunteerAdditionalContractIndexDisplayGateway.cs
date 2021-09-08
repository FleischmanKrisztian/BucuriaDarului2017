using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using BucuriaDarului.Gateway.VolunteerGateways;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.VolunteerContractGateways
{
    public class VolunteerAdditionalContractIndexDisplayGateway : IVolunteerAdditionalContractMainDisplayGateway
    {
        public VolunteerContract GetContract(string id)
        {
            return SingleVolunteerContractReturnerGateway.GetVolunteerContract(id);
        }

        public List<AdditionalContractVolunteer> GetListAdditionalContracts()
        {
            var contracts = ListVolunteerAdditionalContractGateway.GetListAdditionalContracts();
            return contracts;
        }

        public Volunteer GetVolunteer(string idOfVolunteer)
        {
            return SingleVolunteerReturnerGateway.ReturnVolunteer(idOfVolunteer);
        }
    }
}