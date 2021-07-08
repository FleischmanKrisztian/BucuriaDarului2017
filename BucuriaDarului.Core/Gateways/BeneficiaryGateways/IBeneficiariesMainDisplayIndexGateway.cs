using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.BeneficiaryGateways
{
    public interface IBeneficiaryMainDisplayIndexGateway
    {
        List<Beneficiary> GetListOfBeneficiaries();
    }
}