using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using System;

namespace BucuriaDarului.Contexts.VolunteerAdditionalContractContexts
{
    public class VolunteerAdditionalContractCreateContext
    {
        private readonly IVolunteerAdditionalContractCreateGateway dataGateway;
        private VolunteerAdditionalContractCreateResponse response = new VolunteerAdditionalContractCreateResponse("", true);

        public VolunteerAdditionalContractCreateContext(IVolunteerAdditionalContractCreateGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public VolunteerAdditionalContractCreateResponse Execute(VolunteerAdditionalContractCreateRequest request)
        {
            var additionalContractToCreate = CreateAdditionalContract(request);
            if (response.IsValid)
                dataGateway.Insert(additionalContractToCreate);
            return response;
        }

        private static VolunteerAdditionalContractCreateRequest ChangeNullValues(VolunteerAdditionalContractCreateRequest request)
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

        private AdditionalContractVolunteer CreateAdditionalContract(VolunteerAdditionalContractCreateRequest request)
        {
            request = ChangeNullValues(request);
            var contract = new AdditionalContractVolunteer();
            var volunteerContract = dataGateway.GetContract(request.ContractID);
            contract.Id = Guid.NewGuid().ToString();
            if (request.AdditionalContractNumberOfRegistration == "")
            {
                response.IsValid = false;
                response.Message += "Please enter number of registration!";
            }
            else
                contract.AdditionalContractNumberOfRegistration = request.AdditionalContractNumberOfRegistration;
            if (request.CreationDate < DateTime.MinValue.AddYears(3))
            {
                response.IsValid = false;
                response.Message += "Please enter a valid Creation Date!";
            }
            else
                contract.CreationDate = request.CreationDate;
            if (request.ExpirationDate < DateTime.MinValue.AddYears(3))
            {
                response.IsValid = false;
                response.Message += "Please enter a valid Expiration Date!";
            }
            else
                contract.ExpirationDate = request.ExpirationDate.AddDays(1);
            if (request.RegistrationDate < DateTime.MinValue.AddYears(3))
            {
                response.IsValid = false;
                response.Message += "Please enter a valid Registration Date!";
            }
            else
                contract.RegistrationDate = request.RegistrationDate.AddDays(1);

                contract.Birthdate = volunteerContract.Birthdate;
            contract.Fullname = volunteerContract.Fullname;
            if (volunteerContract.CNP != "")
                contract.CNP = volunteerContract.CNP;
            else
            {
                response.IsValid = false;
                response.Message += "Missing CNP information for this volunteer! Please fill in all the data necessary for contract creation.";
            }
            //CI will never be null
            if (volunteerContract.CI != null)
                contract.CI = volunteerContract.CI;
            else
            {
                response.IsValid = false;
                response.Message += "Missing CI information for this volunteer!Please fill in all the data necessary for contract creation.";
            }
            contract.PhoneNumber = volunteerContract.PhoneNumber;
            contract.HourCount = volunteerContract.HourCount;
            contract.Address = volunteerContract.Address;
            contract.ContractID = volunteerContract.Id;
            contract.ContractNumberOfRegistration = volunteerContract.NumberOfRegistration;
            contract.OwnerID = volunteerContract.OwnerID;

            return contract;
        }
    }

    public class VolunteerAdditionalContractCreateResponse
    {
        public string Message { get; set; }

        public bool IsValid { get; set; }

        public VolunteerAdditionalContractCreateResponse(string message, bool isValid)
        {
            Message = message;
            IsValid = isValid;
        }
    }

    public class VolunteerAdditionalContractCreateRequest
    {
        public string OwnerId { get; set; }
        public string ContractID { get; set; }
        public string NumberOfRegistration { get; set; }
        public string AdditionalContractNumberOfRegistration { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}