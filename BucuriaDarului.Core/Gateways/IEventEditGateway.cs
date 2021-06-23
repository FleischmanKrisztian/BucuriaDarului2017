using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways
{
    public interface IEventEditGateway
    {
        public void Edit(Event @event);

        public Event ReturnEvent(string id);

        public List<ModifiedIDs> ReturnModificationList();

        public void AddEventToModifiedList(string beforeEditingEventString);
    }
}