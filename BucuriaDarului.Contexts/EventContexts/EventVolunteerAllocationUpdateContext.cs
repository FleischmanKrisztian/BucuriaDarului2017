using System.Collections.Generic;
using System.Linq;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;

namespace BucuriaDarului.Contexts.EventContexts
{
    public class EventVolunteerAllocationUpdateContext
    {
        private readonly IEventVolunteerAllocationUpdateGateway dataGateway;

        public EventVolunteerAllocationUpdateContext(IEventVolunteerAllocationUpdateGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public EventsVolunteerAllocationResponse Execute(EventsVolunteerAllocationRequest request)
        {
            var result = new EventsVolunteerAllocationResponse();

            var @event = dataGateway.ReturnEvent(request.EventId);

            var volunteers = dataGateway.GetListOfVolunteers();
            volunteers = GetVolunteersByIds(volunteers, request.VolunteerIds);
            var nameOfVolunteers = GetVolunteerNames(volunteers);
            var volunteerForAllocation = GetVolunteerNames(volunteers);
            @event.AllocatedVolunteers = volunteerForAllocation;
            @event.NumberAllocatedVolunteers = VolunteersAllocatedCounter(nameOfVolunteers);
            dataGateway.UpdateEvent(request.EventId, @event);

            if (@event.AllocatedVolunteers != volunteerForAllocation)
            {
                result.IsValid = false;
            }

            return result;
        }

        private int VolunteersAllocatedCounter(string allocatedVolunteers)
        {
            if (allocatedVolunteers != null)
            {
                var split = allocatedVolunteers.Split(" / ");
                return split.Count() - 1;
            }
            return 0;
        }

        private string GetVolunteerNames(List<Volunteer> volunteers)
        {
            var result = "";
            foreach (var volunteer in volunteers)
            {
                result = result + volunteer.Fullname + " / ";
            }
            return result;
        }

        // Can Probably simplify here
        private List<Volunteer> GetVolunteersByIds(List<Volunteer> volunteers, string[] ids)
        {
            var volunteerList = new List<Volunteer>();
            foreach (var id in ids)
            {
                var singleVolunteer = volunteers.First(x => x._id == id);
                volunteerList.Add(singleVolunteer);
            }
            return volunteerList;
        }
    }

    public class EventsVolunteerAllocationRequest
    {
        public string EventId { get; set; }
        public string[] VolunteerIds { get; set; }

        public EventsVolunteerAllocationRequest(string[] volunteerIds, string eventId)
        {
            this.EventId = eventId;
            this.VolunteerIds = volunteerIds;
        }
    }

    public class EventsVolunteerAllocationResponse
    {
        public bool IsValid { get; set; }
        public List<KeyValuePair<string, string>> Message { get; set; }

        public EventsVolunteerAllocationResponse()
        {
            IsValid = true;
            Message = new List<KeyValuePair<string, string>>();
        }
    }
}