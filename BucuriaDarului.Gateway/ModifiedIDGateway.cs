using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using MongoDB.Driver;
using System;

namespace BucuriaDarului.Gateway
{
    public class ModifiedIDGateway : IModifiedIDGateway
    {
        MongoDBGateway dBContext = new MongoDBGateway();
        internal static string SERVER_NAME_LOCAL = Environment.GetEnvironmentVariable("volmongo_server");
        internal static int SERVER_PORT_LOCAL = int.Parse(Environment.GetEnvironmentVariable("volmongo_port"));
        internal static string DATABASE_NAME_LOCAL = Environment.GetEnvironmentVariable("volmongo_databasename");

        public void AddIDtoModifications(string id)
        {
            dBContext.ConnectToDB(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);
            IMongoCollection<ModifiedIDs> modificationCollection = dBContext.Database.GetCollection<ModifiedIDs>("ModifiedIDS");
            ModifiedIDs modified = new ModifiedIDs();
            modified._id = Guid.NewGuid().ToString();
            modified.ModifiedId = id;
            modificationCollection.InsertOne(modified);
        }
    }
}