using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.EventGateways;
using Newtonsoft.Json;
using System;

namespace BucuriaDarului.Contexts.EventContexts
{
    public class EventEditContext
    {
        private readonly IEventEditGateway dataGateway;
        private EventEditResponse response = new EventEditResponse("", true);

        public EventEditContext(IEventEditGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public EventEditResponse Execute(EventEditRequest request)
        {
            var beforeEditingEvent = dataGateway.ReturnEvent(request.Id);
            request.AllocatedSponsorsId = beforeEditingEvent.AllocatedSponsorsId;
            request.AllocatedVolunteersId = beforeEditingEvent.AllocatedVolunteersId;
            var noNullRequest = ChangeNullValues(request);

            if (ContainsSpecialChar(noNullRequest))
            {
                response.IsValid = false;
                response.Message = "The Object Cannot contain Semi-Colons! ";
            }

            var @event = ValidateRequest(noNullRequest);

            if (response.IsValid)
            {
                var modifiedList = dataGateway.ReturnModificationList();
                var modifiedListString = JsonConvert.SerializeObject(modifiedList);
                if (!modifiedListString.Contains(@event.Id))
                {
                    var beforeEditingEventString = JsonConvert.SerializeObject(beforeEditingEvent);
                    dataGateway.AddEventToModifiedList(beforeEditingEventString);
                }
                dataGateway.Edit(@event);
            }
            response.Event = @event;

            return response;
        }

        private Event ValidateRequest(EventEditRequest request)
        {
            if (request.NameOfEvent == "")
            {
                response.Message += "The Event must have a name! ";
                response.IsValid = false;
            }

            var validatedEvent = new Event
            {
                Id = request.Id,
                NameOfEvent = request.NameOfEvent,
                PlaceOfEvent = request.PlaceOfEvent,
                DateOfEvent = request.DateOfEvent.AddHours(5),
                NumberOfVolunteersNeeded = request.NumberOfVolunteersNeeded,
                TypeOfActivities = request.TypeOfActivities,
                TypeOfEvent = request.TypeOfEvent,
                Duration = request.Duration,
                AllocatedVolunteers = request.AllocatedVolunteers,
                NumberAllocatedVolunteers = request.NumberAllocatedVolunteers,
                AllocatedSponsors = request.AllocatedSponsors,
                AllocatedVolunteersId = request.AllocatedVolunteersId,
                AllocatedSponsorsId = request.AllocatedSponsorsId
            };

            return validatedEvent;
        }

        private bool ContainsSpecialChar(object @event)
        {
            var eventString = JsonConvert.SerializeObject(@event);
            var containsSpecialChar = eventString.Contains(";");
            return containsSpecialChar;
        }

        private static EventEditRequest ChangeNullValues(EventEditRequest request)
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

    public class EventEditResponse
    {
        public Event Event { get; set; }
        public string Message { get; set; }

        public bool IsValid { get; set; }

        public EventEditResponse(string message, bool isValid)
        {
            Message = message;
            IsValid = isValid;
        }
    }

    public class EventEditRequest
    {
        public string Id { get; set; }
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

        public string AllocatedVolunteersId { get; set; }

        public string AllocatedSponsorsId { get; set; }
    }
}