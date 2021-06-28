using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
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
            var totalSponsors = sponsors.Count();
            var sponsorsIdString = GetStringOfIds(sponsors);
            sponsors = GetSponsorsAfterPaging(sponsors, request.PagingData.CurrentPage, request.PagingData.NrOfDocumentsPerPage);
            sponsors = GetSponsorsAfterSearching(sponsors, request.FilterData.NameOfSponsor);

            return new EventsSponsorAllocationDisplayResponse(@event, sponsors, totalSponsors, sponsorsIdString, request.PagingData, request.FilterData);
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
                stringOfIds = stringOfIds + "," + sponsor._id;
            }
            return stringOfIds;
        }
    }

    public class EventsSponsorsAllocationDisplayRequest
    {
        public string EventId { get; set; }
        public SponsorAllocationPagingData PagingData { get; set; }
        public SponsorAllocationFilterData FilterData { get; set; }

        public EventsSponsorsAllocationDisplayRequest(string eventId, int page, int nrOfDocs, string searching)
        {
            this.EventId = eventId;
            var pagingData = new SponsorAllocationPagingData();
            var filterData = new SponsorAllocationFilterData();

            filterData.NameOfSponsor = !string.IsNullOrEmpty(searching) ? searching : "";
            pagingData.CurrentPage = GetCurrentPage(page);
            pagingData.NrOfDocumentsPerPage = nrOfDocs;

            this.FilterData = filterData;
            this.PagingData = pagingData;
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
        public string AllocatedSponsorsIdString { get; set; }

        public SponsorAllocationPagingData PagingData { get; set; }
        public SponsorAllocationFilterData FilterData { get; set; }

        public EventsSponsorAllocationDisplayResponse(Event @event, List<Sponsor> sponsors, int totalSponsors, string allocatedSponsorsIdString, SponsorAllocationPagingData pagingData, SponsorAllocationFilterData filterData)
        {
            Event = @event;
            Sponsors = sponsors;
            TotalSponsors = totalSponsors;
            AllocatedSponsorsIdString = allocatedSponsorsIdString;
            PagingData = pagingData;
            FilterData = filterData;
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