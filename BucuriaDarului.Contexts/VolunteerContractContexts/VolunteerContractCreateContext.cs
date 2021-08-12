using System;
using System.ComponentModel.DataAnnotations;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using Microsoft.Extensions.Localization;

namespace BucuriaDarului.Contexts.VolunteerContractContexts
{
    public class VolunteerContractCreateContext
    {
        private readonly IVolunteerContractCreateGateway dataGateway;
        private VolunteerContractCreateResponse response = new VolunteerContractCreateResponse("", true);
        private readonly IStringLocalizer localizer;

        public VolunteerContractCreateContext(IVolunteerContractCreateGateway dataGateway, IStringLocalizer localizer)
        {
            this.dataGateway = dataGateway;
            this.localizer = localizer;
        }

        public VolunteerContractCreateResponse Execute(VolunteerContractCreateRequest request)
        {
          
            var contractToCreate = CreateContract(request);
            if (response.IsValid)
                dataGateway.Insert(contractToCreate);
            return response;
        }

        private static VolunteerContractCreateRequest ChangeNullValues(VolunteerContractCreateRequest request)
        {
            foreach (var property in request.GetType().GetProperties())
            {
                var propertyType = property.PropertyType;
                if (propertyType != typeof(DateTime))
                {
                    var value = property.GetValue(request, null);
                    if (propertyType == typeof(string) && value == null)
                    {
                        property.SetValue(request, string.Empty);
                    }
                }
            }

            return request;
        }

        private VolunteerContract CreateContract(VolunteerContractCreateRequest request)
        {
            request = ChangeNullValues(request);
            var contract = new VolunteerContract();
            var volunteer = dataGateway.GetVolunteer(request.OwnerId);
            contract.Id = Guid.NewGuid().ToString();
            if (request.NumberOfRegistration == "")
            {
                response.IsValid = false;
                response.Message += @localizer["Please enter number of registration!"];
            }
            else
                contract.NumberOfRegistration = request.NumberOfRegistration;
            if (request.ExpirationDate < DateTime.MinValue.AddYears(3))
            {
                response.IsValid = false;
                response.Message += @localizer["Please enter a valid Expiration Date!"];
            }
            else
                contract.ExpirationDate = request.ExpirationDate.AddDays(1);
            if (request.RegistrationDate < DateTime.MinValue.AddYears(3))
            {
                response.IsValid = false;
                response.Message += @localizer["Please enter a valid Registration Date!"];
            }
            else
                contract.RegistrationDate = request.RegistrationDate.AddDays(1);
            contract.Birthdate = volunteer.Birthdate;
            contract.Fullname = volunteer.Fullname;
            if (volunteer.CNP != "")
                contract.CNP = volunteer.CNP;
            else
            {
                response.IsValid = false;
                response.Message += @localizer["Missing CNP information for this volunteer! Please fill in all the data necessary for contract creation."];
            }
            //CI will never be null 
            if (volunteer.CI != null)
                contract.CI = volunteer.CI;
            else
            {
                response.IsValid = false;
                response.Message += @localizer["Missing CI information for this volunteer!Please fill in all the data necessary for contract creation."];
            }
            contract.PhoneNumber = volunteer.ContactInformation.PhoneNumber;
            contract.HourCount = volunteer.HourCount;
            contract.Address = volunteer.Address;
            contract.OwnerID = volunteer.Id;

            return contract;
        }
    }

    public class VolunteerContractCreateResponse
    {
        public string Message { get; set; }

        public bool IsValid { get; set; }

        public VolunteerContractCreateResponse(string message, bool isValid)
        {
            Message = message;
            IsValid = isValid;
        }
    }

    public class VolunteerContractCreateRequest
    {
        public string OwnerId { get; set; }

        public string NumberOfRegistration { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}