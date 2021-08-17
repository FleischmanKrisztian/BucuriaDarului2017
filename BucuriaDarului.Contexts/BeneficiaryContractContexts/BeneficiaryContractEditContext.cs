using BucuriaDarului.Contexts.BeneficiaryContexts;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryContractGateways;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BucuriaDarului.Contexts.BeneficiaryContractContexts
{
    public class BeneficiaryContractEditContext
    {
        private readonly IBeneficiaryContractUpdateGateway dataGateway;
        public BeneficiaryContractEditResponse response = new BeneficiaryContractEditResponse("", true);

        public BeneficiaryContractEditContext(IBeneficiaryContractUpdateGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public BeneficiaryContractEditResponse Execute(BeneficiaryEditRequest request)
        {
            var listOfContracts = dataGateway.GetListOfBeneficiaryContracts(request.Id);
            if (listOfContracts.Count > 0)
            {
                CreateAuxiliaryFiles(listOfContracts);
                UpdateContracts(listOfContracts, request);
            }
            return response;
        }

        private void CreateAuxiliaryFiles(List<BeneficiaryContract> listOfContracts)
        {
            foreach (var contract in listOfContracts)
            {
                var beforeEditingBeneficiaryContractString = JsonConvert.SerializeObject(contract);
                dataGateway.AddBeneficiaryContractToModifiedList(beforeEditingBeneficiaryContractString);
            }
        }

        private void UpdateContracts(List<BeneficiaryContract> listOfContracts, BeneficiaryEditRequest request)
        {
            try
            {
                foreach (var contract in listOfContracts)
                {
                    var beforeEditingBeneficiaryContractString = JsonConvert.SerializeObject(contract);
                    contract.Address = request.Address;
                    contract.Birthdate = request.PersonalInfo.Birthdate;
                    contract.CIinfo = request.CI.Info;
                    contract.CNP = request.CNP;
                    contract.Fullname = request.Fullname;
                    contract.IdApplication = request.Marca.IdApplication;
                    contract.IdInvestigation = request.Marca.IdInvestigation;
                    contract.NumberOfPortions = request.NumberOfPortions;
                    contract.PhoneNumber = request.PersonalInfo.PhoneNumber;

                    dataGateway.Update(contract);
                }
            }
            catch
            {
                response.Message = "There was an Error Updating the Beneficiarys Contracts. Please Try again!";
                response.IsValid = false;
            }
        }
    }

    public class BeneficiaryContractEditResponse
    {
        public string Message { get; set; }

        public bool IsValid { get; set; }

        public BeneficiaryContractEditResponse(string message, bool isValid)
        {
            Message = message;
            IsValid = isValid;
        }
    }
}