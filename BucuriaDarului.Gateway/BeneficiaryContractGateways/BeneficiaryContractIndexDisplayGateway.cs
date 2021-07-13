using BucuriaDarului.Core;
using BucuriaDarului.Gateway.BeneficiaryGateways;
using System.Collections.Generic;
using BucuriaDarului.Core.Gateways.BeneficiaryContractGateways;

namespace BucuriaDarului.Gateway.BeneficiaryContractGateways
{
    public class BeneficiaryContractIndexDisplayGateway : IBeneficiaryContractMainDisplayGateway
    {
        public List<BeneficiaryContract> GetListBeneficiaryContracts()
        {
            var contracts = ListBeneficiaryContractGateway.GetListBeneficiaryContracts();
            return contracts;
        }

        public Beneficiary GetBeneficiary(string idOfBeneficiary)
        {
            return SingleBeneficiaryReturnerGateway.ReturnBeneficiary(idOfBeneficiary);
        }
    }
}