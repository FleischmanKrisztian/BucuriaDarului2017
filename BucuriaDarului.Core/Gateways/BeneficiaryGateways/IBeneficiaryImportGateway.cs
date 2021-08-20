using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.BeneficiaryGateways
{
    public interface IBeneficiaryImportGateway
    {
        void Insert(List<Beneficiary> beneficiaries);

        void Update(List<Beneficiary> beneficiaries);

        void InsertBeneficiaryContracts(List<BeneficiaryContract> contracts);
        List<Beneficiary> GetBenficiariesList();
    }
}