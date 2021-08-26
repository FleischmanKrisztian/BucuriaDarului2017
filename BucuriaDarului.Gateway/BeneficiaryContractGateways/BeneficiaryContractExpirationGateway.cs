using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryContractGateways;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.BeneficiaryContractGateways
{
    public class BeneficiaryContractExpirationGateway : IListDisplayBeneficiaryContractsGateway
    {
        public List<BeneficiaryContract> GetListBeneficiaryContracts()
        {
            var contracts = ListBeneficiaryContractGateway.GetListBeneficiaryContracts();
            return contracts;
        }
    }
}