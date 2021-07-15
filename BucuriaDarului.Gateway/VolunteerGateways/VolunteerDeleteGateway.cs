using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Gateway.VolunteerGateways
{
   public  class VolunteerDeleteGateway: IVolunteerDeleteGateways
    {
        private readonly MongoDBGateway dbContext = new MongoDBGateway();
        public  void Delete(string id)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var volunteerCollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            var filter = Builders<Volunteer>.Filter.Eq("Id", id);
            volunteerCollection.DeleteOne(filter);
        }

        public void DeleteVolunteerContracts(string id)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var volunteerContractCollection = dbContext.Database.GetCollection<VolunteerContract>("Contracts");
            var filter = Builders<VolunteerContract>.Filter.Eq("OwnerID", id);
            volunteerContractCollection.DeleteMany(filter);
        }

        public Volunteer GetVolunteer(string id)
        {
            return SingleVolunteerReturnerGateway.ReturnVolunteer(id);
        }

        public void UpdateVolunteer(string volunteerId, Volunteer volunteerToUpdate)
        {
           
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var volunteerCollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            var filter = Builders<Volunteer>.Filter.Eq("Id", volunteerId);
            volunteerToUpdate.Id = volunteerId;
            var modifiedIDGateway = new ModifiedIDGateway();
            modifiedIDGateway.AddIDtoModifications(volunteerId);
            volunteerCollection.FindOneAndReplace(filter, volunteerToUpdate);
        }
        
    }
    
}
