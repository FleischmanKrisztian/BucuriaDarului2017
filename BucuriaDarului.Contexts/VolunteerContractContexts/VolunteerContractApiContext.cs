using BucuriaDarului.Gateway.VolunteerContractGateways;
using Newtonsoft.Json;

namespace BucuriaDarului.Contexts.VolunteerContractContexts
{
    public class VolunteerContractApiContext
    {
        public string Execute(string id)
        {
            var volContract = SingleVolunteerContractReturnerGateway.GetVolunteerContract(id);
            var jsonString = JsonConvert.SerializeObject(volContract);
            return jsonString;
        }
    }
}