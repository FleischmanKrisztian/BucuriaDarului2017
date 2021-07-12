using BucuriaDarului.Core;
using MongoDB.Driver;

namespace BucuriaDarului.Gateway.BeneficiaryContractGateways
{
    public class SingleBeneficiaryContractReturnerGateway
    {
        public static BeneficiaryContract GetBeneficiaryContract(string id)
        {
            var dbContext = new MongoDBGateway();
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var volunteerContractCollection = dbContext.Database.GetCollection<BeneficiaryContract>("BeneficiaryContracts");
            var filter = Builders<BeneficiaryContract>.Filter.Eq("Id", id);
            return volunteerContractCollection.Find(filter).FirstOrDefault();
        }
    }
}