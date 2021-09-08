using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using BucuriaDarului.Gateway.VolunteerGateways;

namespace BucuriaDarului.Gateway.VolunteerContractGateways
{
    public class VolunteerAdditionalContractCreateGateway : IVolunteerAdditionalContractCreateGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public VolunteerContract GetContract(string id)
        {
            return SingleVolunteerContractReturnerGateway.GetVolunteerContract(id);
        }

        public Volunteer GetVolunteer(string idOfVolunteer)
        {
            return SingleVolunteerReturnerGateway.ReturnVolunteer(idOfVolunteer);
        }

        public void Insert(AdditionalContractVolunteer additionalContract)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var additionalContractCollection = dbContext.Database.GetCollection<AdditionalContractVolunteer>("VolunteerAdditionalContracts");
            additionalContractCollection.InsertOne(additionalContract);
            var modifiedIdGateway = new ModifiedIDGateway();
            modifiedIdGateway.AddIDtoModifications(additionalContract.Id);
        }

       
    }
}