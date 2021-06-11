using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways
{
    public interface IEventsImportDataGateway
    {
        void Insert(List<Event> events);
    }
}