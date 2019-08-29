using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using VolCommon;
using Newtonsoft.Json;

namespace Finalaplication.Models
{
    public class Event : EventBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string EventID { get; set; }
    }
}