using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.DatabaseManagerGateways;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using BucuriaDarului.Gateway.VolunteerGateways;

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
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON); var volunteerCollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            var volunteers = volunteerCollection.AsQueryable().ToList();
            return volunteers;
        }

        public List<Beneficiary> GetBeneficiaryList(bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON); var beneficiaryCollection = dbContext.Database.GetCollection<Beneficiary>("Beneficiaries");
            var beneficiaries = beneficiaryCollection.AsQueryable().ToList();
            return beneficiaries;
        }

        public List<BeneficiaryContract> GetBeneficiaryContractList(bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON); var beneficiaryContractCollection = dbContext.Database.GetCollection<BeneficiaryContract>("BeneficiaryContracts");
            var contracts = beneficiaryContractCollection.AsQueryable().ToList();
            return contracts;
        }

        public List<VolunteerContract> GetVolunteerContractList(bool localConnection)
        {
            if (localConnection)
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            else
                dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON); var volunteerContractCollection = dbContext.Database.GetCollection<VolunteerContract>("Contracts");
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


        //Maybe The Following Methods Have to be Moved Elsewhere
        public void DeleteAVolunteer(string id)
        {
            dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var volunteerCollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            var filter = Builders<Volunteer>.Filter.Eq("Id", id);
            volunteerCollection.DeleteOne(filter);
        }

        public void InsertVolunteer(Volunteer volunteer)
        {
            dbContext.ConnectToDB(SecondaryConnection.SERVER_NAME_COMMON, SecondaryConnection.SERVER_PORT_COMMON, SecondaryConnection.DATABASE_NAME_COMMON);
            var volunteerCollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            volunteerCollection.InsertOne(volunteer);
        }

        public void UpdateVolunteer(Volunteer volunteer)
        {
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

        public string GetAuxiliaryDocument(string id)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var auxiliaryCollection = dbContext.Database.GetCollection<BsonDocument>("Auxiliary");
            var filter = Builders<BsonDocument>.Filter.Eq("Id", id);
            var bsonDocument = auxiliaryCollection.Find(filter).FirstOrDefault();
            var documentString = bsonDocument.ToString();
            return documentString;
        }
    }
}