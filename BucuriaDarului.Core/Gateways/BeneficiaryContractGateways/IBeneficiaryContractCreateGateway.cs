namespace BucuriaDarului.Core.Gateways.BeneficiaryContractGateways
{
    public interface IBeneficiaryContractCreateGateway
    {
        void Insert(BeneficiaryContract beneficiaryContract);

        Beneficiary GetBeneficiary(string idOfBeneficiary);
    }
}