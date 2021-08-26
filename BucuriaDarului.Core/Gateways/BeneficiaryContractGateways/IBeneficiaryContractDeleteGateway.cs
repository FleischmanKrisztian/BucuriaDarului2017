using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.BeneficiaryContractGateways
{
    public interface IBeneficiaryContractDeleteGateway
    {
        public void Delete(string id);

        List<BeneficiaryContract> GetListBeneficiaryContracts();
    }
}