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

        //Maybe The Following Methods Have to be Moved Elsewhere
        public void DeleteAVolunteer(string id);
        public void InsertVolunteer(Volunteer volunteer);
        public void UpdateVolunteer(Volunteer volunteer);
        public string GetAuxiliaryDocument(string id);
        public Volunteer GetOneVolunteer(string id);
    }
}