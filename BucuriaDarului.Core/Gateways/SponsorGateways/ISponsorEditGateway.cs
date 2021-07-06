using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.SponsorGateways
{
    public interface ISponsorEditGateway
    {
        public void Edit(Sponsor @sponsor);

        public List<ModifiedIDs> ReturnModificationList();

        public void AddSponsorToModifiedList(string beforeEditingSponsorString);
        public Sponsor ReturnSponsor(string id);
    }
}