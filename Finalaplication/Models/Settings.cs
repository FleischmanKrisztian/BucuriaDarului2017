using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finalaplication.Models
{
    public class Settings
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string settingID { get; set; }

        public string Lang { get; set; }

        public string Env { get; set; }

        public int Quantity { get; set; }
    }
}
