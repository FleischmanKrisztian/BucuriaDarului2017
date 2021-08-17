using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryContractGateways;
using Newtonsoft.Json;

namespace BucuriaDarului.Contexts.BeneficiaryContractContexts
{
    public class BeneficiaryContractUpdateContext
    {
        private readonly IBeneficiaryContractUpdateGateway dataGateway;

        public BeneficiaryContractUpdateContext(IBeneficiaryContractUpdateGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public BeneficiaryContractEditResponse Execute(string beneficiaryId)
        {
            var response = new BeneficiaryContractEditResponse("",true);
            var beneficiary = dataGateway.GetBeneficiary(beneficiaryId);
            var listOfContracts = dataGateway.GetContractsOfBeneficiary();
            try
            {
                if (listOfContracts.Count > 0)
                {
                    foreach (var c in listOfContracts)
                    {
                        var updatedContract = GetUpdateContract(beneficiary, c);

                        var modifiedList = dataGateway.ReturnModificationList();
                        var modifiedListString = JsonConvert.SerializeObject(modifiedList);
                        if (!modifiedListString.Contains(updatedContract.Id))
                        {
                            var beforeEditingBeneficiaryContractString = JsonConvert.SerializeObject(c);
                            dataGateway.AddBeneficiaryContractToModifiedList(beforeEditingBeneficiaryContractString);
                        }
                        dataGateway.UpdateBeneficiaryContract(updatedContract);
                    }
                }
            }
            catch
            {
                response.Message = "There was an Error Updating the Volunteers Contracts. Please Try again!";
                response.IsValid = false;
            }

            return response;
        }

        public BeneficiaryContract GetUpdateContract(Beneficiary beneficiary, BeneficiaryContract beneficiaryContract)
        {
            beneficiaryContract.Fullname = beneficiary.Fullname;
            beneficiaryContract.Address = beneficiary.Address;
            beneficiaryContract.Birthdate = beneficiary.PersonalInfo.Birthdate;
            beneficiaryContract.CIinfo = beneficiary.CI.Info;
            beneficiaryContract.CNP = beneficiary.CNP;
            beneficiaryContract.IdApplication = beneficiary.Marca.IdApplication;
            beneficiaryContract.IdInvestigation = beneficiary.Marca.IdInvestigation;
            beneficiaryContract.NumberOfPortions = beneficiary.NumberOfPortions;
            beneficiaryContract.PhoneNumber = beneficiary.PersonalInfo.PhoneNumber;

            return beneficiaryContract;
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