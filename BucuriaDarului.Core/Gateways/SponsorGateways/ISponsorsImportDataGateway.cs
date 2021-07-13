using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.SponsorGateways
{
    public interface ISponsorsImportDataGateway
    {
        void Insert(List<Sponsor> events);
    }
}
