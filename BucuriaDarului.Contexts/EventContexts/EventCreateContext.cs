using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.EventGateways;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace BucuriaDarului.Contexts.EventContexts
{
    public class EventCreateContext
    {
        private readonly IEventCreateGateway dataGateway;
        private EventCreateResponse response = new EventCreateResponse("", true);

        public EventCreateContext(IEventCreateGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public EventCreateResponse Execute(EventCreateRequest request)
        {
            var noNullRequest = ChangeNullValues(request);

            if (ContainsSpecialChar(noNullRequest))
            {
                response.IsValid = false;
                response.Message = "The Object Cannot contain Semi-Colons! ";
            }

            var @event = ValidateRequest(noNullRequest);

            if (response.IsValid)
            {
                dataGateway.Insert(@event);
            }
            return response;
        }

        private Event ValidateRequest(EventCreateRequest request)
        {
            if (request.NameOfEvent == "")
            {
                response.Message += "The Event must have a name! ";
                response.IsValid = false;
            }

            var validatedEvent = new Event
            {
                Id = Guid.NewGuid().ToString(),
                NameOfEvent = request.NameOfEvent,
                PlaceOfEvent = request.PlaceOfEvent,
                DateOfEvent = request.DateOfEvent.AddHours(5),
                NumberOfVolunteersNeeded = request.NumberOfVolunteersNeeded,
                TypeOfActivities = request.TypeOfActivities,
                TypeOfEvent = request.TypeOfEvent,
                Duration = request.Duration,
                AllocatedVolunteers = request.AllocatedVolunteers,
                NumberAllocatedVolunteers = request.NumberAllocatedVolunteers,
                AllocatedSponsors = request.AllocatedSponsors
            };

            return validatedEvent;
        }

        private static bool ContainsSpecialChar(object @event)
        {
            var eventString = JsonConvert.SerializeObject(@event);
            var containsSpecialChar = eventString.Contains(";");
            return containsSpecialChar;
        }

        private EventCreateRequest ChangeNullValues(EventCreateRequest request)
        {
            foreach (var property in request.GetType().GetProperties())
            {
                var propertyType = property.PropertyType;
                var value = property.GetValue(request, null);
                if (propertyType == typeof(string) && value == null)
                {
                    property.SetValue(request, string.Empty);
                }
            }
            return request;
        }
    }

    public class EventCreateResponse
    {
        public string Message { get; set; }

        public bool IsValid { get; set; }

        public EventCreateResponse(string message, bool isValid)
        {
            Message = message;
            IsValid = isValid;
        }
    }

    public class EventCreateRequest
    {
        public string NameOfEvent { get; set; }

        public string PlaceOfEvent { get; set; }

        public DateTime DateOfEvent { get; set; }

        public int NumberOfVolunteersNeeded { get; set; }

        public string TypeOfActivities { get; set; }

        public string TypeOfEvent { get; set; }

        public string Duration { get; set; }

        public string AllocatedVolunteers { get; set; }

        public int NumberAllocatedVolunteers { get; set; }

        public string AllocatedSponsors { get; set; }
    }
}