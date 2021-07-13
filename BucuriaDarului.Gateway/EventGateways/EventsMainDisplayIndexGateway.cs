using System.Collections.Generic;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using BucuriaDarului.Core.Gateways.EventGateways;
using MongoDB.Driver;

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