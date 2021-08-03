using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.EventGateways;
using System;
using System.Collections.Generic;
using System.Linq;

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
            volunteers = GetVolunteersAfterSearching(volunteers, request.FilterData.NameOfVolunteer);
            var totalVolunteers = volunteers.Count();
            volunteers = GetVolunteerAfterSorting(volunteers, request.FilterData.VolunteerActivity);
            volunteers = GetVolunteersAfterPaging(volunteers, request.PagingData.CurrentPage, request.PagingData.NrOfDocumentsPerPage);

            return new EventsVolunteerAllocationDisplayResponse(@event, volunteers, totalVolunteers, request.PagingData, request.FilterData, request.Messages);
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
            var Ids = "";
            foreach (var vol in volunteers)
            {
                if (Ids == "")
                    Ids = vol.Id;
                else
                    Ids = Ids + "," + vol.Id;
            }
            return Ids;
        }
    }

    public class EventsVolunteerAllocationDisplayRequest
    {
        public string EventId { get; set; }
        public VolunteerAllocationPagingData PagingData { get; set; }
        public VolunteerAllocationFilterData FilterData { get; set; }
        public string Messages { get; set; }

        public EventsVolunteerAllocationDisplayRequest(string id, int page, int nrOfDocs, string searching, string messages)
        {
            EventId = id;
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
        public string CurrentlyAllocatedVolunteers { get; set; }
        public string Query { get; set; }
        public VolunteerAllocationPagingData PagingData { get; set; }
        public VolunteerAllocationFilterData FilterData { get; set; }

        public string Messages { get; set; }

        public EventsVolunteerAllocationDisplayResponse(Event @event, List<Volunteer> volunteers, int totalVolunteers, VolunteerAllocationPagingData pagingData, VolunteerAllocationFilterData filterData, string messages)
        {
            Event = @event;
            Volunteers = volunteers;
            TotalVolunteers = totalVolunteers;
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