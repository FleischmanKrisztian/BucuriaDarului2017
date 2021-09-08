namespace BucuriaDarului.Core.Gateways.VolunteerContractGateways
{
    public interface IVolunteerAdditionalContractCreateGateway
    {
        void Insert(AdditionalContractVolunteer additionalContract);

       VolunteerContract GetContract(string id);
        Volunteer GetVolunteer(string idOfVolunteer);
    }
}