namespace BucuriaDarului.Core.Gateways.VolunteerContractGateways
{
    public interface IVolunteerAdditionalContractPrintGateway
    {
        VolunteerContract GetVolunteerContract(string idContract);
        AdditionalContractVolunteer GetAdditionalContract(string id);
    }
}