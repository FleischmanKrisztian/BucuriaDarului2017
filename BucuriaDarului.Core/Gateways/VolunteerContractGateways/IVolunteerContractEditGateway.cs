using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.VolunteerContractGateways
{
    public interface IVolunteerContractEditGateway
    {
        public void AddVolunteerContractToModifiedList(string beforeEditingVolunteerContract);

        public void Update(VolunteerContract contract);

        List<VolunteerContract> GetListOfVolunteersContracts(string id);
    }
}