using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.VolunteerContractGateways
{
    public interface IVolunteerAdditionalContractEditGateway
    {
        public void AddVAdditionalContractToModifiedList(string beforeEditingAdditionalContract);

        public void Update(AdditionalContractVolunteer additionalContract);

        List<AdditionalContractVolunteer> GetListOfAdditionalContracts(string id);
    }
}