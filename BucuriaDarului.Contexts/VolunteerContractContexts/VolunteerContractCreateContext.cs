using System;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;

namespace BucuriaDarului.Contexts.VolunteerContractContexts
{
    public class VolunteerContractCreateContext
    {
        private readonly IVolunteerContractCreateGateway dataGateway;
        private VolunteerContractCreateResponse response = new VolunteerContractCreateResponse("", true);

        public VolunteerContractCreateContext(IVolunteerContractCreateGateway dataGateway)
        {
            this.dataGateway = dataGateway;
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
            var volunteer = dataGateway.GetVolunteer(request.OwnerID);
            contract.NumberOfRegistration = request.NumberOfRegistration;
            contract.Id = Guid.NewGuid().ToString();
            contract.ExpirationDate = contract.ExpirationDate.AddDays(1);
            contract.RegistrationDate = contract.RegistrationDate.AddDays(1);
            if (volunteer.Birthdate != null)
            { contract.Birthdate = volunteer.Birthdate; }
            else
            {
                response.IsValid = false;
                response.Message+= "Missing birthday information for this volunteer!Please fill in all the data necessary for contract creation.";
            }
            contract.Fullname = volunteer.Fullname;
            if (volunteer.CNP != "")
                contract.CNP = volunteer.CNP;
            else
            {
                response.IsValid = false;
                response.Message += "Missing CNP information for this volunteer!Please fill in all the data necessary for contract creation.";
            }
            if (volunteer.CI != null)
                contract.CI = volunteer.CI;
            else
            {
                response.IsValid = false;
                response.Message += "Missing CI information for this volunteer!Please fill in all the data necessary for contract creation.";
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
        public string OwnerID { get; set; }

        public string Fullname { get; set; }

        public string CNP { get; set; }

        public string Address { get; set; }

        public int HourCount { get; set; }

        public string PhoneNumber { get; set; }

        public CI CI { get; set; }

        public DateTime Birthdate { get; set; }

        public string NumberOfRegistration { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime ExpirationDate { get; set; }
    }


}