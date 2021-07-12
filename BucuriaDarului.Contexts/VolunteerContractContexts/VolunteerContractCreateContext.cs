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

        public VolunteerContractCreateResponse Execute(VolunteerContract contract, string volunteerId)
        {
            var volunteer = dataGateway.GetVolunteer(volunteerId);
            contract = ChangeNullValues(contract);
            var contractToCreate = CreateContract(contract, volunteer);
            if (response.IsValid)
                dataGateway.Insert(contractToCreate);
            return response;
        }

        private static VolunteerContract ChangeNullValues(VolunteerContract contract)
        {
            foreach (var property in contract.GetType().GetProperties())
            {
                var propertyType = property.PropertyType;
                if (propertyType != typeof(DateTime))
                {
                    var value = property.GetValue(contract, null);
                    if (propertyType == typeof(string) && value == null)
                    {
                        property.SetValue(contract, string.Empty);
                    }
                }
            }

            return contract;
        }

        private VolunteerContract CreateContract(VolunteerContract contract, Volunteer volunteer)
        {

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

    
}