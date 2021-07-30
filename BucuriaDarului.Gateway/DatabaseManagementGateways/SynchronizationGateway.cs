using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.DatabaseManagerGateways;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.DatabaseManagementGateways
{
    public class SynchronizationGateway : ISynchronizationGateway
    {
        private readonly MongoDBGateway dbContext = new MongoDBGateway();

        public List<Event> GetEventList(bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var eventCollection = dbContext.Database.GetCollection<Event>("Events");
            var events = eventCollection.AsQueryable().ToList();
            return events;
        }

        public List<Sponsor> GetSponsorList(bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var sponsorCollection = dbContext.Database.GetCollection<Sponsor>("Sponsors");
            var sponsors = sponsorCollection.AsQueryable().ToList();
            return sponsors;
        }

        public List<Volunteer> GetVolunteerList(bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var volunteerCollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            var volunteers = volunteerCollection.AsQueryable().ToList();
            return volunteers;
        }

        public List<Beneficiary> GetBeneficiaryList(bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var beneficiaryCollection = dbContext.Database.GetCollection<Beneficiary>("Beneficiaries");
            var beneficiaries = beneficiaryCollection.AsQueryable().ToList();
            return beneficiaries;
        }

        public List<BeneficiaryContract> GetBeneficiaryContractList(bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var beneficiaryContractCollection = dbContext.Database.GetCollection<BeneficiaryContract>("BeneficiaryContracts");
            var contracts = beneficiaryContractCollection.AsQueryable().ToList();
            return contracts;
        }

        public List<VolunteerContract> GetVolunteerContractList(bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var volunteerContractCollection = dbContext.Database.GetCollection<VolunteerContract>("Contracts");
            var contracts = volunteerContractCollection.AsQueryable().ToList();
            return contracts;
        }

        public List<ModifiedIDs> GetListOfModifications()
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var modifiedCollection = dbContext.Database.GetCollection<ModifiedIDs>("ModifiedIDS");
            var modifiedIDs = modifiedCollection.AsQueryable().ToList();
            return modifiedIDs;
        }

        public List<DeletedIds> GetListOfDeletions()
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var deletedCollection = dbContext.Database.GetCollection<DeletedIds>("DeletedIDS");
            var deletedIDs = deletedCollection.AsQueryable().ToList();
            return deletedIDs;
        }

        public void DeleteAuxiliaryDatabases()
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            dbContext.Database.DropCollection("DeletedIDS");
            dbContext.Database.DropCollection("ModifiedIDS");
            dbContext.Database.DropCollection("Auxiliary");
        }

        //Maybe The Following Methods Have to be Moved Elsewhere

        public string GetAuxiliaryDocument(string id)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var auxiliaryCollection = dbContext.Database.GetCollection<BsonDocument>("Auxiliary");
            var filter = Builders<BsonDocument>.Filter.Eq("Id", id);
            var bsonDocument = auxiliaryCollection.Find(filter).FirstOrDefault();
            var documentString = bsonDocument.ToJson();
            return documentString;
        }

        public void DeleteAVolunteer(string id, bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var volunteerCollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            var filter = Builders<Volunteer>.Filter.Eq("Id", id);
            volunteerCollection.DeleteOne(filter);
        }

        public void InsertVolunteer(Volunteer volunteer, bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var volunteerCollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            volunteerCollection.InsertOne(volunteer);
        }

        public void UpdateVolunteer(Volunteer volunteer, bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var volunteerCollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            var filter = Builders<Volunteer>.Filter.Eq("Id", volunteer.Id);
            volunteerCollection.FindOneAndReplace(filter, volunteer);
        }

        public Volunteer GetOneVolunteer(string id)
        {
            dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var volunteerCollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            var filter = Builders<Volunteer>.Filter.Eq("Id", id);
            return volunteerCollection.Find(filter).FirstOrDefault();
        }

        public void InsertEvent(Event @event, bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var eventCollection = dbContext.Database.GetCollection<Event>("Events");
            eventCollection.InsertOne(@event);
        }

        public Event GetOneEvent(string id)
        {
            dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var eventCollection = dbContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("Id", id);
            return eventCollection.Find(filter).FirstOrDefault();
        }

        public void UpdateEvent(Event @event, bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var eventCollection = dbContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("Id", @event.Id);
            eventCollection.FindOneAndReplace(filter, @event);
        }

        public void DeleteAnEvent(string id, bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var eventCollection = dbContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("Id", id);
            eventCollection.DeleteOne(filter);
        }

        public void InsertBeneficiary(Beneficiary beneficiary, bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var beneficiaryCollection = dbContext.Database.GetCollection<Beneficiary>("Beneficiaries");
            beneficiaryCollection.InsertOne(beneficiary);
        }

        public Beneficiary GetOneBeneficiary(string beneficiaryId)
        {
            dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var beneficiaryCollection = dbContext.Database.GetCollection<Beneficiary>("Beneficiaries");
            var filter = Builders<Beneficiary>.Filter.Eq("Id", beneficiaryId);
            return beneficiaryCollection.Find(filter).FirstOrDefault();
        }

        public void UpdateBeneficiary(Beneficiary beneficiary, bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var beneficiaryCollection = dbContext.Database.GetCollection<Beneficiary>("Beneficiaries");
            var filter = Builders<Beneficiary>.Filter.Eq("Id", beneficiary.Id);
            beneficiaryCollection.FindOneAndReplace(filter, beneficiary);
        }

        public void DeleteABeneficiary(string id, bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var beneficiaryCollection = dbContext.Database.GetCollection<Beneficiary>("Beneficiaries");
            var filter = Builders<Beneficiary>.Filter.Eq("Id", id);
            beneficiaryCollection.DeleteOne(filter);
        }

        public void InsertSponsor(Sponsor sponsor, bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var sponsorCollection = dbContext.Database.GetCollection<Sponsor>("Sponsors");
            sponsorCollection.InsertOne(sponsor);
        }

        public Sponsor GetOneSponsor(string id)
        {
            dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var sponsorCollection = dbContext.Database.GetCollection<Sponsor>("Sponsors");
            var filter = Builders<Sponsor>.Filter.Eq("Id", id);
            return sponsorCollection.Find(filter).FirstOrDefault();
        }

        public void UpdateSponsor(Sponsor sponsor, bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var sponsorCollection = dbContext.Database.GetCollection<Sponsor>("Sponsors");
            var filter = Builders<Sponsor>.Filter.Eq("Id", sponsor.Id);
            sponsorCollection.FindOneAndReplace(filter, sponsor);
        }

        public void DeleteASponsor(string id, bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var sponsorCollection = dbContext.Database.GetCollection<Sponsor>("Sponsors");
            var filter = Builders<Sponsor>.Filter.Eq("Id", id);
            sponsorCollection.DeleteOne(filter);
        }

        public void InsertBeneficiaryContract(BeneficiaryContract beneficiaryContract, bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var beneficiaryContractCollection = dbContext.Database.GetCollection<BeneficiaryContract>("BeneficiaryContracts");
            beneficiaryContractCollection.InsertOne(beneficiaryContract);
        }

        public BeneficiaryContract GetOneBeneficiaryContract(string id)
        {
            dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var beneficiaryContractCollection = dbContext.Database.GetCollection<BeneficiaryContract>("BeneficiaryContracts");
            var filter = Builders<BeneficiaryContract>.Filter.Eq("Id", id);
            return beneficiaryContractCollection.Find(filter).FirstOrDefault();
        }

        public void UpdateBeneficiaryContract(BeneficiaryContract beneficiaryContract, bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var beneficiaryContractCollection = dbContext.Database.GetCollection<BeneficiaryContract>("BeneficiaryContracts");
            var filter = Builders<BeneficiaryContract>.Filter.Eq("Id", beneficiaryContract.Id);
            beneficiaryContractCollection.FindOneAndReplace(filter, beneficiaryContract);
        }

        public void DeleteABeneficiaryContract(string id, bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var beneficiaryContractCollection = dbContext.Database.GetCollection<BeneficiaryContract>("BeneficiaryContracts");
            var filter = Builders<BeneficiaryContract>.Filter.Eq("Id", id);
            beneficiaryContractCollection.DeleteOne(filter);
        }

        public void InsertVolunteerContract(VolunteerContract volunteerContract, bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var volunteerContractCollection = dbContext.Database.GetCollection<VolunteerContract>("VolunteerContracts");
            volunteerContractCollection.InsertOne(volunteerContract);
        }

        public VolunteerContract GetOneVolunteerContract(string id)
        {
            dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var volunteerContractCollection = dbContext.Database.GetCollection<VolunteerContract>("VolunteerContracts");
            var filter = Builders<VolunteerContract>.Filter.Eq("Id", id);
            return volunteerContractCollection.Find(filter).FirstOrDefault();
        }

        public void UpdateVolunteerContract(VolunteerContract volunteerContract, bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var volunteerContractCollection = dbContext.Database.GetCollection<VolunteerContract>("VolunteerContracts");
            var filter = Builders<VolunteerContract>.Filter.Eq("Id", volunteerContract.Id);
            volunteerContractCollection.FindOneAndReplace(filter, volunteerContract);
        }

        public void DeleteAVolunteerContract(string id, bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var volunteerContractCollection = dbContext.Database.GetCollection<VolunteerContract>("VolunteerContracts");
            var filter = Builders<VolunteerContract>.Filter.Eq("Id", id);
            volunteerContractCollection.DeleteOne(filter);
        }
    }
}