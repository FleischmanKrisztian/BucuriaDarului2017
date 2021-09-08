using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.VolunteerContractGateways
{
    public interface IListDisplayVolunteerContractsGateway
    {
        List<VolunteerContract> GetListVolunteerContracts();
        List<AdditionalContractVolunteer> GetListAdditionalContracts();
    }
}