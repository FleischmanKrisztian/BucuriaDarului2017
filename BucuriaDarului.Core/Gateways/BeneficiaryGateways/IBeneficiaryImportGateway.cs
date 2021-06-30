using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways
{
    public interface IBeneficiaryImportGateway
    {
        void Insert(List<Beneficiary> beneficiaries);
    }
}