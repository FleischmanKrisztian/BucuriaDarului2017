using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.VolunteerContractGateways
{
    public interface IVolunteerAdditionalContractDeleteGateway
    {
        public void Delete(string id);

        List<AdditionalContractVolunteer> GetListAdditionalContracts();
    }
}