using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.EventGateways;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var response = new EventsVolunteerAllocationResponse();
            var @event = dataGateway.ReturnEvent(request.EventId);
            var volunteers = dataGateway.GetListOfVolunteers();
            var volunteersToAdd = GetVolunteersByIds(volunteers, request.CheckedIds);
            var volunteersToRemove = GetVolunteersByIds(volunteers, request.AllIds);
            var volunteersForAllocation = @event.AllocatedVolunteers;
            volunteersForAllocation = RemoveUncheckedVolunteers(volunteersForAllocation, volunteersToRemove);
            volunteersForAllocation = CheckForDuplicate(volunteersForAllocation, GetVolunteerNames(volunteersToAdd));
            @event.AllocatedVolunteers = volunteersForAllocation;
            dataGateway.UpdateEvent(request.EventId, @event);

            return response;
        }

        private string RemoveUncheckedVolunteers(string allVolulunteers, List<Volunteer> volunteersToRemove)
        {
            foreach (var vol in volunteersToRemove)
            {
                allVolulunteers = allVolulunteers.Replace(", " + vol.Fullname, "");
                allVolulunteers = allVolulunteers.Replace(vol.Fullname, "");
            }
            return allVolulunteers;
        }

        private string CheckForDuplicate(string previouslyAllocatedVolunteers, List<string> names)
        {
            var AllVolunteers = previouslyAllocatedVolunteers;
            foreach( var name in names)
            {
                if(!previouslyAllocatedVolunteers.Contains(name))
                {
                    AllVolunteers += ", " + name;
                }
            }
            return AllVolunteers;
        }

        private int VolunteersAllocatedCounter(string allocatedVolunteers)
        {
            if (allocatedVolunteers != null)
            {
                var split = allocatedVolunteers.Split(", ");
                return split.Count();
            }
            return 0;
        }

        private List<string> GetVolunteerNames(List<Volunteer> volunteers)
        {
            var listOfNames = new List<string>();
            foreach (var volunteer in volunteers)
            {
                listOfNames.Add(volunteer.Fullname);
            }
            return listOfNames;
        }

        private List<Volunteer> GetVolunteersByIds(List<Volunteer> volunteers, string[] ids)
        {
            var volunteerList = new List<Volunteer>();
            foreach (var id in ids)
            {
                var singleVolunteer = volunteers.First(x => x.Id == id);
                volunteerList.Add(singleVolunteer);
            }
            return volunteerList;
        }
    }

    public class EventsVolunteerAllocationRequest
    {
        public string EventId { get; set; }

        public string[] CheckedIds { get; set; }

        public string[] AllIds { get; set; }

        public EventsVolunteerAllocationRequest(string[] checkedIds, string[] allIds, string eventId)
        {
            EventId = eventId;
            AllIds = allIds;
            CheckedIds = checkedIds;
        }
    }

    public class EventsVolunteerAllocationResponse
    {
        public bool IsValid { get; set; }

        public EventsVolunteerAllocationResponse()
        {
            IsValid = true;
        }
    }
}