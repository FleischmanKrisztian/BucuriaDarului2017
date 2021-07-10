using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways.VolunteerContractGateways
{
    public interface IVolunteerContractCreateGateway
    {
        void Insert(VolunteerContract volunteerContract);
        Volunteer GetVolunteer(string idOfVolunteer);
    }
}
