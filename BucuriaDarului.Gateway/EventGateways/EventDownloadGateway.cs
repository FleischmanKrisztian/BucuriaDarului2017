using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.EventGateways;
using System.Collections.Generic;

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