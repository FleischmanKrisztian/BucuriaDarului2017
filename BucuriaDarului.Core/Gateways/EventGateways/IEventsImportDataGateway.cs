using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.EventGateways
{
    public interface IEventsImportDataGateway
    {
        void Insert(List<Event> events);
    }
}