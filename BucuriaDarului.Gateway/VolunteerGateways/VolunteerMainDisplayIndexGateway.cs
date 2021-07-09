using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Gateway.VolunteerGateways
{
    public class VolunteerMainDisplayIndexGateway : IVolunteerMainDisplayIndexGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();
        public List<Volunteer> GetListOfVolunteers()
        {

            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var volunteerCollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            var volunteers= volunteerCollection.AsQueryable().ToList();
            return volunteers;
        }
    }
}
