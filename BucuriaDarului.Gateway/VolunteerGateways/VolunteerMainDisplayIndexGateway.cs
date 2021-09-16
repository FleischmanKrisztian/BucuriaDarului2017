using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
using BucuriaDarului.Gateway.VolunteerContractGateways;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.VolunteerGateways
{
    public class VolunteerMainDisplayIndexGateway : IVolunteerMainDisplayIndexGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public List<AdditionalContractVolunteer> GetAdditionalContractList()
        {
            return ListVolunteerAdditionalContractGateway.GetListAdditionalContracts();
        }

        public List<VolunteerContract> GetContractList()
        {
            return ListVolunteerContractGateway.GetListVolunteerContracts();
        }

        public List<Volunteer> GetListOfVolunteers()
        {
            return ListVolunteersGateway.GetListOfVolunteers();
        }
    }
}