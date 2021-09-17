using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.VolunteerContractGateways
{
    public interface IVolunteerContractMainDisplayGateway
    {
        List<VolunteerContract> GetListVolunteerContracts();

        Volunteer GetVolunteer(string id);
        //VolunteerContract GetLastVolunteerContract();
    }
}