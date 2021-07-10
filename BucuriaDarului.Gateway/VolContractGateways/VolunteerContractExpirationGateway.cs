using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.VolContractGateways
{
    public class VolunteerContractExpirationGateway : IListDisplayVolunterContractsGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public List<VolunteerContract> GetListVolunteerContracts()
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var volunteerContractCollection = dbContext.Database.GetCollection<VolunteerContract>("Contracts");
            List<VolunteerContract> contracts = volunteerContractCollection.AsQueryable().ToList();
            return contracts;
        }
    }
}