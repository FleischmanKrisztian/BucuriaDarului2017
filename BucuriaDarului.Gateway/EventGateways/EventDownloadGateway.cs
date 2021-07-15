using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.EventGateways;
using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Gateway.EventGateways
{
    public class EventDownloadGateway : IEventDownloadGateway
    {
        public List<Event> GetListOfEvents()
        {
            return ListEventsGateway.GetListOfEvents();
        }
    }
}
