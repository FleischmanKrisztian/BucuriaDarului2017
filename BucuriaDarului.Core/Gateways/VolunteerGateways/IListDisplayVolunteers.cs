using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways.VolunteerGateways
{
    public interface IListDisplayVolunteers
    {
        List<Volunteer> GetListOfVolunteers();
    }
}
