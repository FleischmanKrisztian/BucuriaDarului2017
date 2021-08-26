using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;

namespace BucuriaDarului.Gateway.VolunteerContractGateways
{
    public class VolunteerContractPrintGateway : IVolunteerContractPrintGateway
    {
        public VolunteerContract GetVolunteerContract(string idContract)
        {
            return SingleVolunteerContractReturnerGateway.GetVolunteerContract(idContract);
        }
    }
}