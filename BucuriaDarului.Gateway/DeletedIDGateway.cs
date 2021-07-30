using BucuriaDarului.Core;
using MongoDB.Driver;
using System;

namespace BucuriaDarului.Gateway
{
    public class DeletedIDGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();
        internal static string SERVER_NAME_LOCAL = Environment.GetEnvironmentVariable("volmongo_server");
        internal static int SERVER_PORT_LOCAL = int.Parse(Environment.GetEnvironmentVariable("volmongo_port"));
        internal static string DATABASE_NAME_LOCAL = Environment.GetEnvironmentVariable("volmongo_databasename");

        public void AddIDtoDeletions(string id)
        {
            dbContext.ConnectToDB(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);
            IMongoCollection<DeletedIds> deletionCollection = dbContext.Database.GetCollection<DeletedIds>("DeletedIDS");
            DeletedIds deleted = new DeletedIds();
            deleted.Id = Guid.NewGuid().ToString();
            deleted.DeletedId = id;
            deletionCollection.InsertOne(deleted);
        }
    }
}