using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.BeneficiaryGateways
{
    public interface IBeneficiariesMainDisplayIndexGateway
    {
        List<Beneficiary> GetListOfBeneficiaries();

        List<BeneficiaryContract> GetContractList();
    }
}