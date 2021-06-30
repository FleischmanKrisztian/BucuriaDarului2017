using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways
{
    public interface IEventsMainDisplayIndexGateway
    {
        List<Event> GetListOfEvents();
    }
}