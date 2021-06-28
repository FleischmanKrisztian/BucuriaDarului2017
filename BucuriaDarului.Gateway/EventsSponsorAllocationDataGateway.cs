﻿using System;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace BucuriaDarului.Gateway
{
    public class EventsSponsorAllocationDataGateway : IEventSponsorAllocationDisplayGateway
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

        public List<Event> GetListOfEvents()
        {
            dBContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Event> eventCollection = dBContext.Database.GetCollection<Event>("Events");
            List<Event> events = eventCollection.AsQueryable().ToList();
            return events;
        }
        public List<Sponsor> GetListOfSponsors()
        {
            dBContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Sponsor> sponsorCollection = dBContext.Database.GetCollection<Sponsor>("Sponsors");
            List<Sponsor> sponsors = sponsorCollection.AsQueryable().ToList();
            return sponsors;
        }

        public void UpdateEvent(string eventId, Event eventToUpdate)
        {
            dBContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Event> eventcollection = dBContext.Database.GetCollection<Event>("Events");
            var filter = Builders<Event>.Filter.Eq("_id", eventId);
            eventToUpdate._id = eventId;
            ModifiedIDGateway modifiedIDGateway = new ModifiedIDGateway();
            modifiedIDGateway.AddIDtoModifications(eventId);
            eventcollection.FindOneAndReplace(filter, eventToUpdate);

        }
    }
}