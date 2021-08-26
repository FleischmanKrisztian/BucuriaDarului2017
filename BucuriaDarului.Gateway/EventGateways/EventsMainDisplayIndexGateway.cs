using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.EventGateways;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.EventGateways
{
    public class EventsMainDisplayIndexGateway : IEventsMainDisplayIndexGateway
    {
        public List<Event> GetListOfEvents()
        {
            return ListEventsGateway.GetListOfEvents();
        }
    }
}