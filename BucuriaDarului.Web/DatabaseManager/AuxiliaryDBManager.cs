using MongoDB.Bson;
using MongoDB.Driver;

namespace BucuriaDarului.Web.DatabaseManager
{
    public class AuxiliaryDBManager
    {
        private MongoDBContext dbContext;

        public AuxiliaryDBManager(string SERVER_NAME, int SERVER_PORT, string DATABASE_NAME)
        {
            dbContext = new MongoDBContext(SERVER_NAME, SERVER_PORT, DATABASE_NAME);
        }

        internal void AddDocumenttoDB(string documentToAdd)
        {
            BsonDocument documentAsBson;
            BsonDocument.TryParse(documentToAdd, out documentAsBson);
            IMongoCollection<BsonDocument> AuxiliaryCollection = dbContext.Database.GetCollection<BsonDocument>("Auxiliary");
            AuxiliaryCollection.InsertOne(documentAsBson);
        }

        internal string GetDocumentByID(string id)
        {
            IMongoCollection<BsonDocument> auxiliarycollection = dbContext.Database.GetCollection<BsonDocument>("Auxiliary");
            var filter = Builders<BsonDocument>.Filter.Eq("Id", id);
            BsonDocument returndocument = auxiliarycollection.Find(filter).FirstOrDefault();
            string returnstring = returndocument.ToString();
            return returnstring;
        }

        internal void DropAuxiliaryDatabase()
        {
            dbContext.Database.DropCollection("Auxiliary");
        }
    }
}