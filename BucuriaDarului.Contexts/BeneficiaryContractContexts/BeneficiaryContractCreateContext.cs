using System;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryContractGateways;

namespace BucuriaDarului.Contexts.BeneficiaryContractContexts
{
    public class BeneficiaryContractCreateContext
    {
        private readonly IBeneficiaryContractCreateGateway dataGateway;
        private BeneficiaryContractCreateResponse response = new BeneficiaryContractCreateResponse("", true);

        public BeneficiaryContractCreateContext(IBeneficiaryContractCreateGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public BeneficiaryContractCreateResponse Execute(BeneficiaryContractCreateRequest request)
        {

            var contractToCreate = CreateContract(request);
            if (response.IsValid)
                dataGateway.Insert(contractToCreate);
            return response;
        }

        private static BeneficiaryContractCreateRequest ChangeNullValues(BeneficiaryContractCreateRequest request)
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

        private BeneficiaryContract CreateContract(BeneficiaryContractCreateRequest request)
        {
            request = ChangeNullValues(request);
            var contract = new BeneficiaryContract();
            var beneficiary = dataGateway.GetBeneficiary(request.OwnerID);
            contract.Id = Guid.NewGuid().ToString();
            contract.OwnerID = request.OwnerID;
            contract.NumberOfRegistration = request.NumberOfRegistration;
            contract.ExpirationDate = request.ExpirationDate.AddDays(1);
            contract.RegistrationDate = request.RegistrationDate.AddDays(1);
            contract.Fullname = beneficiary.Fullname;
            contract.NumberOfPortions = beneficiary.NumberOfPortions;
            if (beneficiary.CNP != "")
                contract.CNP = beneficiary.CNP;
            else
            {
                response.IsValid = false;
                response.Message += "Missing CNP information for this beneficiary!Please fill in all the data necessary for contract creation.";
            }

            // Validation Must be added here if We choose to.
            contract.Address = beneficiary.Address;
            contract.PhoneNumber = beneficiary.PersonalInfo.PhoneNumber;
            contract.CIinfo = beneficiary.CI.Info;
            contract.Birthdate = beneficiary.PersonalInfo.Birthdate;
            contract.IdInvestigation = beneficiary.Marca.IdInvestigation;
            contract.IdApplication = beneficiary.Marca.IdApplication;

            return contract;
        }
    }

    public class BeneficiaryContractCreateResponse
    {
        public string Message { get; set; }

        public bool IsValid { get; set; }

        public BeneficiaryContractCreateResponse(string message, bool isValid)
        {
            Message = message;
            IsValid = isValid;
        }
    }

    public class BeneficiaryContractCreateRequest
    {
        public string OwnerID { get; set; }

        public string NumberOfRegistration { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}