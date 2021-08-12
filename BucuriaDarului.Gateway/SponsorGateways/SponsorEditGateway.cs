using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.SponsorGateways;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.SponsorGateways
{
    public class SponsorEditGateway : ISponsorEditGateway
    {
        private readonly MongoDBGateway dbContext = new MongoDBGateway();

        public void AddSponsorToModifiedList(string beforeEditingSponsorString)
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            BsonDocument.TryParse(beforeEditingSponsorString, out var documentAsBson);
            var auxiliaryCollection = dbContext.Database.GetCollection<BsonDocument>("Auxiliary");
            auxiliaryCollection.InsertOne(documentAsBson);
        }

        public void Edit(Sponsor sponsor)
        {
            var modifiedIdGateway = new ModifiedIDGateway();
            var sponsorCollection = dbContext.Database.GetCollection<Sponsor>("Sponsors");
            var filter = Builders<Sponsor>.Filter.Eq("Id", sponsor.Id);
            modifiedIdGateway.AddIDtoModifications(sponsor.Id);
            sponsorCollection.FindOneAndReplace(filter, sponsor);
        }

        public List<ModifiedIDs> ReturnModificationList()
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var modifiedCollection = dbContext.Database.GetCollection<ModifiedIDs>("ModifiedIDS");
            var modifiedIDs = modifiedCollection.AsQueryable().ToList();
            return modifiedIDs;
        }

        public Sponsor ReturnSponsor(string id)
        {
            return SingleSponsorReturnerGateway.ReturnSponsor(id);
        }
    }
}