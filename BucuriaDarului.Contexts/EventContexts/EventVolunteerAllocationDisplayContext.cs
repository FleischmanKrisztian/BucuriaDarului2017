using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BucuriaDarului.Contexts
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
            Event event_ = dataGateway.GetEvent(request.EventId);
            List<Volunteer> volunteers = dataGateway.GetListOfVolunteers();
            int totalVolunteers = volunteers.Count();
            string volunteersIdString = GetStringOfIds(volunteers);
            volunteers = GetVolunteerAfterSorting(volunteers, request.FilterData.VolunteerActivity);
            volunteers = GetVolunteersAfterPaging(volunteers, request.PagingData.CurrentPage, request.PagingData.NrOfDocumentsPerPage);
            volunteers = GetVolunteersAfterSearching(volunteers, request.FilterData.NameOfVolunteer);
            //string AllocatedVolunteersIdString = GetAllocatedVolunteersString(event_, request.EventId);

            return new EventsVolunteerAllocationDisplayResponse(event_, volunteers, totalVolunteers, volunteersIdString, request.PagingData, request.FilterData, request.Messages);
        }
 

        private List<Volunteer> GetVolunteersAfterPaging(List<Volunteer> volunteers, int page, int nrofdocs)
        {
            volunteers = volunteers.AsQueryable().Skip((page - 1) * nrofdocs).ToList();
            volunteers = volunteers.AsQueryable().Take(nrofdocs).ToList();
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
            string stringofids = "volunteerCSV";
            foreach (Volunteer vol in volunteers)
            {
                stringofids = stringofids + "," + vol._id;
            }
            return stringofids;
        }
    }

    public class EventsVolunteerAllocationDisplayRequest
    {
        public string EventId { get; set; }
        public VolunteerAllocationPagingData PagingData { get; set; }
        public VolunteerAllocationFilterData FilterData { get; set; }
        public  string  Messages { get; set; }
        public EventsVolunteerAllocationDisplayRequest(string id, int page, int nrofdocs, string searching, string messages)
        {
            this.EventId = id;
            VolunteerAllocationPagingData pagingData = new VolunteerAllocationPagingData();
            VolunteerAllocationFilterData filterData = new VolunteerAllocationFilterData();

            if (searching != null && searching != "")
                filterData.NameOfVolunteer = searching;
            else
                filterData.NameOfVolunteer = "";

            filterData.VolunteerActivity = "Active_desc";

            pagingData.CurrentPage = GetCurrentPage(page);
            pagingData.NrOfDocumentsPerPage = nrofdocs;

            this.FilterData = filterData;
            this.PagingData = pagingData;
            this.Messages = messages;
        }

        private int GetCurrentPage(int page)
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

        public EventsVolunteerAllocationDisplayResponse(Event event_, List<Volunteer> volunteers, int totalVolunteers, string allocateVolunteersIdString, VolunteerAllocationPagingData pagingData, VolunteerAllocationFilterData filterData, string messages)
        {
            Event = event_;
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