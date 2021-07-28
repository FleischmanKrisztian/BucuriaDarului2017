using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using BucuriaDarului.Gateway.VolunteerGateways;
using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Gateway.VolunteerContractGateways
{
    public class VolunteerContractPrintGateway : IVolunteerContractPrintGateway
    {
       

        public VolunteerContract GetVolunteerContract(string idContract)
        {
            return SingleVolunteerContractReturnerGateway.GetVolunteerContract(idContract);
        }
    }
}
