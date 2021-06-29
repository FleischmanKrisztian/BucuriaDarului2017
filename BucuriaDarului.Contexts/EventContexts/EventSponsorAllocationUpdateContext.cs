using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using System.Collections.Generic;
using System.Linq;

namespace BucuriaDarului.Contexts.EventContexts
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
            Event @event= dataGateway.ReturnEvent(request.EventId);
            List<Sponsor> sponsors = dataGateway.GetListOfSponsors();

            sponsors = GetSponsorsByIds(sponsors, request.SponsorIds);
            var allocatedSponsors = GetSponsorNames(sponsors);
            @event.AllocatedSponsors = allocatedSponsors;
            dataGateway.UpdateEvent(request.EventId, @event);

            if (@event.AllocatedSponsors != allocatedSponsors)
            {
                response.IsValid = false;
                
            }
            return response;
        }

        private string GetSponsorNames(List<Sponsor> sponsors)
        {
            var sponsorNames = "";
            foreach (var sponsor in sponsors)
            {
                sponsorNames = sponsorNames + sponsor.NameOfSponsor + " / ";
            }
            return sponsorNames;
        }

        // Can probably only return the names of sponsors in this method
        private List<Sponsor> GetSponsorsByIds(List<Sponsor> sponsors, string[] sponsorIds)

        {
            var sponsorList = new List<Sponsor>();
            foreach (var sponsorId in sponsorIds)
            {
                var singleSponsor = sponsors.First(x => x._id == sponsorId);
                sponsorList.Add(singleSponsor);
            }
            return sponsorList;
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