using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Finalaplication.Models
{
    public class Settings
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string settingID { get; set; }

        public string Lang { get; set; }

        public int Quantity { get; set; }
    }
}