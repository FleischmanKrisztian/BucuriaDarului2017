﻿using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.EventGateways;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BucuriaDarului.Contexts.EventContexts
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
            var emptyDatabase = false;
            var events = dataGateway.GetListOfEvents();
            if (events.Count == 0)
                emptyDatabase = true;

            events = GetEventsAfterFilters(events, request.FilterData);

            var eventsAfterFiltering = events.Count();

            var stringOfIDs = GetStringOfIds(events);

            events = GetEventsAfterPaging(events, request.PagingData);

            return new EventsMainDisplayIndexResponse(events, request.FilterData, request.PagingData, emptyDatabase, eventsAfterFiltering, stringOfIDs, Constants.EVENT_SESSION);
        }

        private List<Event> GetEventsAfterFilters(List<Event> events, FilterData filterData)
        {
            events = events.Where(x => x.NameOfEvent.Contains(filterData.NameOfEvent, StringComparison.InvariantCultureIgnoreCase)).ToList();
            events = events.Where(x => x.PlaceOfEvent.Contains(filterData.PlaceOfEvent, StringComparison.InvariantCultureIgnoreCase)).ToList();
            events = events.Where(x => x.TypeOfActivities.Contains(filterData.TypeOfActivities, StringComparison.InvariantCultureIgnoreCase)).ToList();
            events = events.Where(x => x.TypeOfEvent.Contains(filterData.TypeOfEvent, StringComparison.InvariantCultureIgnoreCase)).ToList();
            events = events.Where(x => x.AllocatedVolunteers.Contains(filterData.AllocatedVolunteers, StringComparison.InvariantCultureIgnoreCase)).ToList();
            events = events.Where(x => x.AllocatedSponsors.Contains(filterData.AllocatedSponsors, StringComparison.InvariantCultureIgnoreCase)).ToList();

            if (DateInputReceived(filterData.LowerDate))
            {
                events = events.Where(x => x.DateOfEvent > filterData.LowerDate).ToList();
            }
            if (DateInputReceived(filterData.UpperDate))
            {
                events = events.Where(x => x.DateOfEvent <= filterData.UpperDate).ToList();
            }
            return events;
        }

        private string GetStringOfIds(List<Event> events)
        {
            var stringOfIDs = "eventCSV";
            foreach (Event eve in events)
            {
                stringOfIDs = stringOfIDs + "," + eve.Id;
            }
            return stringOfIDs;
        }

        private List<Event> GetEventsAfterPaging(List<Event> events, PagingData pagingData)
        {
            events = events.AsQueryable().Skip((pagingData.CurrentPage - 1) * pagingData.NrOfDocumentsPerPage).ToList();
            events = events.AsQueryable().Take(pagingData.NrOfDocumentsPerPage).ToList();
            return events;
        }

        private static bool DateInputReceived(DateTime date)
        {
            var comparisonDate = new DateTime(0003, 1, 1);
            return date > comparisonDate;
        }
    }

    public class EventsMainDisplayIndexRequest
    {
        public FilterData FilterData { get; set; }

        public PagingData PagingData { get; set; }

        public EventsMainDisplayIndexRequest(string searching, int page, int nrOfDocs, string searchingPlace, string searchingActivity, string searchingType, string searchingVolunteers, string searchingSponsor, DateTime lowerDate, DateTime upperDate)
        {
            var filterData = new FilterData();
            var pagingData = new PagingData();

            filterData.NameOfEvent = searching ?? "";
            filterData.PlaceOfEvent = searchingPlace ?? "";
            filterData.TypeOfActivities = searchingActivity ?? "";
            filterData.TypeOfEvent = searchingType ?? "";
            filterData.AllocatedVolunteers = searchingVolunteers ?? "";
            filterData.AllocatedSponsors = searchingSponsor ?? "";
            filterData.LowerDate = lowerDate;
            filterData.UpperDate = upperDate;

            pagingData.CurrentPage = GetCurrentPage(page);
            pagingData.NrOfDocumentsPerPage = nrOfDocs;

            FilterData = filterData;
            PagingData = pagingData;
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

        public bool EmptyDatabase { get; set; }

        public int TotalEvents { get; set; }

        public string StringOfIDs { get; set; }

        public string DictionaryKey { get; set; }

        public EventsMainDisplayIndexResponse(List<Event> events, FilterData filterData, PagingData pagingData, bool emptyDatabase, int eventsAfterFiltering, string stringOfIDs, string dictionaryKey)
        {
            Events = events;
            FilterData = filterData;
            PagingData = pagingData;
            TotalEvents = eventsAfterFiltering;
            EmptyDatabase = emptyDatabase;
            StringOfIDs = stringOfIDs;
            DictionaryKey = dictionaryKey;
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

        public string TypeOfActivities { get; set; }

        public string TypeOfEvent { get; set; }

        public string AllocatedVolunteers { get; set; }

        public string AllocatedSponsors { get; set; }

        public DateTime LowerDate { get; set; }

        public DateTime UpperDate { get; set; }
    }
}