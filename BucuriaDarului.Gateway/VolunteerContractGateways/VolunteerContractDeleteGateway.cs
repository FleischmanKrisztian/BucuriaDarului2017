using BucuriaDarului.Core;
using MongoDB.Driver;

namespace BucuriaDarului.Gateway.VolunteerContractGateways
{
    public class VolunteerContractDeleteGateway
    {
       
        public static void Delete(string id)
        {
            var dbContext = new MongoDBGateway();
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var volunteerContractCollection = dbContext.Database.GetCollection<VolunteerContract>("Contracts");
            var filter = Builders<VolunteerContract>.Filter.Eq("Id", id);
            volunteerContractCollection.DeleteOne(filter);
        }
    }
}
