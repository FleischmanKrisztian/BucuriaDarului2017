using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using VolCommon;

namespace Finalaplication.Models
{
    public class Volunteer : VolunteerBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string VolunteerID { get; set; }
    }
}