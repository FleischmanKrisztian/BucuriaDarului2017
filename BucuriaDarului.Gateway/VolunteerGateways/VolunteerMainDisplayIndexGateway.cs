using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.VolunteerGateways
{
    public class VolunteerMainDisplayIndexGateway : IVolunteerMainDisplayIndexGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public List<Volunteer> GetListOfVolunteers()
        {
            return ListVolunteersGateway.GetListOfVolunteers();
        }
    }
}