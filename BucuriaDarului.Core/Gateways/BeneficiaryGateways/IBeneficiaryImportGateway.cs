using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.BeneficiaryGateways
{
    public interface IBeneficiaryImportGateway
    {
        void Insert(List<Beneficiary> beneficiaries);
    }
}