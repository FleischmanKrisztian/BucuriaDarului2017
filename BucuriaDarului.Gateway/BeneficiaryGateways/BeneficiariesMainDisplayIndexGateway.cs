using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryGateways;
using BucuriaDarului.Gateway.BeneficiaryContractGateways;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.BeneficiaryGateways
{
    public class BeneficiariesMainDisplayIndexGateway : IBeneficiariesMainDisplayIndexGateway
    {
        public List<BeneficiaryContract> GetContractList()
        {
            return ListBeneficiaryContractGateway.GetListBeneficiaryContracts();
        }

        public List<Beneficiary> GetListOfBeneficiaries()
        {
            return ListBeneficiariesGateway.GetListOfBeneficiaries();
        }
    }
}