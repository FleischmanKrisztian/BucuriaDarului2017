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
            bool updateCompleted = false;
            string message = "";
            Event event_ = dataGateway.GetEvent(request.EventId);
            List<Volunteer> volunteers = dataGateway.GetListOfVolunteers();
            volunteers = GetVolunteersByIds(volunteers, request.VolunteerIds);
            string nameOfVolunteers = GetVolunteerNames(volunteers);
            string volunteerForAllocation = GetVolunteerNames(volunteers);
            event_.AllocatedVolunteers = volunteerForAllocation;
            event_.NumberAllocatedVolunteers = VolunteersAllocatedCounter(nameOfVolunteers);
            dataGateway.UpdateEvent(request.EventId, event_);
            if (event_.AllocatedVolunteers == volunteerForAllocation)
            { updateCompleted = true;
                message = "The event has been successfuly updated!";
            }
            else
            {
                updateCompleted = false;
                message = "Update failed!!!";
            }

            return new EventsVolunteerAllocationResponse(updateCompleted, message);
        }

        private int VolunteersAllocatedCounter(string AllocatedVolunteers)
        {
            if (AllocatedVolunteers != null)
            {
                string[] split = AllocatedVolunteers.Split(" / ");
                return split.Count() - 1;
            }
            return 0;
        }

        private string GetVolunteerNames(List<Volunteer> volunteers)
        {
            string volnames = "";
            for (int i = 0; i < volunteers.Count; i++)
            {
                var volunteer = volunteers[i];
                volnames = volnames + volunteer.Fullname + " / ";
            }
            return volnames;
        }

        private string GetAllocatedVolunteersString(Event event_, string id)
        {
            event_.AllocatedVolunteers += " / ";
            return event_.AllocatedVolunteers;
        }

        private List<Volunteer> GetVolunteersByIds(List<Volunteer> volunteers, string[] vols)
        {
            List<Volunteer> volunteerlist = new List<Volunteer>();
            for (int i = 0; i < vols.Length; i++)
            {
                Volunteer singlevolunteer = volunteers.Where(x => x._id == vols[i]).First();
                volunteerlist.Add(singlevolunteer);
            }
            return volunteerlist;
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
        public bool UpdateCompleted { get; set; }
        public string Message { get; set; }

        public EventsVolunteerAllocationResponse(bool updatedCompleted, string message)
        {
            this.UpdateCompleted = updatedCompleted;

            if (message != " " && message != null)
                this.Message = message;
            else
                this.Message = "Update failed!!!";
        }
    }
}