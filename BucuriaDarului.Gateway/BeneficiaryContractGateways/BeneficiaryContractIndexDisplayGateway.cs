using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryContractGateways;
using BucuriaDarului.Gateway.BeneficiaryGateways;
using System.Collections.Generic;

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