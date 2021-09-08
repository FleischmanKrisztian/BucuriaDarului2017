using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.VolunteerContractGateways
{
    public interface IVolunteerAdditionalContractMainDisplayGateway
    {
        List<AdditionalContractVolunteer> GetListAdditionalContracts();

        Volunteer GetVolunteer(string id);
        VolunteerContract GetContract(string id);
    }
}