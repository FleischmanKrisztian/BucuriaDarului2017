using BucuriaDarului.Core;
using System.Collections.Generic;
using BucuriaDarului.Core.Gateways.BeneficiaryContractGateways;

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