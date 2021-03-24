using Finalaplication.App_Start;
using MongoDB.Driver;
using VolCommon;

namespace Finalaplication.DatabaseManager
{
    public class VolContractManager
    {
        private MongoDBContext dbcontext = new MongoDBContext();

        internal void DeleteAVolunteersContracts(string id)
        {
            IMongoCollection<Contract> contractcollection = dbcontext.database.GetCollection<Contract>("Contracts");
            contractcollection.DeleteMany(Builders<Contract>.Filter.Eq("OwnerID", id));
        }
    }
}