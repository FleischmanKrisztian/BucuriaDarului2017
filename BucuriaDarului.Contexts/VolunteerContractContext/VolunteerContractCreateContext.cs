using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using System;
using System.Collections.Generic;

namespace BucuriaDarului.Contexts.VolunteerContractContext
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
            var volunteer = dataGateway.GetVolunteer(request.VolunteerId);
            request = ChangeNullValues(request);
            var contract = CreateContract(request.Contract, volunteer);
            dataGateway.Insert(contract);
            return response;
        }
        private static VolunteerContractCreateRequest ChangeNullValues(VolunteerContractCreateRequest request)
        {
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

            return request;
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
                response.Message.Add( "Missig birthday information for this volunteer!Please fill in all the data necessary for contract creation.");
            }
            contract.Fullname = volunteer.Fullname;
            if (volunteer.CNP != null)
                contract.CNP = volunteer.CNP;
            else
            {
                response.IsValid = false;
                response.Message.Add("Missig CNP information for this volunteer!Please fill in all the data necessary for contract creation.");
            }
            if (volunteer.CI != null)
                contract.CI = volunteer.CI;
            else
            {
                response.IsValid = false;
                response.Message.Add("Missig CI information for this volunteer!Please fill in all the data necessary for contract creation.");
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
        public List<string> Message { get; set; }

        public bool IsValid { get; set; }

        public VolunteerContractCreateResponse(string message, bool isValid)
        {
            var Message = new List<string>();
            if (message != "")
                Message.Add(message);
            IsValid = isValid;
        }
    }

    public class VolunteerContractCreateRequest
    {
        public string VolunteerId { get; set; }
        public VolunteerContract Contract { get; set; }

        public VolunteerContractCreateRequest(string volunteerId, VolunteerContract contract)
        {
            VolunteerId = volunteerId;
            Contract = contract;
        }
    }
}