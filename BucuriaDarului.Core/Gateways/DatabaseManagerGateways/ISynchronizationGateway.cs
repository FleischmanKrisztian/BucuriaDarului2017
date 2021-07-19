using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.DatabaseManagerGateways
{
    public interface ISynchronizationGateway
    {
        public List<Event> GetEventList(bool localConnection);

        public List<Sponsor> GetSponsorList(bool localConnection);

        public List<Volunteer> GetVolunteerList(bool localConnection);

        public List<Beneficiary> GetBeneficiaryList(bool localConnection);

        public List<BeneficiaryContract> GetBeneficiaryContractList(bool localConnection);

        public List<VolunteerContract> GetVolunteerContractList(bool localConnection);

        public List<ModifiedIDs> GetListOfModifications();

        public List<DeletedIds> GetListOfDeletions();

        public void DeleteAuxiliaryDatabases();

        //Maybe The Following Methods Have to be Moved Elsewhere
        public string GetAuxiliaryDocument(string id);

        public void DeleteAVolunteer(string id, bool localConnection);

        public void InsertVolunteer(Volunteer volunteer, bool localConnection);

        public void UpdateVolunteer(Volunteer volunteer, bool localConnection);

        public Volunteer GetOneVolunteer(string id);

        public void InsertEvent(Event @event, bool localConnection);

        public Event GetOneEvent(string id);

        public void UpdateEvent(Event @event, bool localConnection);

        public void DeleteAnEvent(string id, bool localConnection);

        public void InsertBeneficiary(Beneficiary beneficiary, bool localConnection);

        public Beneficiary GetOneBeneficiary(string beneficiaryId);

        public void UpdateBeneficiary(Beneficiary beneficiary, bool localConnection);

        public void DeleteABeneficiary(string id, bool localConnection);

        public void InsertSponsor(Sponsor sponsor, bool localConnection);

        public Sponsor GetOneSponsor(string id);

        public void UpdateSponsor(Sponsor sponsor, bool localConnection);

        public void DeleteASponsor(string id, bool localConnection);

        public void InsertBeneficiaryContract(BeneficiaryContract beneficiaryContract, bool localConnection);

        public BeneficiaryContract GetOneBeneficiaryContract(string id);

        public void UpdateBeneficiaryContract(BeneficiaryContract beneficiaryContract, bool localConnection);

        public void DeleteABeneficiaryContract(string id, bool localConnection);

        public void InsertVolunteerContract(VolunteerContract volunteerContract, bool localConnection);

        public VolunteerContract GetOneVolunteerContract(string id);

        public void UpdateVolunteerContract(VolunteerContract volunteerContract, bool localConnection);

        public void DeleteAVolunteerContract(string id, bool localConnection);
    }
}