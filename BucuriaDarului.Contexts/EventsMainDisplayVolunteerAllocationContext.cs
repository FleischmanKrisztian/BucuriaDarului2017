using System;
using System.Collections.Generic;
using System.Text;
using BucuriaDarului.Core.Gateways;

namespace BucuriaDarului.Contexts
{
    class EventsMainDisplayVolunteerAllocationContext
    {
        private readonly IEnventsMainDataVolunteerAllocationGateways dataGateway;

        public EventsMainDisplayVolunteerAllocationContext(IEnventsMainDataVolunteerAllocationGateways dataGateway)
        {
            this.dataGateway = dataGateway;
        }


    }
}
