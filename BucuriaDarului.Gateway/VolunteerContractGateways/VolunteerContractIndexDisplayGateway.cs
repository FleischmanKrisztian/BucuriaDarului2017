using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using BucuriaDarului.Gateway.VolunteerGateways;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.VolunteerContractGateways
{
    public class VolunteerContractIndexDisplayGateway : IVolunteerContractMainDisplayGateway
    {
        //public VolunteerContract GetLastVolunteerContract()
        //{
        //    var lastContract = ListVolunteerContractGateway.GetLastVolunteerContracts();
        //    return lastContract;
        //}

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