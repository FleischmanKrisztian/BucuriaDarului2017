using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using Newtonsoft.Json;

namespace BucuriaDarului.Contexts
{
    public class EventEditContext
    {
        private readonly IEventEditGateway dataGateway;

        public EventEditContext(IEventEditGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public EventEditResponse Execute(EventEditRequest request)
        {
            var response = new EventEditResponse();

            var @event = ChangeNullValues(request.IncomingEvent);

            if (ContainsSpecialchar(@event))
            {
                response.ContainsSpecialChar = true;
                response.Message = "The Object Cannot contain Semi-Colons";
            }

            @event.DateOfEvent = @event.DateOfEvent.AddHours(5);

            if (response.ContainsSpecialChar == false)
            {
                var modifiedList = dataGateway.ReturnModificationList();

                var modifiedListString = JsonConvert.SerializeObject(modifiedList);
                if (!modifiedListString.Contains(@event._id))
                {
                    Event beforeEditingEvent = dataGateway.ReturnEvent(@event._id);
                    string beforeEditingEventString = JsonConvert.SerializeObject(beforeEditingEvent);
                    dataGateway.AddEventToModifiedList(beforeEditingEventString);
                }
                dataGateway.Edit(@event);
            }

            return response;
        }

        private bool ContainsSpecialchar(object @event)
        {
            string eventString = JsonConvert.SerializeObject(@event);
            bool containsSpecialChar = false;
            if (eventString.Contains(";"))
            {
                containsSpecialChar = true;
            }
            return containsSpecialChar;
        }

        private Event ChangeNullValues(Event incomingevent)
        {
            foreach (var property in incomingevent.GetType().GetProperties())
            {
                var propertyType = property.PropertyType;
                var value = property.GetValue(incomingevent, null);
                if (propertyType == typeof(string) && value == null)
                {
                    property.SetValue(incomingevent, string.Empty);
                }
            }
            return incomingevent;
        }
    }

    public class EventEditResponse
    {
        public string Message { get; set; }
        public bool ContainsSpecialChar { get; set; }
    }

    public class EventEditRequest
    {
        public Event IncomingEvent { get; set; }

        public EventEditRequest(Event incomingevent)
        {
            IncomingEvent = incomingevent;
        }
    }
}