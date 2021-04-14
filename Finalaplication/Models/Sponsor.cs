using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VolCommon;

namespace Finalaplication.Models
{
    public class Sponsor : SponsorBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string SponsorID { get; set; }
    }
}