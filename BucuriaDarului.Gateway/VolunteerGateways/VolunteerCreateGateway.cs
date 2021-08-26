using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;

namespace BucuriaDarului.Gateway.VolunteerGateways
{
    public class VolunteerCreateGateway : IVolunteerCreateGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public void Insert(Volunteer volunteer)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var volunteerCollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            volunteerCollection.InsertOne(volunteer);
            var modifiedIDGateway = new ModifiedIDGateway();
            modifiedIDGateway.AddIDtoModifications(volunteer.Id);
        }
    }
}