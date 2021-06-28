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

        public EventSponsorAllocationResponse Execute(EventsSponsorAllocationRequest request)
        {
            bool updateCompleted = false;
            Event event_ = dataGateway.ReturnEvent(request.EventId);
            List<KeyValuePair<string, string>> messages = new List<KeyValuePair<string, string>>();
            List<Sponsor> sponsors = dataGateway.GetListOfSponsors();
            sponsors = GetSponsorsByIds(sponsors, request.SponsorIds);
            string allocatedSponsors = GetSponsorNames(sponsors);
            event_.AllocatedSponsors = allocatedSponsors;
            dataGateway.UpdateEvent(request.EventId, event_);

            if (event_.AllocatedSponsors == allocatedSponsors)
            {
                updateCompleted = true;

            }
            else
            {
                updateCompleted = false;
                messages.Add(item: new KeyValuePair<string, string>("fail", "Update failed!Please try again!"));
            }
            return new EventSponsorAllocationResponse(updateCompleted, messages);
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
        public List<KeyValuePair<string, string>> Messages { get; set; }

        public EventSponsorAllocationResponse(bool updatedCompleted, List<KeyValuePair<string, string>> messages)
        {
            this.UpdateCompleted = updatedCompleted;

            if (messages.Count() != 0)
                Messages = messages;
            else
                Messages.Add(item: new KeyValuePair<string, string>("success", "The event has been successfuly updated!"));
        }
    }
}
