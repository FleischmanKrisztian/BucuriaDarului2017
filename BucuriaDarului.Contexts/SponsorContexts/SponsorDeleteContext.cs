using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.SponsorGateways;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            events = events.FindAll(x => x.AllocatedSponsorsID.Contains(id));
            
            if (events.Count() != 0)
            {
                foreach (var e in events)
                {
                    var eventToUpdate = new Event();
                    eventToUpdate = e;
                    eventToUpdate.AllocatedVolunteersID = eventToUpdate.AllocatedSponsorsID.Replace(" + " + id, "");
                    eventToUpdate.AllocatedVolunteersID = eventToUpdate.AllocatedSponsorsID.Replace(id, "");
                    var sponsor = dataGateway.GetSponsor(id);
                    eventToUpdate.AllocatedVolunteers = eventToUpdate.AllocatedSponsors.Replace(" + " + sponsor.NameOfSponsor, "");
                    eventToUpdate.AllocatedVolunteersID = eventToUpdate.AllocatedSponsors.Replace(sponsor.NameOfSponsor, "");
                  

                    dataGateway.UpdateEvent(e.Id, eventToUpdate);
                }
            }

            dataGateway.DeleteSponsor(id);


        }
    }
}
