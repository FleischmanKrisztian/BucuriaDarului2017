using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.BeneficiaryGateways
{
    public interface IBeneficiaryEditGateway
    {
        public void Edit(Beneficiary beneficiary);

        public List<ModifiedIDs> ReturnModificationList();

        public void AddBeneficiaryToModifiedList(string beforeEditingBeneficiaryString);

        public Beneficiary ReturnBeneficiary(string id);
    }
}