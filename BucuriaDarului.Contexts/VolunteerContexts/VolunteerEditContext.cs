using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
using Newtonsoft.Json;
using System;

namespace BucuriaDarului.Contexts.VolunteerContexts
{
    public class VolunteerEditContext
    {
        private readonly IVolunteerEditGateway dataGateway;
        private VolunteerEditResponse response = new VolunteerEditResponse("", true);

        public VolunteerEditContext(IVolunteerEditGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public VolunteerEditResponse Execute(VolunteerEditRequest request, byte[] image)
        {
            var noNullRequest = ChangeNullValues(request);

            if (ContainsSpecialChar(noNullRequest))
            {
                response.IsValid = false;
                response.Message = "The Object Cannot contain Semi-Colons! ";
            }

            var volunteer = ValidateRequest(noNullRequest, image);
            var beforeEditingVolunteer = dataGateway.ReturnVolunteer(volunteer.Id);
            if (volunteer.Image == null)
                volunteer.Image = beforeEditingVolunteer.Image;

            if (response.IsValid)
            {
                var modifiedList = dataGateway.ReturnModificationList();
                var modifiedListString = JsonConvert.SerializeObject(modifiedList);
                if (!modifiedListString.Contains(volunteer.Id))
                {
                    var beforeEditingVolunteerString = JsonConvert.SerializeObject(beforeEditingVolunteer);
                    dataGateway.AddVolunteerToModifiedList(beforeEditingVolunteerString);
                }
                dataGateway.Edit(volunteer);
            }
            response.Volunteer = volunteer;

            return response;
        }

        private Volunteer ValidateRequest(VolunteerEditRequest request, byte[] image)
        {
            if (request.Fullname == "")
            {
                response.Message += "The Volunteer must have a name!";
                response.IsValid = false;
            }

            request.CI.ExpirationDate = request.CI.ExpirationDate.AddHours(5);

            var validatedVolunteer = new Volunteer
            {
                Id = request.Id,
                Fullname = request.Fullname,
                Birthdate = request.Birthdate.AddHours(5),
                Address = request.Address,
                Gender = request.Gender,
                DesiredWorkplace = request.DesiredWorkplace,
                CNP = request.CNP,
                FieldOfActivity = request.FieldOfActivity,
                Occupation = request.Occupation,
                CI = request.CI,
                InActivity = false,
                HourCount = request.HourCount,
                ContactInformation = request.ContactInformation,
                AdditionalInfo = request.AdditionalInfo
            };

            if (image.Length > 2)
            {
                validatedVolunteer.Image = image;
            }

            return validatedVolunteer;
        }

        private static bool ContainsSpecialChar(object volunteer)
        {
            var volunteerString = JsonConvert.SerializeObject(volunteer);
            var containsSpecialChar = volunteerString.Contains(";");
            return containsSpecialChar;
        }

        private static VolunteerEditRequest ChangeNullValues(VolunteerEditRequest request)
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

            foreach (var property in request.AdditionalInfo.GetType().GetProperties())
            {
                var propertyType = property.PropertyType;
                if (propertyType != typeof(DateTime))
                {
                    var value = property.GetValue(request.AdditionalInfo, null);
                    if (propertyType == typeof(string) && value == null)
                    {
                        property.SetValue(request.AdditionalInfo, string.Empty);
                    }
                }
            }
            foreach (var property in request.ContactInformation.GetType().GetProperties())
            {
                var propertyType = property.PropertyType;
                if (propertyType != typeof(DateTime))
                {
                    var value = property.GetValue(request.ContactInformation, null);
                    if (propertyType == typeof(string) && value == null)
                    {
                        property.SetValue(request.ContactInformation, string.Empty);
                    }
                }
            }
            foreach (var property in request.CI.GetType().GetProperties())
            {
                var propertyType = property.PropertyType;
                if (propertyType != typeof(DateTime))
                {
                    var value = property.GetValue(request.CI, null);
                    if (propertyType == typeof(string) && value == null)
                    {
                        property.SetValue(request.CI, string.Empty);
                    }
                }
            }

            return request;
        }
    }

    public class VolunteerEditResponse
    {
        public Volunteer Volunteer { get; set; }
        public string Message { get; set; }
        public bool IsValid { get; set; }

        public VolunteerEditResponse(string message, bool isValid)
        {
            Message = message;
            IsValid = isValid;
        }
    }

    public class VolunteerEditRequest
    {
        public string Id { get; set; }

        public string Fullname { get; set; }

        public DateTime Birthdate { get; set; }

        public string Address { get; set; }

        public Gender Gender { get; set; }

        public string DesiredWorkplace { get; set; }

        public string CNP { get; set; }

        public string FieldOfActivity { get; set; }

        public string Occupation { get; set; }

        public CI CI { get; set; }

        public string HourCount { get; set; }

        public ContactInformation ContactInformation { get; set; }

        public AdditionalInfo AdditionalInfo { get; set; }

        public byte[] Image { get; set; }
    }
}