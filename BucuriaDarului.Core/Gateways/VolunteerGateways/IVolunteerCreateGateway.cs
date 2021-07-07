using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways.VolunteerGateways
{
    public interface IVolunteerCreateGateway
    {
        void Insert(Volunteer volunteer);
    }
}
