using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.BeneficiaryContractGateways
{
    public interface IListDisplayBeneficiaryContractsGateway
    {
        List<BeneficiaryContract> GetListBeneficiaryContracts();
    }
}