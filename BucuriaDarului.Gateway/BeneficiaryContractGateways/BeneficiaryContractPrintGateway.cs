using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryContractGateways;

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