﻿using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
using Newtonsoft.Json;
using System;

namespace BucuriaDarului.Contexts.VolunteerContexts
{
    public class VolunteerCreateContext
    {
        private readonly IVolunteerCreateGateway dataGateway;
        private VolunteerCreateResponse response = new VolunteerCreateResponse("", true);

        public VolunteerCreateContext(IVolunteerCreateGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public VolunteerCreateResponse Execute(VolunteerCreateRequest request, byte[] image)
        {
            var noNullRequest = ChangeNullValues(request);

            if (ContainsSpecialChar(noNullRequest))
            {
                response.IsValid = false;
                response.Message = "The Object Cannot contain Semi-Colons! ";
            }

            var volunteer = ValidateRequest(noNullRequest, image);

            if (response.IsValid)
            {
                dataGateway.Insert(volunteer);
            }
            return response;
        }

        private Volunteer ValidateRequest(VolunteerCreateRequest request, byte[] image)
        {
            if (request.Fullname == "")
            {
                response.Message += "The Volunteer must have a name! ";
                response.IsValid = false;
            }

            var validatedVolunteer = new Volunteer
            {
                Id = Guid.NewGuid().ToString(),
                Fullname = request.Fullname,
                Birthdate = request.Birthdate,
                Address = request.Address,
                Gender = request.Gender,
                DesiredWorkplace = request.DesiredWorkplace,
                CNP = request.CNP,
                FieldOfActivity = request.FieldOfActivity,
                Occupation = request.Occupation,
                CI = request.CI,
                InActivity = request.InActivity,
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

        private static VolunteerCreateRequest ChangeNullValues(VolunteerCreateRequest request)
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

    public class VolunteerCreateResponse
    {
        public string Message { get; set; }

        public bool IsValid { get; set; }

        public VolunteerCreateResponse(string message, bool isValid)
        {
            Message = message;
            IsValid = isValid;
        }
    }

    public class VolunteerCreateRequest
    {
        public string Fullname { get; set; }

        public DateTime Birthdate { get; set; }

        public string Address { get; set; }

        public Gender Gender { get; set; }

        public string DesiredWorkplace { get; set; }

        public string CNP { get; set; }

        public string FieldOfActivity { get; set; }

        public string Occupation { get; set; }

        public CI CI { get; set; }

        public bool InActivity { get; set; }

        public int HourCount { get; set; }

        public ContactInformation ContactInformation { get; set; }

        public AdditionalInfo AdditionalInfo { get; set; }

        public byte[] Image { get; set; }
    }
}