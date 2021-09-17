using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.VolunteerGateways
{
    public interface IVolunteerDeleteGateways
    {
        public void UpdateVolunteer(string volunteerId, Volunteer volunteer);

        public Volunteer GetVolunteer(string id);

        public void Delete(string volunteerId);

        void DeleteVolunteerContracts(string id);

        void DeleteAdditionalContracts(string id);

        void UpdateEvent(string eventId, Event @event);

        List<Event> GetEvents();
    }
}