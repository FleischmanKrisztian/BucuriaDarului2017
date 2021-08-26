using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.EventGateways;
using BucuriaDarului.Gateway.VolunteerGateways;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.EventGateways
{
    public class EventVolunteerAllocationDataGateway : IEventVolunteerAllocationDisplayGateway
    {
        private readonly MongoDBGateway dbContext = new MongoDBGateway();

        public List<Volunteer> GetListOfVolunteers()
        {
            return ListVolunteersGateway.GetListOfVolunteers();
        }

        public Event ReturnEvent(string eventId)
        {
            return SingleEventReturnerGateway.ReturnEvent(eventId);
        }
    }
}