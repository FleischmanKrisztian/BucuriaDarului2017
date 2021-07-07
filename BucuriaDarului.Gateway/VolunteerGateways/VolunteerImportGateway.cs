using System.Collections.Generic;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
using MongoDB.Driver;

namespace BucuriaDarului.Gateway.VolunteerGateways
{
    public class VolunteerImportGateway : IVolunteerImportGateway
    {
        private readonly MongoDBGateway dbContext = new MongoDBGateway();

        public void Insert(List<Volunteer> volunteers)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var volunteerCollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            var modifiedIdGateway = new ModifiedIDGateway();
            foreach (var volunteer in volunteers)
            {
                var filter = Builders<Volunteer>.Filter.Eq("_id", volunteer.Id);
                if (volunteerCollection.Find(filter).FirstOrDefault() == null)
                {
                    volunteerCollection.InsertOne(volunteer);
                    modifiedIdGateway.AddIDtoModifications(volunteer.Id);
                }
            }
        }
    }
}