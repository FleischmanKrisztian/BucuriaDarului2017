using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.VolunteerGateways
{
    public class VolunteerImportGateway : IVolunteerImportGateway
    {
        private readonly MongoDBGateway dbContext = new MongoDBGateway();

        public List<Volunteer> GetVolunteersList()
        {
            return ListVolunteersGateway.GetListOfVolunteers();
        }

        public void Insert(List<Volunteer> volunteers)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var volunteerCollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            var modifiedIdGateway = new ModifiedIDGateway();
            foreach (var volunteer in volunteers)
            {
                var filter = Builders<Volunteer>.Filter.Eq("Id", volunteer.Id);
                if (volunteerCollection.Find(filter).FirstOrDefault() == null)
                {
                    volunteerCollection.InsertOne(volunteer);
                    modifiedIdGateway.AddIDtoModifications(volunteer.Id);
                }
            }
        }

        public void UpdateVolunteers(List<Volunteer> volunteers)
        {
            foreach (var volunteer in volunteers)
            {
                dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
                var volunteerCollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
                var filter = Builders<Volunteer>.Filter.Eq("Id", volunteer.Id);
                var modifiedIdGateway = new ModifiedIDGateway();
                modifiedIdGateway.AddIDtoModifications(volunteer.Id);
                volunteerCollection.FindOneAndReplace(filter, volunteer);
            }
        }
    }
}