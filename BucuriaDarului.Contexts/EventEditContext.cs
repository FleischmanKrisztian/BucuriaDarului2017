using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using Newtonsoft.Json;
using System;

namespace BucuriaDarului.Contexts
{
    public class EventEditContext
    {
        private readonly IEventEditGateway dataGateway;
        EventEditResponse response = new EventEditResponse("", false, true);

        public EventEditContext(IEventEditGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public EventEditResponse Execute(EventEditRequest request)
        {
            var noNullRequest = ChangeNullValues(request);

            if (ContainsSpecialchar(noNullRequest))
            {
                response.ContainsSpecialChar = true;
                response.Message = "The Object Cannot contain Semi-Colons! ";
            }

            var @event = ValidateRequest(noNullRequest);

            if (response.ContainsSpecialChar == false && response.IsValid)
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

            var validatedEvent = new Event();
            validatedEvent._id = request._id;
            validatedEvent.NameOfEvent = request.NameOfEvent;
            validatedEvent.PlaceOfEvent = request.PlaceOfEvent;
            validatedEvent.DateOfEvent = request.DateOfEvent.AddHours(5);
            validatedEvent.NumberOfVolunteersNeeded = request.NumberOfVolunteersNeeded;
            validatedEvent.TypeOfActivities = request.TypeOfActivities;
            validatedEvent.TypeOfEvent = request.TypeOfEvent;
            validatedEvent.Duration = request.Duration;
            validatedEvent.AllocatedVolunteers = request.AllocatedVolunteers;
            validatedEvent.NumberAllocatedVolunteers = request.NumberAllocatedVolunteers;
            validatedEvent.AllocatedSponsors = request.AllocatedSponsors;

            return validatedEvent;
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

        private EventEditRequest ChangeNullValues(EventEditRequest request)
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

        public bool ContainsSpecialChar { get; set; }

        public EventEditResponse(string message, bool containsSpecialChar, bool isValid)
        {
            Message = message;
            ContainsSpecialChar = containsSpecialChar;
            IsValid = isValid;
        }
    }

    public class EventEditRequest
    {
        public string _id { get; set; }
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