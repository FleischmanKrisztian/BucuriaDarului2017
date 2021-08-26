using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.BeneficiaryGateways
{
    public interface IBeneficiaryDownloadGateway
    {
        List<Beneficiary> GetListOfBeneficiaries();
    }
}