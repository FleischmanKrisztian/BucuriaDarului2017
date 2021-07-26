using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.EventGateways;
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
            var response = new EventSponsorAllocationResponse();
            var @event = dataGateway.ReturnEvent(request.EventId);
            var sponsors = dataGateway.GetListOfSponsors();
            var sponsorsToAdd = GetSponsorsByIds(sponsors, request.CheckedIds);
            var sponsorsToRemove = GetSponsorsByIds(sponsors, request.AllIds);
            var sponsorsForAllocation = @event.AllocatedSponsors;
            sponsorsForAllocation = RemoveUncheckedSponsors(sponsorsForAllocation, sponsorsToRemove);
            sponsorsForAllocation = CheckForDuplicate(sponsorsForAllocation, GetSponsorNames(sponsorsToAdd));
            @event.AllocatedSponsors = sponsorsForAllocation;
            dataGateway.UpdateEvent(request.EventId, @event);

            return response;
        }

        private string RemoveUncheckedSponsors(string allSponsors, List<Sponsor> sponsorsToRemove)
        {
            foreach (var spons in sponsorsToRemove)
            {
                allSponsors = allSponsors.Replace(" + " + spons.NameOfSponsor, "");
                allSponsors = allSponsors.Replace(spons.NameOfSponsor, "");
            }
            return allSponsors;
        }

        private string CheckForDuplicate(string previouslyAllocatedSponsors, List<string> names)
        {
            var AllSponsors = previouslyAllocatedSponsors;
            foreach (var name in names)
            {
                if (!previouslyAllocatedSponsors.Contains(name))
                {
                    AllSponsors += " + " + name;
                }
            }
            return AllSponsors;
        }

        private List<string> GetSponsorNames(List<Sponsor> sponsors)
        {
            var listOfNames = new List<string>();
            foreach (var sponsor in sponsors)
            {
                listOfNames.Add(sponsor.NameOfSponsor);
            }
            return listOfNames;
        }

        private List<Sponsor> GetSponsorsByIds(List<Sponsor> sponsors, string[] sponsorIds)

        {
            var sponsorList = new List<Sponsor>();
            foreach (var sponsorId in sponsorIds)
            {
                var singleSponsor = sponsors.First(x => x.Id == sponsorId);
                sponsorList.Add(singleSponsor);
            }
            return sponsorList;
        }
    }

    public class EventsSponsorAllocationRequest
    {
        public string EventId { get; set; }

        public string[] CheckedIds { get; set; }

        public string[] AllIds { get; set; }

        public EventsSponsorAllocationRequest(string[] checkedIds, string[] allIds, string eventId)
        {
            EventId = eventId;
            AllIds = allIds;
            CheckedIds = checkedIds;
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