using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.SponsorGateways;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;

namespace BucuriaDarului.Contexts.SponsorContexts
{
    public class SponsorCreateContext
    {
        private readonly ISponsorCreateGateway dataGateway;
        private SponsorCreateResponse response = new SponsorCreateResponse("", false, true);
        private readonly IStringLocalizer localizer;

        public SponsorCreateContext(ISponsorCreateGateway dataGateway, IStringLocalizer localizer)
        {
            this.localizer = localizer;
            this.dataGateway = dataGateway;
        }

        public SponsorCreateResponse Execute(SponsorCreateRequest request)
        {
            var noNullRequest = ChangeNullValues(request);

            if (ContainsSpecialChar(noNullRequest))
            {
                response.ContainsSpecialChar = true;
                response.Message = localizer["The Object Cannot contain Semi-Colons!"];
            }

            var @sponsor = ValidateRequest(noNullRequest);

            if (response.ContainsSpecialChar == false && response.IsValid)
            {
                dataGateway.Insert(@sponsor);
            }
            return response;
        }

        private Sponsor ValidateRequest(SponsorCreateRequest request)
        {
            if (request.NameOfSponsor == "")
            {
                response.Message += localizer["The Sponsor must have a name!"];
                response.IsValid = false;
            }

            var validatedSponsor = new Sponsor
            {
                Id = Guid.NewGuid().ToString(),
                NameOfSponsor = request.NameOfSponsor,
                Sponsorship = request.Sponsorship,
                Contract = request.Contract,
                ContactInformation = request.ContactInformation
            };
            validatedSponsor.Contract.RegistrationDate = validatedSponsor.Contract.RegistrationDate.AddHours(5);
            validatedSponsor.Contract.ExpirationDate = validatedSponsor.Contract.ExpirationDate.AddHours(5);
            validatedSponsor.Sponsorship.Date = validatedSponsor.Sponsorship.Date.AddHours(5);

            return validatedSponsor;
        }

        private static bool ContainsSpecialChar(object @event)
        {
            var eventString = JsonConvert.SerializeObject(@event);
            var containsSpecialChar = eventString.Contains(";");
            return containsSpecialChar;
        }

        private SponsorCreateRequest ChangeNullValues(SponsorCreateRequest request)
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
            foreach (var property in request.Contract.GetType().GetProperties())
            {
                var propertyType = property.PropertyType;
                if (propertyType != typeof(DateTime))
                {
                    var value = property.GetValue(request.Contract, null);
                    if (propertyType == typeof(string) && value == null)
                    {
                        property.SetValue(request.Contract, string.Empty);
                    }
                }
            }
            foreach (var property in request.Sponsorship.GetType().GetProperties())
            {
                var propertyType = property.PropertyType;
                if (propertyType != typeof(DateTime))
                {
                    var value = property.GetValue(request.Sponsorship, null);
                    if (propertyType == typeof(string) && value == null)
                    {
                        property.SetValue(request.Sponsorship, string.Empty);
                    }
                }
            }
            return request;
        }
    }

    public class SponsorCreateResponse
    {
        public string Message { get; set; }

        public bool IsValid { get; set; }

        public bool ContainsSpecialChar { get; set; }

        public SponsorCreateResponse(string message, bool containsSpecialChar, bool isValid)
        {
            Message = message;
            ContainsSpecialChar = containsSpecialChar;
            IsValid = isValid;
        }
    }

    public class SponsorCreateRequest
    {
        public string NameOfSponsor { get; set; }
        public Sponsorship Sponsorship { get; set; }

        public Contract Contract { get; set; }

        public ContactInformation ContactInformation { get; set; }
    }
}