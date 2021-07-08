using BucuriaDarului.Core;
using MongoDB.Driver;

namespace BucuriaDarului.Gateway.BeneficiaryGateways
{
    public class SingleBeneficiaryReturnerGateway
    {
        public static Beneficiary ReturnBeneficiary(string id)
        {
            var dbContext = new MongoDBGateway();

            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var beneficiaryCollection = dbContext.Database.GetCollection<Beneficiary>("Beneficiaries");
            var filter = Builders<Beneficiary>.Filter.Eq("Id", id);
            return beneficiaryCollection.Find(filter).FirstOrDefault();
        }
    }
}