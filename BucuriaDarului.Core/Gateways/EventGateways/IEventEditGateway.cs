using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.EventGateways
{
    public interface IEventEditGateway
    {
        public void Edit(Event @event);

        public List<ModifiedIDs> ReturnModificationList();

        public void AddEventToModifiedList(string beforeEditingEventString);

        public Event ReturnEvent(string id);
    }
}