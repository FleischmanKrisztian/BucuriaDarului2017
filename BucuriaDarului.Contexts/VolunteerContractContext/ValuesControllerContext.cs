using BucuriaDarului.Gateway.VolContractGateways;
using BucuriaDarului.Gateway.VolunteerGateways;
using Newtonsoft.Json;


namespace BucuriaDarului.Contexts.VolunteerContractContext
{
    public class ValuesControllerContext 
    {
        public string Execute( string id)
        {
            string jsonstring;
            var volcontract = SingleVolunteerContractReturnerGateway.GetVolunteerContract(id);
            jsonstring = JsonConvert.SerializeObject(volcontract);
            return jsonstring;
        }
    }
}
