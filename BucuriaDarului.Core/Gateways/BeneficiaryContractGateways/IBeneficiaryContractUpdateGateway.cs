using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways.BeneficiaryContractGateways
{
    public interface IBeneficiaryContractUpdateGateway
    {
        Beneficiary GetBeneficiary(string beneficiaryId);
        List<BeneficiaryContract> GetContractsOfBeneficiary();
        void AddBeneficiaryContractToModifiedList(string beforeEditingBeneficiaryContractString);
        public List<ModifiedIDs> ReturnModificationList();
        void UpdateBeneficiaryContract(BeneficiaryContract beneficiaryContract);
    }
}
