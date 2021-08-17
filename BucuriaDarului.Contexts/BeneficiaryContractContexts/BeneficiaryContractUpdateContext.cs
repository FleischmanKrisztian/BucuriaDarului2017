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

        public void Execute(string beneficiaryId)
        {
            var beneficiary = dataGateway.GetBeneficiary(beneficiaryId);
            var contracts = dataGateway.GetContractsOfBeneficiary();

            foreach (var c in contracts)
            { var updatedContract = GetUpdateContract(beneficiary, c);

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
}