using BucuriaDarului.Core.Gateways.BeneficiaryContractGateways;

namespace BucuriaDarului.Contexts.BeneficiaryContractContexts
{
    public class BeneficiaryContractDeleteContext
    {
        private readonly IBeneficiaryContractDeleteGateway dataGateway;

        public BeneficiaryContractDeleteContext(IBeneficiaryContractDeleteGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public string Execute(string contractId, string ownerId)
        {
            dataGateway.Delete(contractId);
            var contracts = dataGateway.GetListBeneficiaryContracts();
            if (contracts.Find(x => x.Id == contractId) != null)
                return "There was an Error Deleting Contract! ";
            return "Contract Deleted Successfully! ";
        }
    }
}