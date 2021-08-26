using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.VolunteerGateways
{
    public interface IVolunteerEditGateway
    {
        public void Edit(Volunteer volunteer);

        public List<ModifiedIDs> ReturnModificationList();

        public void AddVolunteerToModifiedList(string beforeEditingVolunteerString);

        public Volunteer ReturnVolunteer(string id);
    }
}