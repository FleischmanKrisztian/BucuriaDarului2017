using BucuriaDarului.Core;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.VolunteerContractGateways
{
    public class ListVolunteerAdditionalContractGateway
    {
        public static List<AdditionalContractVolunteer> GetListAdditionalContracts()
        {
            var dbContext = new MongoDBGateway();
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var additionalContractCollection = dbContext.Database.GetCollection<AdditionalContractVolunteer>("VolunteerAdditionalContracts");
            var contracts = additionalContractCollection.AsQueryable().ToList();
            return contracts;
        }
    }
}