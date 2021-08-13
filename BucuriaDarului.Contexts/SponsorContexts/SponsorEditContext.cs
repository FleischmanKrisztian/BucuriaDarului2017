using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.SponsorGateways;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace BucuriaDarului.Contexts.SponsorContexts
{
    public class SponsorEditContext
    {
        private readonly ISponsorEditGateway dataGateway;
        private SponsorEditResponse response = new SponsorEditResponse("", true);
        public SponsorEditContext(ISponsorEditGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public SponsorEditResponse Execute(SponsorEditRequest request)
        {
            var noNullRequest = ChangeNullValues(request);

            if (ContainsSpecialChar(noNullRequest))
            {
                response.Message = "The Object Cannot contain Semi-Colons!";
            }

            var sponsor = ValidateRequest(noNullRequest);

            if (response.IsValid)
            {
                var modifiedList = dataGateway.ReturnModificationList();
                var modifiedListString = JsonConvert.SerializeObject(modifiedList);
                if (!modifiedListString.Contains(sponsor.Id))
                {
                    var beforeEditingSponsor = dataGateway.ReturnSponsor(sponsor.Id);
                    var beforeEditingSponsorString = JsonConvert.SerializeObject(beforeEditingSponsor);
                    dataGateway.AddSponsorToModifiedList(beforeEditingSponsorString);
                }
                dataGateway.Edit(sponsor);
            }
            response.Sponsor = sponsor;

            return response;
        }

        private Sponsor ValidateRequest(SponsorEditRequest request)
        {
            if (request.NameOfSponsor == "")
            {
                response.Message += "The Sponsor must have a name!";
                response.IsValid = false;
            }
            request.Contract.RegistrationDate = request.Contract.RegistrationDate.AddHours(5);
            request.Contract.ExpirationDate = request.Contract.ExpirationDate.AddHours(5);
            request.Sponsorship.Date = request.Sponsorship.Date.AddHours(5);
            var validatedSponsor = new Sponsor
            {
                Id = request.Id,
                NameOfSponsor = request.NameOfSponsor,
                Sponsorship = request.Sponsorship,
                Contract = request.Contract,
                ContactInformation = request.ContactInformation
        };

            return validatedSponsor;
        }

        private bool ContainsSpecialChar(object sponsor)
        {
            var sponsorString = JsonConvert.SerializeObject(sponsor);
            var containsSpecialChar = sponsorString.Contains(";");
            return containsSpecialChar;
        }

        private static SponsorEditRequest ChangeNullValues(SponsorEditRequest request)
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

                var value = property.GetValue(request.ContactInformation, null);
                if (propertyType == typeof(string) && value == null)
                {
                    property.SetValue(request.ContactInformation, string.Empty);
                }
            }
            foreach (var property in request.Sponsorship.GetType().GetProperties())
            {
                var propertyType = property.PropertyType;

                var value = property.GetValue(request.Sponsorship, null);
                if (propertyType == typeof(string) && value == null)
                {
                    property.SetValue(request.Sponsorship, string.Empty);
                }
            }
            foreach (var property in request.Contract.GetType().GetProperties())
            {
                var propertyType = property.PropertyType;

                var value = property.GetValue(request.Contract, null);
                if (propertyType == typeof(string) && value == null)
                {
                    property.SetValue(request.Contract, string.Empty);
                }
            }
            return request;
        }
    }

    public class SponsorEditResponse
    {
        public Sponsor Sponsor { get; set; }
        public string Message { get; set; }

        public bool IsValid { get; set; }

        public SponsorEditResponse(string message, bool isValid)
        {
            Message = message;
            IsValid = isValid;
        }
    }

    public class SponsorEditRequest
    {
        public string Id { get; set; }
        public string NameOfSponsor { get; set; }
        public Sponsorship Sponsorship { get; set; }

        public Contract Contract { get; set; }

        public ContactInformation ContactInformation { get; set; }
    }
}