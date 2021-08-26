﻿using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.EventGateways;
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
            @event.NumberAllocatedVolunteers = VolunteersAllocatedCounter(volunteersForAllocation);
            var volunteersAllocatedIds = @event.AllocatedVolunteersId;
            volunteersAllocatedIds = RemoveUncheckedVolunteersIds(volunteersAllocatedIds, volunteersToRemove);
            var volunteersForAllocationIds = CheckForDuplicateIds(volunteersAllocatedIds, GetVolunteerIds(volunteersToAdd));
            @event.AllocatedVolunteersId = volunteersForAllocationIds;

            dataGateway.UpdateEvent(request.EventId, @event);

            return response;
        }

        private string RemoveUncheckedVolunteers(string allVolunteers, List<Volunteer> volunteersToRemove)
        {
            foreach (var vol in volunteersToRemove)
            {
                allVolunteers = allVolunteers.Replace(" + " + vol.Fullname, "");
                allVolunteers = allVolunteers.Replace(vol.Fullname, "");
            }
            return allVolunteers;
        }

        private string RemoveUncheckedVolunteersIds(string allVolunteersIds, List<Volunteer> volunteersToRemove)
        {
            if (allVolunteersIds != null && allVolunteersIds != "")
                foreach (var vol in volunteersToRemove)
                {
                    allVolunteersIds = allVolunteersIds.Replace(" + " + vol.Id, "");
                    allVolunteersIds = allVolunteersIds.Replace(vol.Id, "");
                }
            return allVolunteersIds;
        }

        private string CheckForDuplicate(string previouslyAllocatedVolunteers, List<string> names)
        {
            var allVolunteers = previouslyAllocatedVolunteers;
            foreach (var name in names)
            {
                if (!previouslyAllocatedVolunteers.Contains(name))
                {
                    allVolunteers += " + " + name;
                }
            }
            return allVolunteers;
        }

        private string CheckForDuplicateIds(string previouslyAllocatedVolunteersIds, List<string> ids)
        {
            var allVolunteers = previouslyAllocatedVolunteersIds;
            foreach (var id in ids)
            {
                if (previouslyAllocatedVolunteersIds != null && previouslyAllocatedVolunteersIds != "")
                {
                    if (!previouslyAllocatedVolunteersIds.Contains(id))
                    {
                        allVolunteers += " + " + ids;
                    }
                }
                else

                    allVolunteers += " + " + id;
            }
            return allVolunteers;
        }

        private int VolunteersAllocatedCounter(string allocatedVolunteers)
        {
            if (allocatedVolunteers != null)
            {
                var split = allocatedVolunteers.Split(" + ");
                return split.Count() - 1;
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

        private List<string> GetVolunteerIds(List<Volunteer> volunteers)
        {
            var listOfIds = new List<string>();
            foreach (var volunteer in volunteers)
            {
                listOfIds.Add(volunteer.Id);
            }
            return listOfIds;
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