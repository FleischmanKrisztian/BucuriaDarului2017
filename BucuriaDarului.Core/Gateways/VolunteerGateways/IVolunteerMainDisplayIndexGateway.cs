using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways.VolunteerGateways
{
    public interface IVolunteerMainDisplayIndexGateway
    {
        List<Volunteer> GetListOfVolunteers();
    }
}
