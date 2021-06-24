using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BucuriaDarului.Contexts
{
    public class EventsSponsorAllocationContext
    {
        private readonly IEventSponsorAllocationGateway dataGateway;

        public EventsSponsorAllocationContext(IEventSponsorAllocationGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public EventsSponsorAllocationDisplayResponse Execute(EventsSponsorsAllocationDisplayRequest request)
        {
            Event event_ = dataGateway.GetEvent(request.EventId);
            List<Sponsor> sponsors = dataGateway.GetListOfSponsors();
            int totalSponsors = sponsors.Count();
            string sponsorsIdString = GetStringOfIds(sponsors);
            sponsors = GetSponsorsAfterPaging(sponsors, request.PagingData.CurrentPage, request.PagingData.NrOfDocumentsPerPage);
            sponsors = GetSponsorsAfterSearching(sponsors, request.FilterData.NameOfSponsor);
            List<Event> events = dataGateway.GetListOfEvents();
            string AllocatedVolunteersIdString = GetAllocatedSponsorsString(events, request.EventId);

            return new EventsSponsorAllocationDisplayResponse(event_, sponsors, totalSponsors, sponsorsIdString, request.PagingData, request.FilterData);

        }
        public void UdateAllocationToEvent(EventsSponsorAllocationRequest request)
        {
            Event event_ = dataGateway.GetEvent(request.EventId);
            List<Sponsor> sponsors = dataGateway.GetListOfSponsors();
            sponsors = GetSponsorsByIds(sponsors, request.SponsorIds);
            event_.AllocatedSponsors= GetSponsorNames(sponsors);
            dataGateway.UpdateEvent(request.EventId, event_);
        }

        private string GetSponsorNames(List<Sponsor> sponsors)
        {
            string sponsornames = "";
            for (int i = 0; i < sponsors.Count; i++)
            {
                var sponsor = sponsors[i];
                sponsornames = sponsornames + sponsor.NameOfSponsor + " / ";
            }
            return sponsornames;
        }
        private List<Sponsor> GetSponsorsByIds(List<Sponsor> sponsors, string[] sponsorids)
        {
            List<Sponsor> sponsorlist = new List<Sponsor>();
            for (int i = 0; i < sponsorids.Length; i++)
            {
                Sponsor singlesponsor = sponsors.Where(x => x._id == sponsorids[i]).First();
                sponsorlist.Add(singlesponsor);
            }
            return sponsorlist;
        }
        private dynamic GetAllocatedSponsorsString(List<Event> events, string id)
        {
            Event returnedevent = events.Find(b => b._id.ToString() == id);
            returnedevent.AllocatedSponsors += " / ";
            return returnedevent.AllocatedSponsors;
        }
        private List<Sponsor> GetSponsorsAfterSearching(List<Sponsor> sponsors, string searching)
        {
            if (searching != null)
            {
                sponsors = sponsors.Where(x => x.NameOfSponsor.Contains(searching, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            return sponsors;
        }
        private List<Sponsor> GetSponsorsAfterPaging(List<Sponsor> sponsors, int page, int nrofdocs)
        {
            sponsors = sponsors.AsQueryable().Skip((page - 1) * nrofdocs).ToList();
            sponsors = sponsors.AsQueryable().Take(nrofdocs).ToList();
            return sponsors;
        }
        private string GetStringOfIds(List<Sponsor> sponsors)
        {
            string stringofids = "sponsorCSV";
            foreach (Sponsor sponsor in sponsors)
            {
                stringofids = stringofids + "," + sponsor._id;
            }
            return stringofids;
        }
    }


    public class EventsSponsorsAllocationDisplayRequest
    {
        public string EventId { get; set; }
        public SponsorAllocationPagingData PagingData { get; set; }
        public SponsorAllocationFilterData FilterData { get; set; }

        public EventsSponsorsAllocationDisplayRequest(string eventId, int page, int nrofdocs, string searching)
        {
            this.EventId = eventId;
            SponsorAllocationPagingData pagingData = new SponsorAllocationPagingData();
            SponsorAllocationFilterData filterData = new SponsorAllocationFilterData();

            if (searching != null && searching != "")
                filterData.NameOfSponsor = searching;
            else
                filterData.NameOfSponsor = "";
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

    public class EventsSponsorAllocationDisplayResponse
    {
        public Event Event { get; set; }
        public List<Sponsor> Sponsors { get; set; }
        public int TotalSponsors { get; set; }
        public string AllocatedSponsorsIdString { get; set; }

        public SponsorAllocationPagingData PagingData { get; set; }
        public SponsorAllocationFilterData FilterData { get; set; }

        public EventsSponsorAllocationDisplayResponse(Event event_, List<Sponsor> sponsors, int totalSponsors, string allocatedSponsorsIdString, SponsorAllocationPagingData pagingData, SponsorAllocationFilterData filterData)
        {
            Event = event_;
            Sponsors = sponsors;
            TotalSponsors = totalSponsors;
            AllocatedSponsorsIdString = allocatedSponsorsIdString;
            PagingData = pagingData;
            FilterData = filterData;
        }
    }

    public class EventsSponsorAllocationRequest
    {
        public string EventId { get; set; }
        public string[] SponsorIds { get; set; }

        public EventsSponsorAllocationRequest(string[] sponsorIds, string eventId)
        {
            this.EventId = eventId;
            this.SponsorIds = sponsorIds;
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
