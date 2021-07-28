using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways.VolunteerContractGateways
{
    public interface IVolunteerContractPrintGateway
    {
        VolunteerContract GetVolunteerContract(string idContract);
    }
}
