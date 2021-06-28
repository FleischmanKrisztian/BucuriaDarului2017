using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using System.Collections.Generic;
using System.Linq;

namespace BucuriaDarului.Contexts
{
    public class EventSponsorAllocationUpdateContext
    {
        private readonly IEventSponsorAllocationUpdateGateway dataGateway;

        public EventSponsorAllocationUpdateContext(IEventSponsorAllocationUpdateGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public EventSponsorAllocationResponse Execute(EventsSponsorAllocationRequest request)
        {
            EventSponsorAllocationResponse response = new EventSponsorAllocationResponse();
            Event event_ = dataGateway.ReturnEvent(request.EventId);
            List<Sponsor> sponsors = dataGateway.GetListOfSponsors();
            sponsors = GetSponsorsByIds(sponsors, request.SponsorIds);
            string allocatedSponsors = GetSponsorNames(sponsors);
            event_.AllocatedSponsors = allocatedSponsors;
            dataGateway.UpdateEvent(request.EventId, event_);

            if (event_.AllocatedSponsors != allocatedSponsors)
            {
                response.IsValid = false;
                
            }
            return response;
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

    public class EventSponsorAllocationResponse
    {
        public bool IsValid { get; set; }
        

        public EventSponsorAllocationResponse()
        {
            IsValid = true;
           
        }
    }
}