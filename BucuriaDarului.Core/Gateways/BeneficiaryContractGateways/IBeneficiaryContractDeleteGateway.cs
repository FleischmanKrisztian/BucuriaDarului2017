using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways.BeneficiaryContractGateways
{
    public interface IBeneficiaryContractDeleteGateway
    {
        public void Delete(string id);

        List<BeneficiaryContract> GetListBeneficiaryContracts();
    }
}
