using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.VolunteerContractGateways
{
    public interface IVolunteerContractDeleteGateway
    {
        public void Delete(string id);

        void DeleteAdditionalContracts(string id);

        List<VolunteerContract> GetListVolunteerContracts();
    }
}