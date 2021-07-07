using System;
using System.Collections.Generic;
using System.Linq;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using BucuriaDarului.Core.Gateways.EventGateways;

namespace BucuriaDarului.Contexts.EventContexts
{
    public class EventVolunteerAllocationDisplayContext
    {
        private readonly IEventVolunteerAllocationDisplayGateway dataGateway;

        public EventVolunteerAllocationDisplayContext(IEventVolunteerAllocationDisplayGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public EventsVolunteerAllocationDisplayResponse Execute(EventsVolunteerAllocationDisplayRequest request)
        {
            var @event = dataGateway.ReturnEvent(request.EventId);
            var volunteers = dataGateway.GetListOfVolunteers();
            var totalVolunteers = volunteers.Count();
            var volunteersIdString = GetStringOfIds(volunteers);
            volunteers = GetVolunteerAfterSorting(volunteers, request.FilterData.VolunteerActivity);
            volunteers = GetVolunteersAfterPaging(volunteers, request.PagingData.CurrentPage, request.PagingData.NrOfDocumentsPerPage);
            volunteers = GetVolunteersAfterSearching(volunteers, request.FilterData.NameOfVolunteer);


            return new EventsVolunteerAllocationDisplayResponse(@event, volunteers, totalVolunteers, volunteersIdString, request.PagingData, request.FilterData, request.Messages);
        }
 

        private List<Volunteer> GetVolunteersAfterPaging(List<Volunteer> volunteers, int page, int nrOfDocs)
        {
            volunteers = volunteers.AsQueryable().Skip((page - 1) * nrOfDocs).ToList();
            volunteers = volunteers.AsQueryable().Take(nrOfDocs).ToList();
            return volunteers;
        }

        private List<Volunteer> GetVolunteersAfterSearching(List<Volunteer> volunteers, string searching)
        {
            if (searching != null)
            {
                volunteers = volunteers.Where(x => x.Fullname.Contains(searching, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            return volunteers;
        }

        private List<Volunteer> GetVolunteerAfterSorting(List<Volunteer> volunteers, string sortOrder)
        {
            if (sortOrder == "Active_desc")
            {
                volunteers = volunteers.OrderByDescending(s => s.InActivity).ToList();
            }

            return volunteers;
        }

        private string GetStringOfIds(List<Volunteer> volunteers)
        {
            var getStringOfIds = "volunteerCSV";
            foreach (var vol in volunteers)
            {
                getStringOfIds = getStringOfIds + "," + vol.Id;
            }
            return getStringOfIds;
        }
    }

    public class EventsVolunteerAllocationDisplayRequest
    {
        public string EventId { get; set; }
        public VolunteerAllocationPagingData PagingData { get; set; }
        public VolunteerAllocationFilterData FilterData { get; set; }
        public  string  Messages { get; set; }
        public EventsVolunteerAllocationDisplayRequest(string id, int page, int nrOfDocs, string searching, string messages)
        {
            this.EventId = id;
            var pagingData = new VolunteerAllocationPagingData();
            var filterData = new VolunteerAllocationFilterData();

            filterData.NameOfVolunteer = !string.IsNullOrEmpty(searching) ? searching : "";

            filterData.VolunteerActivity = "Active_desc";

            pagingData.CurrentPage = GetCurrentPage(page);
            pagingData.NrOfDocumentsPerPage = nrOfDocs;

            FilterData = filterData;
            PagingData = pagingData;
            Messages = messages;
        }

        private static int GetCurrentPage(int page)
        {
            if (page > 0)
                return page;
            else
            {
                page = 1;
                return page;
            }
        }
    }

    public class EventsVolunteerAllocationDisplayResponse
    {
        public Event Event { get; set; }
        public List<Volunteer> Volunteers { get; set; }
        public int TotalVolunteers { get; set; }
        public string AllocatedVolunteersIdString { get; set; }

        public VolunteerAllocationPagingData PagingData { get; set; }
        public VolunteerAllocationFilterData FilterData { get; set; }

        public  string Messages { get; set; }

        public EventsVolunteerAllocationDisplayResponse(Event @event, List<Volunteer> volunteers, int totalVolunteers, string allocateVolunteersIdString, VolunteerAllocationPagingData pagingData, VolunteerAllocationFilterData filterData, string messages)
        {
            Event = @event;
            Volunteers = volunteers;
            TotalVolunteers = totalVolunteers;
            AllocatedVolunteersIdString = allocateVolunteersIdString;
            PagingData = pagingData;
            FilterData = filterData;
            Messages = messages;
            
        }
    }

    public class VolunteerAllocationPagingData
    {
        public int CurrentPage { get; set; }

        public int NrOfDocumentsPerPage { get; set; }
    }

    public class VolunteerAllocationFilterData
    {
        public string NameOfVolunteer { get; set; }
        public string VolunteerActivity { get; set; }
    }
}