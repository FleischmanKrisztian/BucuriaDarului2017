using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using Newtonsoft.Json;
using System;

namespace BucuriaDarului.Contexts
{
    public class EventCreateContext
    {
        private readonly IEventCreateGateway dataGateway;

        public EventCreateContext(IEventCreateGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public EventCreateResponse Execute(EventCreateRequest request)
        {
            var response = new EventCreateResponse("", false);
            var @event = ChangeNullValues(request.Incomingevent);

            if (ContainsSpecialchar(@event))
            {
                response.ContainsSpecialChar = true;
                response.Message = "The Object Cannot contain Semi-Colons";
            }

            @event._id = Guid.NewGuid().ToString();

            @event.DateOfEvent = @event.DateOfEvent.AddHours(5);

            if (response.ContainsSpecialChar == false)
            {
                dataGateway.Insert(@event);
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

    public class EventCreateResponse
    {
        public string Message { get; set; }

        public bool ContainsSpecialChar { get; set; }

        public EventCreateResponse(string message, bool containsSpecialChar)
        {
            Message = message;
            ContainsSpecialChar = containsSpecialChar;
        }
    }

    public class EventCreateRequest
    {
        public Event Incomingevent { get; set; }

        public EventCreateRequest(Event incomingEvent)
        {
            Incomingevent = incomingEvent;
        }
    }
}