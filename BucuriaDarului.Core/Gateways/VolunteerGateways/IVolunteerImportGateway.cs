using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.VolunteerGateways
{
    public interface IVolunteerImportGateway
    {
        void Insert(List<Volunteer> volunteers);

        public List<Volunteer> GetVolunteersList();

        void UpdateVolunteers(List<Volunteer> volunteers);
    }
}