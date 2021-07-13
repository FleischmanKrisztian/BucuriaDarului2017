namespace BucuriaDarului.Core.Gateways.BeneficiaryGateways
{
    public interface IBeneficiaryDeleteGateway
    {
        public void DeleteBeneficiary(string id);

        public Beneficiary GetBeneficiary(string id);

        public void UpdateBeneficiary(string beneficiaryId, Beneficiary beneficiaryToUpdate);

        void DeleteBeneficiaryContracts(string id);
    }
}