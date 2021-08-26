using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.EventGateways;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BucuriaDarului.Contexts.EventContexts
{
    public class EventSponsorAllocationDisplayContext
    {
        private readonly IEventSponsorAllocationDisplayGateway dataGateway;

        public EventSponsorAllocationDisplayContext(IEventSponsorAllocationDisplayGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public EventsSponsorAllocationDisplayResponse Execute(EventsSponsorsAllocationDisplayRequest request)
        {
            var @event = dataGateway.ReturnEvent(request.EventId);
            var sponsors = dataGateway.GetListOfSponsors();
            sponsors = GetSponsorsAfterSearching(sponsors, request.FilterData.NameOfSponsor);
            var totalSponsors = sponsors.Count();
            sponsors = GetSponsorsAfterPaging(sponsors, request.PagingData.CurrentPage, request.PagingData.NrOfDocumentsPerPage);

            return new EventsSponsorAllocationDisplayResponse(@event, sponsors, totalSponsors, request.PagingData, request.FilterData, request.Messages);
        }

        private List<Sponsor> GetSponsorsAfterSearching(List<Sponsor> sponsors, string searching)
        {
            if (searching != null)
            {
                sponsors = sponsors.Where(x => x.NameOfSponsor.Contains(searching, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            return sponsors;
        }

        private List<Sponsor> GetSponsorsAfterPaging(List<Sponsor> sponsors, int page, int nrOfDocs)
        {
            sponsors = sponsors.AsQueryable().Skip((page - 1) * nrOfDocs).ToList();
            sponsors = sponsors.AsQueryable().Take(nrOfDocs).ToList();
            return sponsors;
        }

        private string GetStringOfIds(List<Sponsor> sponsors)
        {
            var stringOfIds = "sponsorCSV";
            foreach (var sponsor in sponsors)
            {
                stringOfIds = stringOfIds + "," + sponsor.Id;
            }
            return stringOfIds;
        }
    }

    public class EventsSponsorsAllocationDisplayRequest
    {
        public string EventId { get; set; }
        public SponsorAllocationPagingData PagingData { get; set; }
        public SponsorAllocationFilterData FilterData { get; set; }

        public string Messages { get; set; }

        public EventsSponsorsAllocationDisplayRequest(string eventId, int page, int nrOfDocs, string searching, string messages)
        {
            this.EventId = eventId;
            var pagingData = new SponsorAllocationPagingData();
            var filterData = new SponsorAllocationFilterData();

            filterData.NameOfSponsor = !string.IsNullOrEmpty(searching) ? searching : "";
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
            page = 1;
            return page;
        }
    }

    public class EventsSponsorAllocationDisplayResponse
    {
        public Event Event { get; set; }
        public List<Sponsor> Sponsors { get; set; }
        public int TotalSponsors { get; set; }
        public string Query { get; set; }
        public SponsorAllocationPagingData PagingData { get; set; }
        public SponsorAllocationFilterData FilterData { get; set; }
        public string Messages { get; set; }

        public EventsSponsorAllocationDisplayResponse(Event @event, List<Sponsor> sponsors, int totalSponsors, SponsorAllocationPagingData pagingData, SponsorAllocationFilterData filterData, string messages)
        {
            Event = @event;
            Sponsors = sponsors;
            TotalSponsors = totalSponsors;
            PagingData = pagingData;
            FilterData = filterData;
            Messages = messages;
        }
    }

    public class SponsorAllocationPagingData
    {
        public int CurrentPage { get; set; }

        public int NrOfDocumentsPerPage { get; set; }
    }

    public class SponsorAllocationFilterData
    {
        public string NameOfSponsor { get; set; }
    }
}