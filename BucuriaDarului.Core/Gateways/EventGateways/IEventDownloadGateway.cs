using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.EventGateways
{
    public interface IEventDownloadGateway
    {
        List<Event> GetListOfEvents();
    }
}