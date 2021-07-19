using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.DatabaseManagerGateways;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BucuriaDarului.Contexts.SynchronizationContexts
{
    public class DatabaseSynchronizationContext
    {
        private readonly ISynchronizationGateway dataGateway;
        public SynchronizationResponse response;

        public DatabaseSynchronizationContext(ISynchronizationGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public SynchronizationResponse Execute()
        {
            var databaseLists = GetDatabaseLists();
            var serializedStrings = GetSerializedStrings(databaseLists);


            SynchronizeVolunteers(databaseLists.LocalVolunteers, databaseLists.CommonVolunteers,serializedStrings.VolunteerString, serializedStrings.ModifiedIdString, serializedStrings.DeletedIdString);
            //TODO: The same for each DocumentType
            //SynchronizeVolunteers(databaseLists.LocalVolunteers, databaseLists.CommonVolunteers, serializedStrings.VolunteerString, serializedStrings.ModifiedIdString, serializedStrings.DeletedIdString);
            //SynchronizeVolunteers(databaseLists.LocalVolunteers, databaseLists.CommonVolunteers, serializedStrings.VolunteerString, serializedStrings.ModifiedIdString, serializedStrings.DeletedIdString);
            //SynchronizeVolunteers(databaseLists.LocalVolunteers, databaseLists.CommonVolunteers, serializedStrings.VolunteerString, serializedStrings.ModifiedIdString, serializedStrings.DeletedIdString);
            //SynchronizeVolunteers(databaseLists.LocalVolunteers, databaseLists.CommonVolunteers, serializedStrings.VolunteerString, serializedStrings.ModifiedIdString, serializedStrings.DeletedIdString);
            //SynchronizeVolunteers(databaseLists.LocalVolunteers, databaseLists.CommonVolunteers, serializedStrings.VolunteerString, serializedStrings.ModifiedIdString, serializedStrings.DeletedIdString);

            //TODO: DeleteAuxiliary Databases

            //TODO: Bring the Changes to the LocalDatabase

            //TODO: DeleteAuxiliary Databases

            //TODO: TEST IT!!



            return response;
        }

        private SerializedLists GetSerializedStrings(DatabaseLists databaseLists)
        {
            var serializedStrings = new SerializedLists
            {
                VolunteerString = JsonConvert.SerializeObject(databaseLists.CommonVolunteers),
                EventString = JsonConvert.SerializeObject(databaseLists.CommonEvents),
                BeneficiaryString = JsonConvert.SerializeObject(databaseLists.CommonBeneficiaries),
                SponsorString = JsonConvert.SerializeObject(databaseLists.CommonSponsors),
                VolunteerContractString = JsonConvert.SerializeObject(databaseLists.CommonVolunteerContracts),
                BeneficiaryContractString = JsonConvert.SerializeObject(databaseLists.CommonBeneficiaryContracts),
                ModifiedIdString = JsonConvert.SerializeObject(databaseLists.ModifiedIds),
                DeletedIdString = JsonConvert.SerializeObject(databaseLists.DeletedIds)
            };
            return serializedStrings;
        }

        private void SynchronizeVolunteers(List<Volunteer> localVolunteers, List<Volunteer> commonVolunteers, string volunteerString, string modifiedIds, string deletedIds)
        {
            foreach (var volunteer in localVolunteers)
            {
                // If the common db does not contain the volunteer and it has been created since the last fetch/push it gets added.
                if (!volunteerString.Contains(volunteer.Id) && modifiedIds.Contains(volunteer.Id))
                    dataGateway.InsertVolunteer(volunteer);
                // if the common db contains the volunteer, but it has been edited since last sync it gets updated
                else if (modifiedIds.Contains(volunteer.Id))
                {
                    var auxiliaryDocument = dataGateway.GetAuxiliaryDocument(volunteer.Id);
                    var currentDocument = JsonConvert.SerializeObject(dataGateway.GetOneVolunteer(volunteer.Id));
                    auxiliaryDocument = auxiliaryDocument.Replace(" ", "");
                    currentDocument = currentDocument.Replace(" ", "");
                    //Checking whether or not the Document has been modified since we have last synced.
                    if (auxiliaryDocument != currentDocument)
                    {
                        response.OutOfSyncDocuments += volunteer.Fullname + ", ";
                    }
                    dataGateway.UpdateVolunteer(volunteer);
                }
            }

            foreach (var volunteer in localVolunteers)
            {
                // if the document has been deleted it will get deleted from the common db aswell.
                // the document will not be re-added unless someone has modified the document with this ID.
                if (deletedIds.Contains(volunteer.Id))
                    dataGateway.DeleteAVolunteer(volunteer.Id);
            }
        }

        private DatabaseLists GetDatabaseLists()
        {
            const bool localConnection = true;
            const bool commonConnection = false;

            var databaseLists = new DatabaseLists
            {
                LocalEvents = dataGateway.GetEventList(localConnection),
                CommonEvents = dataGateway.GetEventList(commonConnection),
                LocalVolunteers = dataGateway.GetVolunteerList(localConnection),
                CommonVolunteers = dataGateway.GetVolunteerList(commonConnection),
                LocalSponsors = dataGateway.GetSponsorList(localConnection),
                CommonSponsors = dataGateway.GetSponsorList(commonConnection),
                LocalBeneficiaries = dataGateway.GetBeneficiaryList(localConnection),
                CommonBeneficiaries = dataGateway.GetBeneficiaryList(commonConnection),
                LocalVolunteerContracts = dataGateway.GetVolunteerContractList(localConnection),
                CommonVolunteerContracts = dataGateway.GetVolunteerContractList(commonConnection),
                LocalBeneficiaryContracts = dataGateway.GetBeneficiaryContractList(localConnection),
                CommonBeneficiaryContracts = dataGateway.GetBeneficiaryContractList(commonConnection),
                ModifiedIds = dataGateway.GetListOfModifications(),
                DeletedIds = dataGateway.GetListOfDeletions()
            };
            return databaseLists;
        }

        public class SynchronizationResponse
        {
            public int NumberOfModifications { get; set; }
            public int NumberOfDeletions { get; set; }
            public string OutOfSyncDocuments { get; set; }
        }

        public class DatabaseLists
        {
            public List<Event> LocalEvents { get; set; }
            public List<Event> CommonEvents { get; set; }
            public List<Sponsor> LocalSponsors { get; set; }
            public List<Sponsor> CommonSponsors { get; set; }
            public List<Volunteer> LocalVolunteers { get; set; }
            public List<Volunteer> CommonVolunteers { get; set; }
            public List<Beneficiary> LocalBeneficiaries { get; set; }
            public List<Beneficiary> CommonBeneficiaries { get; set; }
            public List<VolunteerContract> LocalVolunteerContracts { get; set; }
            public List<VolunteerContract> CommonVolunteerContracts { get; set; }
            public List<BeneficiaryContract> LocalBeneficiaryContracts { get; set; }
            public List<BeneficiaryContract> CommonBeneficiaryContracts { get; set; }
            public List<ModifiedIDs> ModifiedIds { get; set; }
            public List<DeletedIds> DeletedIds { get; set; }
        }

        public class SerializedLists
        {
            public string VolunteerString { get; set; }
            public string EventString { get; set; }
            public string BeneficiaryString { get; set; }
            public string SponsorString { get; set; }
            public string VolunteerContractString { get; set; }
            public string BeneficiaryContractString { get; set; }
            public string ModifiedIdString { get; set; }
            public string DeletedIdString { get; set; }
        }
    }
}