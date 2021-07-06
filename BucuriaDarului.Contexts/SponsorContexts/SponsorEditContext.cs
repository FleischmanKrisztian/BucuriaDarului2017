using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.SponsorGateways;
using Newtonsoft.Json;

namespace BucuriaDarului.Contexts.SponsorContexts
{
    public class SponsorEditContext
    {
        private readonly ISponsorEditGateway dataGateway;
        private SponsorEditResponse response = new SponsorEditResponse("", false, true);

        public SponsorEditContext(ISponsorEditGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public SponsorEditResponse Execute(SponsorEditRequest request)
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
                var modifiedList = dataGateway.ReturnModificationList();
                var modifiedListString = JsonConvert.SerializeObject(modifiedList);
                if (!modifiedListString.Contains(@sponsor._id))
                {
                    var beforeEditingSponsor = dataGateway.ReturnSponsor(@sponsor._id);
                    var beforeEditingSponsorString = JsonConvert.SerializeObject(beforeEditingSponsor);
                    dataGateway.AddSponsorToModifiedList(beforeEditingSponsorString);
                }
                dataGateway.Edit(@sponsor);
            }
            response.Sponsor = @sponsor;

            return response;
        }

        private Sponsor ValidateRequest(SponsorEditRequest request)
        {
            if (request.NameOfSponsor == "")
            {
                response.Message += "The Event must have a name! ";
                response.IsValid = false;
            }

            var validatedSponsor = new Sponsor
            {
                _id = request._id,
                NameOfSponsor = request.NameOfSponsor,
                Sponsorship = request.Sponsorship,
                Contract = request.Contract,
                ContactInformation = request.ContactInformation
            };

            return validatedSponsor;
        }

        private bool ContainsSpecialChar(object @event)
        {
            var eventString = JsonConvert.SerializeObject(@event);
            var containsSpecialChar = eventString.Contains(";");
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
            return request;
        }
    }

    public class SponsorEditResponse
    {
        public Sponsor Sponsor { get; set; }
        public string Message { get; set; }

        public bool IsValid { get; set; }

        public bool ContainsSpecialChar { get; set; }

        public SponsorEditResponse(string message, bool containsSpecialChar, bool isValid)
        {
            Message = message;
            ContainsSpecialChar = containsSpecialChar;
            IsValid = isValid;
        }
    }

    public class SponsorEditRequest
    {
        public string _id { get; set; }
        public string NameOfSponsor { get; set; }
        public Sponsorship Sponsorship { get; set; }

        public Contract Contract { get; set; }

        public ContactInformation ContactInformation { get; set; }
    }
}