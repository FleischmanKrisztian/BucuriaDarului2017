using BucuriaDarului.Core.Gateways.SponsorGateways;
using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Contexts.SponsorContexts
{
    public class SponsorDownloadContext
    {
        private readonly ISponsorDownloadGateway dataGateway;

        public SponsorDownloadContext(ISponsorDownloadGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }
    }
}
