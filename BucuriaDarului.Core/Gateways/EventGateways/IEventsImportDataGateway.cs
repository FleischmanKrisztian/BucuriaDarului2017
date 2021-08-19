using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.EventGateways
{
    public interface IEventsImportDataGateway
    {
        void Insert(List<Event> events);

        List<Event> GetEvents();

        Event GetEvent(string eventId);
        void Update(List<Event> events);
    }
}