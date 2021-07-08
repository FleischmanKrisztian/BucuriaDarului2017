using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.VolunteerGateways
{
    public interface IVolunteerMainDisplayIndexGateway
    {
        List<Volunteer> GetListOfVolunteers();
    }
}