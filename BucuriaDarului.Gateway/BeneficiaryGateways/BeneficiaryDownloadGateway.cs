using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryGateways;
using System;
using System.Collections.Generic;
using System.Text;

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
