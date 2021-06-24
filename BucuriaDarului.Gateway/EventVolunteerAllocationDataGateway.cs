using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Gateway
{
    public class EventVolunteerAllocationDataGateway: IEventVolunteerAllocationDisplayGateway
    {
        private MongoDBGateway dBContext = new MongoDBGateway();

        public Event GetEvent(string eventId)
        {
            dBContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Event> eventCollection = dBContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("_id", eventId);
            Event returnevent = eventCollection.Find(filter).FirstOrDefault();
            return returnevent;
           
        }

        public List<Volunteer> GetListOfVolunteers()
        {
            dBContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Volunteer> volunteerCollection = dBContext.Database.GetCollection<Volunteer>("Volunteers");
            List<Volunteer> volunteers = volunteerCollection.AsQueryable().ToList();
            return volunteers;
        }

    }
  }

