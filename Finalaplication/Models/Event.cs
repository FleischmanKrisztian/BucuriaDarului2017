using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using VolCommon;
using Newtonsoft.Json;

namespace Finalaplication.Models
{
    public class Event : EventBase
    {
        [JsonIgnore][BsonId]
        public ObjectId EventID { get; set; }
    }
}