using Finalaplication.App_Start;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Finalaplication.LocalDatabaseManager
{
    public class AuxiliaryDBManager
    {
        private MongoDBContext dBContext;

        public AuxiliaryDBManager(string SERVER_NAME, int SERVER_PORT, string DATABASE_NAME)
        {
            dBContext = new MongoDBContext(SERVER_NAME, SERVER_PORT, DATABASE_NAME);
        }

        internal void AddDocumenttoDB(string documentToAdd)
        {
            BsonDocument documentAsBson;
            BsonDocument.TryParse(documentToAdd, out documentAsBson);
            IMongoCollection<BsonDocument> AuxiliaryCollection = dBContext.Database.GetCollection<BsonDocument>("Auxiliary");
            AuxiliaryCollection.InsertOne(documentAsBson);
        }

        internal string GetDocumentByID(string id)
        {
            IMongoCollection<BsonDocument> auxiliarycollection = dBContext.Database.GetCollection<BsonDocument>("Auxiliary");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            BsonDocument returndocument = auxiliarycollection.Find(filter).FirstOrDefault();
            string returnstring = returndocument.ToString();
            return returnstring;
        }
        internal List<BsonDocument> GetListDocuments()
        {
            List<BsonDocument> auxiliarycollection = dBContext.Database.GetCollection<BsonDocument>("Auxiliary").AsQueryable<BsonDocument>().ToList() ;
          
            return auxiliarycollection;
        }

        internal void DropAuxiliaryDatabase()
        {
            dBContext.Database.DropCollection("Auxiliary");
        }
    }
}