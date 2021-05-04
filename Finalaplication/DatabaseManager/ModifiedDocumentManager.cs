using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Finalaplication.LocalDatabaseManager
{
    public class ModifiedDocumentManager
    {
        private MongoDBContext dBContext = new MongoDBContext(
          Environment.GetEnvironmentVariable(Common.VolMongoConstants.SERVER_NAME_LOCAL),
          int.Parse(Environment.GetEnvironmentVariable(Common.VolMongoConstants.SERVER_PORT_LOCAL)),
          Environment.GetEnvironmentVariable(Common.VolMongoConstants.DATABASE_NAME_LOCAL));

        public void AddIDtoString(string id)
        {
            IMongoCollection<ModifiedIDs> modifiedIDS = dBContext.Database.GetCollection<ModifiedIDs>("ModifiedIDS");
            ModifiedIDs modified = new ModifiedIDs();
            modified._id = Guid.NewGuid().ToString();
            modified.ModifiedId = id;
            modifiedIDS.InsertOne(modified);
        }

        internal void AddIDtoDeletionString(string id)
        {
            IMongoCollection<DeletedIDS> deletedIDS = dBContext.Database.GetCollection<DeletedIDS>("DeletedIDS");
            DeletedIDS deleted = new DeletedIDS();
            deleted._id = Guid.NewGuid().ToString();
            deleted.DeletedId = id;
            deletedIDS.InsertOne(deleted);
        }

        public List<ModifiedIDs> GetListOfModifications()
        {
            IMongoCollection<ModifiedIDs> modifiedcollection = dBContext.Database.GetCollection<ModifiedIDs>("ModifiedIDS");
            List<ModifiedIDs> modifiedIDs = modifiedcollection.AsQueryable().ToList();
            return modifiedIDs;
        }

        internal List<DeletedIDS> GetListOfDeletions()
        {
            IMongoCollection<DeletedIDS> deletedcollection = dBContext.Database.GetCollection<DeletedIDS>("DeletedIDS");
            List<DeletedIDS> deletedIDs = deletedcollection.AsQueryable().ToList();
            return deletedIDs;
        }

        internal void DeleteAuxiliaryDatabases()
        {
            dBContext.Database.DropCollection("DeletedIDS");
            dBContext.Database.DropCollection("ModifiedIDS");
        }
    }
}