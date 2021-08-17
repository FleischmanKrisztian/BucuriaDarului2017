using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.VolunteerContractGateways
{
    public class VolunteerContractDeleteGateway: IVolunteerContractDeleteGateway
    {
        public void Delete(string id)
        {
            var dbContext = new MongoDBGateway();
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var volunteerContractCollection = dbContext.Database.GetCollection<VolunteerContract>("VolunteerContracts");
            var filter = Builders<VolunteerContract>.Filter.Eq("Id", id);
            var deletedIdGateway = new DeletedIDGateway();
            deletedIdGateway.AddIDtoDeletions(id);
            volunteerContractCollection.DeleteOne(filter);
        }

        public  List<VolunteerContract> GetListVolunteerContracts()
        {
            var dbContext = new MongoDBGateway();
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var volunteerContractCollection = dbContext.Database.GetCollection<VolunteerContract>("VolunteerContracts");
            List<VolunteerContract> contracts = volunteerContractCollection.AsQueryable().ToList();
            return contracts;
        }
    }
}
