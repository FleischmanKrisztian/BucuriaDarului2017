using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Gateway.VolunteerGateways
{
    public class VolunteerEditGateway : IVolunteerEditGateway
    {
        private readonly MongoDBGateway dbContext = new MongoDBGateway();

        public void Edit(Volunteer volunteer)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var volunteerCollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            var filter = Builders<Volunteer>.Filter.Eq("Id", volunteer.Id);
            var modifiedIdGateway = new ModifiedIDGateway();
            modifiedIdGateway.AddIDtoModifications(volunteer.Id);
           volunteerCollection.FindOneAndReplace(filter, volunteer);
        }

        public List<ModifiedIDs> ReturnModificationList()
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var modifiedCollection = dbContext.Database.GetCollection<ModifiedIDs>("ModifiedIDS");
            var modifiedIDs = modifiedCollection.AsQueryable().ToList();
            return modifiedIDs;
        }

        public void AddVolunteerToModifiedList(string beforeEditingVolunteerString)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            BsonDocument.TryParse(beforeEditingVolunteerString, out var documentAsBson);
            var auxiliaryCollection = dbContext.Database.GetCollection<BsonDocument>("Auxiliary");
            auxiliaryCollection.InsertOne(documentAsBson);
        }

        public Volunteer ReturnVolunteer(string volunteerId)
        {
            return SingleVolunteerReturnerGateway.ReturnVolunteer(volunteerId);
        }
    }
}
