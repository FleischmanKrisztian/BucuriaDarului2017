using System.Collections.Generic;
using BucuriaDarului.Core;
using MongoDB.Driver;

namespace BucuriaDarului.Gateway.VolunteerContractGateways
{
    public class ListVolunteerContractGateway
    {
        public static List<VolunteerContract> GetListVolunteerContracts()
        {
            var dbContext = new MongoDBGateway();
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var volunteerContractCollection = dbContext.Database.GetCollection<VolunteerContract>("VolunteerContracts");
            var contracts = volunteerContractCollection.AsQueryable().ToList();
            return contracts;
        }
    }
}
