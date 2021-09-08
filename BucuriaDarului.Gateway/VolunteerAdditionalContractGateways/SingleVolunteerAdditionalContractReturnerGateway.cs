using BucuriaDarului.Core;
using MongoDB.Driver;

namespace BucuriaDarului.Gateway.VolunteerContractGateways
{
    public class SingleVolunteerAdditionalContractReturnerGateway
    {
        public static AdditionalContractVolunteer GetAdditionalContract(string id)
        {
            var dbContext = new MongoDBGateway();
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var additionalContractCollection = dbContext.Database.GetCollection<AdditionalContractVolunteer>("VolunteerAdditionalContracts");
            var filter = Builders<AdditionalContractVolunteer>.Filter.Eq("Id", id);
            return additionalContractCollection.Find(filter).FirstOrDefault();
        }
    }
}