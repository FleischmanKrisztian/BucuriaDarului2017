using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways.BeneficiaryGateways
{
    public interface IBeneficiaryDeleteGateway
    {
        public void DeleteBeneficiary(string id);
        public Beneficiary GetVolunteer(string id);

        public void UpdateBeneficiary(string beneficiaryId, Beneficiary beneficiaryToUpdate);
    }
}
