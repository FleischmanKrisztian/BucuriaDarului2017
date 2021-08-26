using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.VolunteerGateways
{
    public interface IVolunteerDownloadGateway
    {
        List<Volunteer> GetListOfVolunteers();
    }
}