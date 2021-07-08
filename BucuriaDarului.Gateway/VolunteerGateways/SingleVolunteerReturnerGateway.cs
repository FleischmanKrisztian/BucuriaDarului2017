using BucuriaDarului.Core;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Gateway.VolunteerGateways
{
    public class SingleVolunteerReturnerGateway
    {
        public static Volunteer ReturnVolunteer(string id)
        {
            var dbContext = new MongoDBGateway();

            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var volunteerCollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            var filter = Builders<Volunteer>.Filter.Eq("Id", id);
            return volunteerCollection.Find(filter).FirstOrDefault();
        }
    }
}
