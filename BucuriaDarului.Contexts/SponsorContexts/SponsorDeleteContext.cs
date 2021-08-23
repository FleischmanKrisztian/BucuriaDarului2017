using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.SponsorGateways;
using System.Collections.Generic;
using System.Linq;

namespace BucuriaDarului.Contexts.SponsorContexts
{
    public class SponsorDeleteContext
    {
        private readonly ISponsorDeleteGateway dataGateway;

        public SponsorDeleteContext(ISponsorDeleteGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public void Execute(string id)
        {
            var events = dataGateway.GetEvents();
            var listOfEventsToModify = new List<Event>();
            foreach (var e in events)
            {
                if (e.AllocatedSponsorsID.Contains(id))
                {
                    listOfEventsToModify.Add(e);
                }
            }

            if (listOfEventsToModify.Count() != 0)
            {
                foreach (var e in listOfEventsToModify)
                {
                    var eventToUpdate = new Event();
                    eventToUpdate = e;
                    eventToUpdate.AllocatedSponsorsID = eventToUpdate.AllocatedSponsorsID.Replace(" + " + id, "");
                    eventToUpdate.AllocatedSponsorsID = eventToUpdate.AllocatedSponsorsID.Replace(id, "");
                    var sponsor = dataGateway.GetSponsor(id);
                    eventToUpdate.AllocatedSponsors = eventToUpdate.AllocatedSponsors.Replace(" + " + sponsor.NameOfSponsor, "");
                    eventToUpdate.AllocatedSponsors = eventToUpdate.AllocatedSponsors.Replace(sponsor.NameOfSponsor, "");

                    dataGateway.UpdateEvent(e.Id, eventToUpdate);
                }
            }

            dataGateway.DeleteSponsor(id);
        }
    }
}