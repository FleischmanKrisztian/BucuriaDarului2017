using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BucuriaDarului.Contexts
{
    public class EventsMainDisplayIndexContext
    {
        private readonly IEventsMainDisplayIndexGateway dataGateway;

        public EventsMainDisplayIndexContext(IEventsMainDisplayIndexGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public EventsMainDisplayIndexResponse Execute(EventsMainDisplayIndexRequest request)
        { 

            List<Event> Events = dataGateway.GetListOfEvents();

            Events = GetEventsAfterFilters(Events, request.FilterData);

            int EventsafterFiltering = Events.Count();

            string stringofids = GetStringOfIds(Events);

            Events = GetEventsAfterPaging(Events, request.PagingData);

            return new EventsMainDisplayIndexResponse(Events,request.FilterData,request.PagingData, EventsafterFiltering, stringofids);
        }

        internal static List<Event> GetEventsAfterFilters(List<Event> events, FilterData filterData)
        {
            if (filterData.Search != null)
            {
                events = events.Where(x => x.NameOfEvent.Contains(filterData.Search, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (filterData.SearchLocation != null)
            {
                List<Event> ev = events;
                foreach (var e in ev)
                {
                    if (e.PlaceOfEvent == null || e.PlaceOfEvent == "")
                    { e.PlaceOfEvent = "-"; }
                }
                events = ev.Where(x => x.PlaceOfEvent.Contains(filterData.SearchLocation, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (filterData.SearchActivity != null)
            {
                List<Event> ev = events;
                foreach (var e in ev)
                {
                    if (e.TypeOfActivities == null || e.TypeOfActivities == "")
                    { e.TypeOfActivities = "-"; }
                }
                events = ev.Where(x => x.TypeOfActivities.Contains(filterData.SearchActivity, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (filterData.SearchType != null)
            {
                List<Event> ev = events;
                foreach (var e in ev)
                {
                    if (e.TypeOfEvent == null || e.TypeOfEvent == "")
                    { e.TypeOfEvent = "-"; }
                }
                events = ev.Where(x => x.TypeOfEvent.Contains(filterData.SearchType, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (filterData.SearchVolunteers != null)
            {
                List<Event> ev = events;
                foreach (var e in ev)
                {
                    if (e.AllocatedVolunteers == null || e.AllocatedVolunteers == "")
                    { e.AllocatedVolunteers = "-"; }
                }
                events = ev.Where(x => x.AllocatedVolunteers.Contains(filterData.SearchVolunteers, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (filterData.SearchSponsors != null)
            {
                List<Event> ev = events;
                foreach (var e in ev)
                {
                    if (e.AllocatedSponsors == null || e.AllocatedSponsors == "")
                    { e.AllocatedSponsors = "-"; }
                }
                events = ev.Where(x => x.AllocatedSponsors.Contains(filterData.SearchSponsors, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (Dateinputreceived(filterData.LowerDate))
            {
                events = events.Where(x => x.DateOfEvent > filterData.LowerDate).ToList();
            }
            if (Dateinputreceived(filterData.UpperDate))
            {
                events = events.Where(x => x.DateOfEvent <= filterData.UpperDate).ToList();
            }
            return events;
        }

        public string GetStringOfIds(List<Event> events)
        {
            string stringofids = "eventCSV";
            foreach (Event eve in events)
            {
                stringofids = stringofids + "," + eve._id;
            }
            return stringofids;
        }

        internal static List<Event> GetEventsAfterPaging(List<Event> events, PagingData pagingData)
        {
            events = events.AsQueryable().Skip((pagingData.CurrentPage - 1) * pagingData.NrOfDocumentsPerPage).ToList();
            events = events.AsQueryable().Take(pagingData.NrOfDocumentsPerPage).ToList();
            return events;
        }

        private static bool Dateinputreceived(DateTime date)
        {
            DateTime comparisondate = new DateTime(0003, 1, 1);
            if (date > comparisondate)
                return true;
            else
                return false;
        }
    }

    public class EventsMainDisplayIndexRequest
    {

        public FilterData FilterData { get; set; }

        public PagingData PagingData { get; set; }

        public EventsMainDisplayIndexRequest(string searching, int page, int nrofdocs,  string searchingPlace, string searchingActivity, string searchingType, string searchingVolunteers, string? searchingSponsor, DateTime lowerdate, DateTime upperdate)
        {
            FilterData filterData = new FilterData();
            PagingData pagingData = new PagingData();

            filterData.Search = searching ?? "";
            filterData.SearchLocation = searchingPlace ?? "";
            filterData.SearchActivity = searchingActivity ?? "";
            filterData.SearchType = searchingType ?? "";
            filterData.SearchVolunteers = searchingVolunteers ?? "";
            filterData.SearchSponsors = searchingSponsor ?? "";
            filterData.LowerDate = lowerdate;
            filterData.UpperDate = upperdate;

            pagingData.CurrentPage = GetCurrentPage(page);
            pagingData.NrOfDocumentsPerPage = nrofdocs;

            this.FilterData = filterData;
            this.PagingData = pagingData;
        }

        internal static int GetCurrentPage(int page)
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


    public class EventsMainDisplayIndexResponse
    {
        public List<Event> Events { get; set; }

        public FilterData FilterData { get; set; }

        public PagingData PagingData { get; set; }

        public int TotalEvents { get; set; }

        public string StringOfIDs { get; set; }

        public EventsMainDisplayIndexResponse(List<Event> events, FilterData filterData, PagingData pagingData, int eventsAfterFiltering, string stringOfIDs)
        {
            Events = events;
            FilterData = filterData;
            PagingData = pagingData;
            TotalEvents = eventsAfterFiltering;
            StringOfIDs = stringOfIDs;
        }
    }

    public class PagingData
    {
        public int CurrentPage { get; set; }

        public int NrOfDocumentsPerPage { get; set; }
    }

    public class FilterData
    {
        public string Search { get; set; }

        public string SearchLocation { get; set; }

        public string SearchActivity { get; set; }

        public string SearchType { get; set; }

        public string SearchVolunteers { get; set; }

        public string SearchSponsors { get; set; }

        public DateTime LowerDate { get; set; }

        public DateTime UpperDate { get; set; }
    }
}