using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryContractGateways;
using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Gateway.BeneficiaryContractGateways
{
    public class BeneficiaryContractPrintGateway : IBeneficiaryContractPrintGateway
    {
        public BeneficiaryContract GetBeneficiaryContract(string beneficiaryContractId)
        {
            return SingleBeneficiaryContractReturnerGateway.GetBeneficiaryContract(beneficiaryContractId);
        }
    }
}
