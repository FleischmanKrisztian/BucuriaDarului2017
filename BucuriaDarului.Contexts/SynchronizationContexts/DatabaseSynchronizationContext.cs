using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.DatabaseManagerGateways;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BucuriaDarului.Contexts.SynchronizationContexts
{
    public class DatabaseSynchronizationContext
    {
        private readonly ISynchronizationGateway dataGateway;
        private SynchronizationResponse response = new SynchronizationResponse();

        public DatabaseSynchronizationContext(ISynchronizationGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public SynchronizationResponse Execute()
        {
            var databaseLists = GetDatabaseLists();
            var serializedStrings = GetSerializedStrings(databaseLists);

            SynchronizeVolunteers(databaseLists.LocalVolunteers, serializedStrings.VolunteerString, serializedStrings.ModifiedIdString, databaseLists.DeletedIds);
            SynchronizeEvents(databaseLists.LocalEvents, serializedStrings.EventString, serializedStrings.ModifiedIdString, databaseLists.DeletedIds);
            SynchronizeSponsors(databaseLists.LocalSponsors, serializedStrings.SponsorString, serializedStrings.ModifiedIdString, databaseLists.DeletedIds);
            SynchronizeBeneficiaries(databaseLists.LocalBeneficiaries, serializedStrings.BeneficiaryString, serializedStrings.ModifiedIdString, databaseLists.DeletedIds);
            SynchronizeVolunteerContracts(databaseLists.LocalVolunteerContracts, serializedStrings.VolunteerContractString, serializedStrings.ModifiedIdString, databaseLists.DeletedIds);
            SynchronizeBeneficiaryContracts(databaseLists.LocalBeneficiaryContracts, serializedStrings.BeneficiaryContractString, serializedStrings.ModifiedIdString, databaseLists.DeletedIds);

            response.NumberOfDeletions = dataGateway.GetListOfDeletions().Count;
            response.NumberOfModifications = dataGateway.GetListOfModifications().Count;

            dataGateway.DeleteAuxiliaryDatabases();

            databaseLists = GetDatabaseLists();
            serializedStrings = GetSerializedStrings(databaseLists);

            SynchronizeLocalVolunteers(databaseLists.CommonVolunteers, databaseLists.LocalVolunteers, serializedStrings.VolunteerString, serializedStrings.LocalVolunteerString, serializedStrings.ModifiedIdString);
            SynchronizeLocalEvents(databaseLists.CommonEvents, databaseLists.LocalEvents, serializedStrings.EventString, serializedStrings.LocalEventString, serializedStrings.ModifiedIdString);
            SynchronizeLocalSponsors(databaseLists.CommonSponsors, databaseLists.LocalSponsors, serializedStrings.SponsorString, serializedStrings.LocalSponsorString, serializedStrings.ModifiedIdString);
            SynchronizeLocalBeneficiaries(databaseLists.CommonBeneficiaries, databaseLists.LocalBeneficiaries, serializedStrings.BeneficiaryString, serializedStrings.LocalBeneficiaryString, serializedStrings.ModifiedIdString);
            SynchronizeLocalVolunteerContracts(databaseLists.CommonVolunteerContracts, databaseLists.LocalVolunteerContracts, serializedStrings.VolunteerContractString, serializedStrings.LocalVolunteerContractString, serializedStrings.ModifiedIdString);
            SynchronizeLocalBeneficiaryContracts(databaseLists.CommonBeneficiaryContracts, databaseLists.LocalBeneficiaryContracts, serializedStrings.BeneficiaryContractString, serializedStrings.LocalBeneficiaryContractString, serializedStrings.ModifiedIdString);

            dataGateway.DeleteAuxiliaryDatabases();

            return response;
        }

        private void SynchronizeLocalVolunteers(List<Volunteer> commonVolunteers, List<Volunteer> localVolunteers, string commonVolunteerString, string localVolunteerString, string modifiedIds)
        {
            const bool localConnection = true;
            foreach (var commonVolunteer in commonVolunteers)
            {
                if (!(localVolunteerString.Contains(commonVolunteer.Id)))
                    dataGateway.InsertVolunteer(commonVolunteer, localConnection);
                else if (!modifiedIds.Contains(commonVolunteer.Id))
                    dataGateway.UpdateVolunteer(commonVolunteer, localConnection);
            }

            foreach (var localVolunteer in localVolunteers)
            {
                if (!commonVolunteerString.Contains(localVolunteer.Id))
                    dataGateway.DeleteAVolunteer(localVolunteer.Id, localConnection);
            }
        }

        private void SynchronizeLocalEvents(List<Event> commonEvents, List<Event> localEvents, string commonEventString, string localEventString, string modifiedIds)
        {
            const bool localConnection = true;
            foreach (var commonEvent in commonEvents)
            {
                if (!(localEventString.Contains(commonEvent.Id)))
                    dataGateway.InsertEvent(commonEvent, localConnection);
                else if (!modifiedIds.Contains(commonEvent.Id))
                    dataGateway.UpdateEvent(commonEvent, localConnection);
            }

            foreach (var localEvent in localEvents)
            {
                if (!commonEventString.Contains(localEvent.Id))
                    dataGateway.DeleteAnEvent(localEvent.Id, localConnection);
            }
        }

        private void SynchronizeLocalSponsors(List<Sponsor> commonSponsors, List<Sponsor> localSponsors, string commonSponsorString, string localSponsorString, string modifiedIds)
        {
            const bool localConnection = true;
            foreach (var commonSponsor in commonSponsors)
            {
                if (!(localSponsorString.Contains(commonSponsor.Id)))
                    dataGateway.InsertSponsor(commonSponsor, localConnection);
                else if (!modifiedIds.Contains(commonSponsor.Id))
                    dataGateway.UpdateSponsor(commonSponsor, localConnection);
            }

            foreach (var localSponsor in localSponsors)
            {
                if (!commonSponsorString.Contains(localSponsor.Id))
                    dataGateway.DeleteASponsor(localSponsor.Id, localConnection);
            }
        }

        private void SynchronizeLocalBeneficiaries(List<Beneficiary> commonBeneficiaries, List<Beneficiary> localBeneficiaries, string commonBeneficiaryString, string localBeneficiaryString, string modifiedIds)
        {
            const bool localConnection = true;
            foreach (var commonBeneficiary in commonBeneficiaries)
            {
                if (!(localBeneficiaryString.Contains(commonBeneficiary.Id)))
                    dataGateway.InsertBeneficiary(commonBeneficiary, localConnection);
                else if (!modifiedIds.Contains(commonBeneficiary.Id))
                    dataGateway.UpdateBeneficiary(commonBeneficiary, localConnection);
            }

            foreach (var localBeneficiary in localBeneficiaries)
            {
                if (!commonBeneficiaryString.Contains(localBeneficiary.Id))
                    dataGateway.DeleteABeneficiary(localBeneficiary.Id, localConnection);
            }
        }

        private void SynchronizeLocalVolunteerContracts(List<VolunteerContract> commonVolunteerContracts, List<VolunteerContract> localVolunteerContracts, string commonVolunteerContractString, string localVolunteerContractString, string modifiedIds)
        {
            const bool localConnection = true;
            foreach (var commonVolunteerContract in commonVolunteerContracts)
            {
                if (!(localVolunteerContractString.Contains(commonVolunteerContract.Id)))
                    dataGateway.InsertVolunteerContract(commonVolunteerContract, localConnection);
                else if (!modifiedIds.Contains(commonVolunteerContract.Id))
                    dataGateway.UpdateVolunteerContract(commonVolunteerContract, localConnection);
            }

            foreach (var localVolunteerContract in localVolunteerContracts)
            {
                if (!commonVolunteerContractString.Contains(localVolunteerContract.Id))
                    dataGateway.DeleteAVolunteerContract(localVolunteerContract.Id, localConnection);
            }
        }

        private void SynchronizeLocalBeneficiaryContracts(List<BeneficiaryContract> commonBeneficiaryContracts, List<BeneficiaryContract> localBeneficiaryContracts, string commonBeneficiaryContractString, string localBeneficiaryContractString, string modifiedIds)
        {
            const bool localConnection = true;
            foreach (var commonBeneficiaryContract in commonBeneficiaryContracts)
            {
                if (!(localBeneficiaryContractString.Contains(commonBeneficiaryContract.Id)))
                    dataGateway.InsertBeneficiaryContract(commonBeneficiaryContract, localConnection);
                else if (!modifiedIds.Contains(commonBeneficiaryContract.Id))
                    dataGateway.UpdateBeneficiaryContract(commonBeneficiaryContract, localConnection);
            }

            foreach (var localBeneficiaryContract in localBeneficiaryContracts)
            {
                if (!commonBeneficiaryContractString.Contains(localBeneficiaryContract.Id))
                    dataGateway.DeleteABeneficiaryContract(localBeneficiaryContract.Id, localConnection);
            }
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

                LocalVolunteerString = JsonConvert.SerializeObject(databaseLists.LocalVolunteers),
                LocalEventString = JsonConvert.SerializeObject(databaseLists.LocalEvents),
                LocalBeneficiaryString = JsonConvert.SerializeObject(databaseLists.LocalBeneficiaries),
                LocalSponsorString = JsonConvert.SerializeObject(databaseLists.LocalSponsors),
                LocalVolunteerContractString = JsonConvert.SerializeObject(databaseLists.LocalVolunteerContracts),
                LocalBeneficiaryContractString = JsonConvert.SerializeObject(databaseLists.LocalBeneficiaryContracts),

                ModifiedIdString = JsonConvert.SerializeObject(databaseLists.ModifiedIds),
            };
            return serializedStrings;
        }

        private void SynchronizeVolunteers(List<Volunteer> localVolunteers, string volunteerString, string modifiedIds, List<DeletedIds> deletedIds)
        {
            const bool commonConnection = false;
            foreach (var volunteer in localVolunteers)
            {
                // If the common db does not contain the volunteer and it has been created since the last fetch/push it gets added.
                if (!volunteerString.Contains(volunteer.Id) && modifiedIds.Contains(volunteer.Id))
                    dataGateway.InsertVolunteer(volunteer, commonConnection);
                // if the common db contains the volunteer, but it has been edited since last sync it gets updated
                else if (modifiedIds.Contains(volunteer.Id))
                {
                    var auxiliaryDocument = dataGateway.GetAuxiliaryDocument(volunteer.Id);
                    auxiliaryDocument = RemoveInitialID(auxiliaryDocument);
                    var currentDocument = JsonConvert.SerializeObject(dataGateway.GetOneVolunteer(volunteer.Id));
                    auxiliaryDocument = auxiliaryDocument.Replace(" ", "");
                    currentDocument = currentDocument.Replace(" ", "");
                    //Checking whether or not the Document has been modified since we have last synced.
                    if (auxiliaryDocument != currentDocument)
                    {
                        response.OutOfSyncDocuments += volunteer.Fullname + ", ";
                    }

                    dataGateway.UpdateVolunteer(volunteer, commonConnection);
                }
            }
            foreach (var deletedId in deletedIds)
            {
                dataGateway.DeleteAVolunteer(deletedId.DeletedId, commonConnection);
            }
        }

        private void SynchronizeEvents(List<Event> localEvents, string eventString, string modifiedIds, List<DeletedIds> deletedIds)
        {
            const bool commonConnection = false;
            foreach (var @event in localEvents)
            {
                if (!eventString.Contains(@event.Id) && modifiedIds.Contains(@event.Id))
                    dataGateway.InsertEvent(@event, commonConnection);
                else if (modifiedIds.Contains(@event.Id))
                {
                    var auxiliaryDocument = dataGateway.GetAuxiliaryDocument(@event.Id);
                    auxiliaryDocument = RemoveInitialID(auxiliaryDocument);
                    var currentDocument = JsonConvert.SerializeObject(dataGateway.GetOneEvent(@event.Id));
                    auxiliaryDocument = auxiliaryDocument.Replace(" ", "");
                    currentDocument = currentDocument.Replace(" ", "");
                    if (auxiliaryDocument != currentDocument)
                    {
                        response.OutOfSyncDocuments += @event.NameOfEvent + ", ";
                    }
                    dataGateway.UpdateEvent(@event, commonConnection);
                }
            }
            foreach (var deletedId in deletedIds)
            {
                dataGateway.DeleteAnEvent(deletedId.DeletedId, commonConnection);
            }
        }

        private void SynchronizeBeneficiaries(List<Beneficiary> localBeneficiaries, string beneficiaryString, string modifiedIds, List<DeletedIds> deletedIds)
        {
            const bool commonConnection = false;
            foreach (var beneficiary in localBeneficiaries)
            {
                if (!beneficiaryString.Contains(beneficiary.Id) && modifiedIds.Contains(beneficiary.Id))
                    dataGateway.InsertBeneficiary(beneficiary, commonConnection);
                else if (modifiedIds.Contains(beneficiary.Id))
                {
                    var auxiliaryDocument = dataGateway.GetAuxiliaryDocument(beneficiary.Id);
                    auxiliaryDocument = RemoveInitialID(auxiliaryDocument);
                    var currentDocument = JsonConvert.SerializeObject(dataGateway.GetOneBeneficiary(beneficiary.Id));
                    auxiliaryDocument = auxiliaryDocument.Replace(" ", "");
                    currentDocument = currentDocument.Replace(" ", "");
                    if (auxiliaryDocument != currentDocument)
                    {
                        response.OutOfSyncDocuments += beneficiary.Fullname + ", ";
                    }
                    dataGateway.UpdateBeneficiary(beneficiary, commonConnection);
                }
            }
            foreach (var deletedId in deletedIds)
            {
                dataGateway.DeleteABeneficiary(deletedId.DeletedId, commonConnection);
            }
        }

        private void SynchronizeSponsors(List<Sponsor> localSponsors, string sponsorString, string modifiedIds, List<DeletedIds> deletedIds)
        {
            const bool commonConnection = false;
            foreach (var sponsor in localSponsors)
            {
                if (!sponsorString.Contains(sponsor.Id) && modifiedIds.Contains(sponsor.Id))
                    dataGateway.InsertSponsor(sponsor, commonConnection);
                else if (modifiedIds.Contains(sponsor.Id))
                {
                    var auxiliaryDocument = dataGateway.GetAuxiliaryDocument(sponsor.Id);
                    auxiliaryDocument = RemoveInitialID(auxiliaryDocument);
                    var currentDocument = JsonConvert.SerializeObject(dataGateway.GetOneSponsor(sponsor.Id));
                    auxiliaryDocument = auxiliaryDocument.Replace(" ", "");
                    currentDocument = currentDocument.Replace(" ", "");
                    if (auxiliaryDocument != currentDocument)
                    {
                        response.OutOfSyncDocuments += sponsor.NameOfSponsor + ", ";
                    }
                    dataGateway.UpdateSponsor(sponsor, commonConnection);
                }
            }
            foreach (var deletedId in deletedIds)
            {
                dataGateway.DeleteASponsor(deletedId.DeletedId, commonConnection);
            }
        }

        private void SynchronizeBeneficiaryContracts(List<BeneficiaryContract> localBeneficiaryContracts, string beneficiaryContractString, string modifiedIds, List<DeletedIds> deletedIds)
        {
            const bool commonConnection = false;
            foreach (var beneficiaryContract in localBeneficiaryContracts)
            {
                if (!beneficiaryContractString.Contains(beneficiaryContract.Id) && modifiedIds.Contains(beneficiaryContract.Id))
                    dataGateway.InsertBeneficiaryContract(beneficiaryContract, commonConnection);
                else if (modifiedIds.Contains(beneficiaryContract.Id))
                {
                    var auxiliaryDocument = dataGateway.GetAuxiliaryDocument(beneficiaryContract.Id);
                    auxiliaryDocument = RemoveInitialID(auxiliaryDocument);
                    var currentDocument = JsonConvert.SerializeObject(dataGateway.GetOneBeneficiaryContract(beneficiaryContract.Id));
                    auxiliaryDocument = auxiliaryDocument.Replace(" ", "");
                    currentDocument = currentDocument.Replace(" ", "");
                    if (auxiliaryDocument != currentDocument)
                    {
                        response.OutOfSyncDocuments += beneficiaryContract.NumberOfRegistration + ", ";
                    }
                    dataGateway.UpdateBeneficiaryContract(beneficiaryContract, commonConnection);
                }
            }
            foreach (var deletedId in deletedIds)
            {
                dataGateway.DeleteABeneficiaryContract(deletedId.DeletedId, commonConnection);
            }
        }

        private void SynchronizeVolunteerContracts(List<VolunteerContract> localVolunteerContracts, string volunteerContractString, string modifiedIds, List<DeletedIds> deletedIds)
        {
            const bool commonConnection = false;
            foreach (var volunteerContract in localVolunteerContracts)
            {
                if (!volunteerContractString.Contains(volunteerContract.Id) && modifiedIds.Contains(volunteerContract.Id))
                    dataGateway.InsertVolunteerContract(volunteerContract, commonConnection);
                else if (modifiedIds.Contains(volunteerContract.Id))
                {
                    var auxiliaryDocument = dataGateway.GetAuxiliaryDocument(volunteerContract.Id);
                    auxiliaryDocument = RemoveInitialID(auxiliaryDocument);
                    var currentDocument = JsonConvert.SerializeObject(dataGateway.GetOneVolunteerContract(volunteerContract.Id));
                    auxiliaryDocument = auxiliaryDocument.Replace(" ", "");
                    currentDocument = currentDocument.Replace(" ", "");
                    if (auxiliaryDocument != currentDocument)
                    {
                        response.OutOfSyncDocuments += volunteerContract.NumberOfRegistration + ", ";
                    }
                    dataGateway.UpdateVolunteerContract(volunteerContract, commonConnection);
                }
            }
            foreach (var deletedId in deletedIds)
            {
                dataGateway.DeleteAVolunteerContract(deletedId.DeletedId, commonConnection);
            }
        }

        private string RemoveInitialID(string auxiliaryDocument)
        {
            return auxiliaryDocument.Remove(1, 46);
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
            public string LocalVolunteerString { get; set; }
            public string LocalEventString { get; set; }
            public string LocalBeneficiaryString { get; set; }
            public string LocalSponsorString { get; set; }
            public string LocalVolunteerContractString { get; set; }
            public string LocalBeneficiaryContractString { get; set; }
            public string ModifiedIdString { get; set; }
        }
    }
}