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

            int eventsAfterFiltering = Events.Count();

            string stringOfIDs = GetStringOfIds(Events);

            Events = GetEventsAfterPaging(Events, request.PagingData);

            return new EventsMainDisplayIndexResponse(Events, request.FilterData, request.PagingData, eventsAfterFiltering, stringOfIDs);
        }

        private List<Event> GetEventsAfterFilters(List<Event> events, FilterData filterData)
        {
            events = events.Where(x => x.NameOfEvent.Contains(filterData.NameOfEvent, StringComparison.InvariantCultureIgnoreCase)).ToList();

            List<Event> auxiliaryEvents = events;
            foreach (var e in auxiliaryEvents)
            {
                if (e.PlaceOfEvent == null)
                    e.PlaceOfEvent = "";
            }
            try
            {
            events = events.Where(x => x.PlaceOfEvent.Contains(filterData.PlaceOfEvent, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            catch
            {
                //throw new ArgumentNullException();
            }


            //List<Event> ev = events;
            //foreach (var e in ev)
            //{
            //    if (e.TypeOfActivities == null || e.TypeOfActivities == "")
            //    { e.TypeOfActivities = "-"; }
            //}
            events = events.Where(x => x.TypeOfActivities.Contains(filterData.TypeOfActivites, StringComparison.InvariantCultureIgnoreCase)).ToList();

            //auxiliaryEvents = events;
            //foreach (var e in auxiliaryEvents)
            //{
            //    if (e.TypeOfEvent == null || e.TypeOfEvent == "")
            //    { e.TypeOfEvent = "-"; }
            //}
            events = events.Where(x => x.TypeOfEvent.Contains(filterData.TypeOfEvent, StringComparison.InvariantCultureIgnoreCase)).ToList();
            //auxiliaryEvents = events;
            //foreach (var e in auxiliaryEvents)
            //{
            //    if (e.AllocatedVolunteers == null || e.AllocatedVolunteers == "")
            //    { e.AllocatedVolunteers = "-"; }
            //}
            events = events.Where(x => x.AllocatedVolunteers.Contains(filterData.AllocatedVolunteers, StringComparison.InvariantCultureIgnoreCase)).ToList();

            //auxiliaryEvents = events;
            //foreach (var e in auxiliaryEvents)
            //{
            //    if (e.AllocatedSponsors == null || e.AllocatedSponsors == "")
            //    { e.AllocatedSponsors = "-"; }
            //}
            events = events.Where(x => x.AllocatedSponsors.Contains(filterData.AllocatedSponsors, StringComparison.InvariantCultureIgnoreCase)).ToList();

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

        private string GetStringOfIds(List<Event> events)
        {
            string stringofids = "eventCSV";
            foreach (Event eve in events)
            {
                stringofids = stringofids + "," + eve._id;
            }
            return stringofids;
        }

        private List<Event> GetEventsAfterPaging(List<Event> events, PagingData pagingData)
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

        public EventsMainDisplayIndexRequest(string searching, int page, int nrofdocs, string searchingPlace, string searchingActivity, string searchingType, string searchingVolunteers, string? searchingSponsor, DateTime lowerdate, DateTime upperdate)
        {
            FilterData filterData = new FilterData();
            PagingData pagingData = new PagingData();

            filterData.NameOfEvent = searching ?? "";
            filterData.PlaceOfEvent = searchingPlace ?? "";
            filterData.TypeOfActivites = searchingActivity ?? "";
            filterData.TypeOfEvent = searchingType ?? "";
            filterData.AllocatedVolunteers = searchingVolunteers ?? "";
            filterData.AllocatedSponsors = searchingSponsor ?? "";
            filterData.LowerDate = lowerdate;
            filterData.UpperDate = upperdate;

            pagingData.CurrentPage = GetCurrentPage(page);
            pagingData.NrOfDocumentsPerPage = nrofdocs;

            this.FilterData = filterData;
            this.PagingData = pagingData;
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
        public string NameOfEvent { get; set; }

        public string PlaceOfEvent { get; set; }

        public string TypeOfActivites { get; set; }

        public string TypeOfEvent { get; set; }

        public string AllocatedVolunteers { get; set; }

        public string AllocatedSponsors { get; set; }

        public DateTime LowerDate { get; set; }

        public DateTime UpperDate { get; set; }
    }
}