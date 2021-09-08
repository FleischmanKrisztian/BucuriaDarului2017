using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.VolunteerContractGateways
{
    public interface IListDisplayVolunteerAdditionalContractsGateway
    {
        List<AdditionalContractVolunteer> GetListAdditionalContracts();
    }
}