using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
using BucuriaDarului.Gateway.EventGateways;
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
            var deletedIdGateway = new DeletedIDGateway();
            deletedIdGateway.AddIDtoDeletions(id);
            volunteerCollection.DeleteOne(filter);
        }

        public void DeleteVolunteerContracts(string id)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var volunteerContractCollection = dbContext.Database.GetCollection<VolunteerContract>("VolunteerContracts");
            var filter = Builders<VolunteerContract>.Filter.Eq("OwnerID", id);
            volunteerContractCollection.DeleteMany(filter);
        }

        public List<Event> GetEvents()
        {
            return ListEventsGateway.GetListOfEvents();
        }

        public Volunteer GetVolunteer(string id)
        {
            return SingleVolunteerReturnerGateway.ReturnVolunteer(id);
        }

        public void UpdateEvent(string eventId, Event @event)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var eventCollection = dbContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("Id", eventId);
            @event.Id = eventId;
            var modifiedIDGateway = new ModifiedIDGateway();
            modifiedIDGateway.AddIDtoModifications(eventId);
            eventCollection.FindOneAndReplace(filter, @event);

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
