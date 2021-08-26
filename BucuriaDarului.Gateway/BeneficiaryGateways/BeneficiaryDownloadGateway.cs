using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryGateways;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.BeneficiaryGateways
{
    public class BeneficiaryDownloadGateway : IBeneficiaryDownloadGateway
    {
        public List<Beneficiary> GetListOfBeneficiaries()
        {
            return ListBeneficiariesGateway.GetListOfBeneficiaries();
        }
    }
}