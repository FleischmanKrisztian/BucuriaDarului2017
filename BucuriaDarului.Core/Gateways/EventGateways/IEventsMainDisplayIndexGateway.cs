using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.EventGateways
{
    public interface IEventsMainDisplayIndexGateway
    {
        List<Event> GetListOfEvents();
    }
}