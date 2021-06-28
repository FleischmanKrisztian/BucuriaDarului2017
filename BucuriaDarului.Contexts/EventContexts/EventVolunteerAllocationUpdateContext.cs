using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using System.Collections.Generic;
using System.Linq;

namespace BucuriaDarului.Contexts
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

            Event @event = dataGateway.ReturnEvent(request.EventId);

            List<Volunteer> volunteers = dataGateway.GetListOfVolunteers();
            volunteers = GetVolunteersByIds(volunteers, request.VolunteerIds);
            string nameOfVolunteers = GetVolunteerNames(volunteers);
            string volunteerForAllocation = GetVolunteerNames(volunteers);
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
                string[] split = allocatedVolunteers.Split(" / ");
                return split.Count() - 1;
            }
            return 0;
        }

        private string GetVolunteerNames(List<Volunteer> volunteers)
        {
            string result = "";
            for (int i = 0; i < volunteers.Count; i++)
            {
                var volunteer = volunteers[i];
                result = result + volunteer.Fullname + " / ";
            }
            return result;
        }


        private List<Volunteer> GetVolunteersByIds(List<Volunteer> volunteers, string[] ids)
        {
            List<Volunteer> volunteerList = new List<Volunteer>();
            for (int i = 0; i < ids.Length; i++)
            {
                Volunteer singlevolunteer = volunteers.Where(x => x._id == ids[i]).First();
                volunteerList.Add(singlevolunteer);
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