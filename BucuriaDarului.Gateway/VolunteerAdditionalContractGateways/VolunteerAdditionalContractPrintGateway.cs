using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;

namespace BucuriaDarului.Gateway.VolunteerContractGateways
{
    public class VolunteerAdditionalContractPrintGateway : IVolunteerAdditionalContractPrintGateway
    {
        public AdditionalContractVolunteer GetAdditionalContract(string id)
        {
            return SingleVolunteerAdditionalContractReturnerGateway.GetAdditionalContract(id);
        }

        public VolunteerContract GetVolunteerContract(string idContract)
        {
            return SingleVolunteerContractReturnerGateway.GetVolunteerContract(idContract);
        }
    }
}