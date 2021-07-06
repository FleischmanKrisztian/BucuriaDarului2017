using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.SponsorGateways;
using Newtonsoft.Json;
using System;

namespace BucuriaDarului.Contexts.SponsorContexts
{
    public class SponsorCreateContext
    {
        private readonly ISponsorCreateGateway dataGateway;
        private SponsorCreateResponse response = new SponsorCreateResponse("", false, true);

        public SponsorCreateContext(ISponsorCreateGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public SponsorCreateResponse Execute(SponsorCreateRequest request)
        {
            var noNullRequest = ChangeNullValues(request);

            if (ContainsSpecialChar(noNullRequest))
            {
                response.ContainsSpecialChar = true;
                response.Message = "The Object Cannot contain Semi-Colons! ";
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
                response.Message += "The Sponsor must have a name! ";
                response.IsValid = false;
            }

            var validatedSponsor = new Sponsor
            {
                _id = Guid.NewGuid().ToString(),
                NameOfSponsor = request.NameOfSponsor,
                Sponsorship = request.Sponsorship,
                Contract = request.Contract,
                ContactInformation = request.ContactInformation
            };

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