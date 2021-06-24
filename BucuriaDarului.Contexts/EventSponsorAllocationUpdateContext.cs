using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BucuriaDarului.Contexts
{
    public class EventSponsorAllocationUpdateContext
    {
        private readonly IEventSponsorAllocationUpdateGateway dataGateway;

        public EventSponsorAllocationUpdateContext(IEventSponsorAllocationUpdateGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public EventSponsorAllocationResponse UpdateAllocationToEvent(EventsSponsorAllocationRequest request)
        {
            bool updateCompleted = false;
            string message = "";
            Event event_ = dataGateway.GetEvent(request.EventId);
            List<Sponsor> sponsors = dataGateway.GetListOfSponsors();
            sponsors = GetSponsorsByIds(sponsors, request.SponsorIds);
            string allocatedSponsors = GetSponsorNames(sponsors);
            event_.AllocatedSponsors = allocatedSponsors;
            dataGateway.UpdateEvent(request.EventId, event_);

            if (event_.AllocatedSponsors == allocatedSponsors)
            {
                updateCompleted = true;
                message = "The event has been successfuly updated!";
            }
            else
            {
                updateCompleted = false;
                message = "Update failed!!!";
            }
            return new EventSponsorAllocationResponse(updateCompleted, message);
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
        public bool UpdateCompleted { get; set; }
        public string Message { get; set; }

        public EventSponsorAllocationResponse(bool updatedCompleted, string message)
        {
            this.UpdateCompleted = updatedCompleted;

            if (message != " " && message != null)
                this.Message = message;
            else
                this.Message = "Update failed!!!";
        }
    }
}
