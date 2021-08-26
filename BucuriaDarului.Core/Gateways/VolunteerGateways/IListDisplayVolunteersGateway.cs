using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.VolunteerGateways
{
    public interface IListDisplayVolunteersGateway
    {
        List<Volunteer> GetListOfVolunteers();
    }
}