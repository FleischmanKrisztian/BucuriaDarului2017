using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways.BeneficiaryGateways
{
    public interface IBeneficiariesMainDisplayIndexGateway
    {
        List<Beneficiary> GetListOfBeneficiaries();
    }
}
