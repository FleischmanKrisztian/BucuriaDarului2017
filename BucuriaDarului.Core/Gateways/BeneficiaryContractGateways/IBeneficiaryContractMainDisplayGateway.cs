using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.BeneficiaryContractGateways
{
    public interface IBeneficiaryContractMainDisplayGateway
    {
        List<BeneficiaryContract> GetListBeneficiaryContracts();

        Beneficiary GetBeneficiary(string id);
    }
}