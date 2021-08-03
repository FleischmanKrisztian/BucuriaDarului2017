using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways.BeneficiaryContractGateways
{
    public interface IBeneficiaryContractPrintGateway
    {
        BeneficiaryContract GetBeneficiaryContract(string beneficiaryContractId);
    }
}
