using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.VolunteerGateways
{
    public class VolunteerDownloadGateway : IVolunteerDownloadGateway
    {
        public List<Volunteer> GetListOfVolunteers()
        {
            return ListVolunteersGateway.GetListOfVolunteers();
        }
    }
}