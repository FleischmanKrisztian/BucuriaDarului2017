using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.SponsorGateways
{
    public interface ISponsorsImportDataGateway
    {
        void Insert(List<Sponsor> sponsors);
        List<Sponsor> GetSponsors();

        Sponsor GetSponsor(string sponsorId);
        void Update(List<Sponsor> sponsors);
    }
}
