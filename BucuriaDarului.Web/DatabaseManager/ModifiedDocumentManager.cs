using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Finalaplication.LocalDatabaseManager
{
    public class ModifiedDocumentManager
    {
        private MongoDBContext dbContext = new MongoDBContext(
          Environment.GetEnvironmentVariable(Common.Constants.SERVER_NAME_LOCAL),
          int.Parse(Environment.GetEnvironmentVariable(Common.Constants.SERVER_PORT_LOCAL)),
          Environment.GetEnvironmentVariable(Common.Constants.DATABASE_NAME_LOCAL));

        public void AddIDtoString(string id)
        {
            IMongoCollection<ModifiedIDs> modifiedIDS = dbContext.Database.GetCollection<ModifiedIDs>("ModifiedIDS");
            ModifiedIDs modified = new ModifiedIDs
            {
                _id = Guid.NewGuid().ToString(),
                ModifiedId = id
            };
            modifiedIDS.InsertOne(modified);
        }

        internal void AddIDtoDeletionString(string id)
        {
            IMongoCollection<DeletedIDS> deletedIDS = dbContext.Database.GetCollection<DeletedIDS>("DeletedIDS");
            DeletedIDS deleted = new DeletedIDS
            {
                _id = Guid.NewGuid().ToString(),
                DeletedId = id
            };
            deletedIDS.InsertOne(deleted);
        }

        public List<ModifiedIDs> GetListOfModifications()
        {
            IMongoCollection<ModifiedIDs> modifiedcollection = dbContext.Database.GetCollection<ModifiedIDs>("ModifiedIDS");
            List<ModifiedIDs> modifiedIDs = modifiedcollection.AsQueryable().ToList();
            return modifiedIDs;
        }

        internal List<DeletedIDS> GetListOfDeletions()
        {
            IMongoCollection<DeletedIDS> deletedcollection = dbContext.Database.GetCollection<DeletedIDS>("DeletedIDS");
            List<DeletedIDS> deletedIDs = deletedcollection.AsQueryable().ToList();
            return deletedIDs;
        }

        internal void DeleteAuxiliaryDatabases()
        {
            dbContext.Database.DropCollection("DeletedIDS");
            dbContext.Database.DropCollection("ModifiedIDS");
        }
    }
}