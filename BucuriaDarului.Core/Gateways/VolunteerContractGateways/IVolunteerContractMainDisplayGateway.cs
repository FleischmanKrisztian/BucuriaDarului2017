using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways.VolunteerContractGateways
{
    public interface IVolunteerContractMainDisplayGateway
    {
        List<VolunteerContract> GetListVolunteerContracts();
        Volunteer GetVolunteer(string id);
    }
}
