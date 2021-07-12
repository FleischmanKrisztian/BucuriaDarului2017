using BucuriaDarului.Gateway.BeneficiaryContractGateways;
using Newtonsoft.Json;

namespace BucuriaDarului.Contexts.BeneficiaryContractContexts
{
    public class BeneficiaryContractApiContext
    {
        public string Execute(string id)
        {
            var beneficiaryContract = SingleBeneficiaryContractReturnerGateway.GetBeneficiaryContract(id);
            var jsonString = JsonConvert.SerializeObject(beneficiaryContract);
            return jsonString;
        }
    }
}