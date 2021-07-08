using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryGateways;
using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Contexts.BeneficiaryContexts
{
   public  class BeneficiaryDeleteContext
    {
        private readonly IBeneficiaryDeleteGateway dataGateway;

        public BeneficiaryDeleteContext(IBeneficiaryDeleteGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }
        public void Execute(bool inactive, string id)
        {


            if (inactive == false)
            {
                dataGateway.DeleteBeneficiary(id);
                //beneficiarycontractManager.DeleteBeneficiaryContracts(id);

            }
            else
            {
                Beneficiary beneficiary = dataGateway.GetBeneficiary(id);
                beneficiary.Active= false;
                var beneficiaryToUpdate = beneficiary;
                dataGateway.UpdateBeneficiary(id, beneficiaryToUpdate);

            }
        }

    }
}
