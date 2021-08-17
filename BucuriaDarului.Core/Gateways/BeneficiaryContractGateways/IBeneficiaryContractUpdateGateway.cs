using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.BeneficiaryContractGateways
{
    public interface IBeneficiaryContractUpdateGateway
    {
        public void AddBeneficiaryContractToModifiedList(string beforeEditingBeneficiaryContract);

        public void Update(BeneficiaryContract contract);

        List<BeneficiaryContract> GetListOfBeneficiaryContracts(string id);
    }
}