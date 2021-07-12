using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways.VolunteerContractGateways
{
    public interface  IVolunteerContractDeleteGateway
    {
        public void Delete(string id);

        List<VolunteerContract> GetListVolunteerContracts();
    }
}
