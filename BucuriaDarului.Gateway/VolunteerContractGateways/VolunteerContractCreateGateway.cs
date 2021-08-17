using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using BucuriaDarului.Gateway.VolunteerGateways;

namespace BucuriaDarului.Gateway.VolunteerContractGateways
{
    public class VolunteerContractCreateGateway : IVolunteerContractCreateGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public Volunteer GetVolunteer(string idOfVolunteer)
        {
            return SingleVolunteerReturnerGateway.ReturnVolunteer(idOfVolunteer);
        }

        public void Insert(VolunteerContract volunteerContract)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var volunteerContractCollection = dbContext.Database.GetCollection<VolunteerContract>("VolunteerContracts");
            volunteerContractCollection.InsertOne(volunteerContract);
            var modifiedIdGateway = new ModifiedIDGateway();
            modifiedIdGateway.AddIDtoModifications(volunteerContract.Id);
        }
    }
}
