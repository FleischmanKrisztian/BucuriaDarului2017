using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using VolCommon;

namespace Finalaplication.Models
{
    public class Sponsor : SponsorBase
    {
        [BsonId]
        [JsonIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public string SponsorID { get; set; }

        public static int Sponsorexp(Sponsor sponsor)
        {
            int sponsorexp;
            {
                sponsorexp = ((sponsor.Contract.ExpirationDate.Month - 1) * 30) + sponsor.Contract.ExpirationDate.Day;
            }
            return sponsorexp;
        }

        public bool GetDayExpiration(DateTime date)
        {
            var now = DateTime.Now;
            var firstday = now.AddDays(-1);
            var lastday = now.AddDays(10);
            var answer = false;
            if (date >= firstday && date <= lastday)
            {
                answer = true;
            }
            return answer;
        }
    }
}
