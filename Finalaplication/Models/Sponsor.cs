using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VolCommon;

namespace Finalaplication.Models
{
   
    public class Sponsor:SponsorBase
    {
        [BsonId]
        public ObjectId SponsorID { get; set; }
       

        public static int Sponsorexp(Sponsor sponsor)
        {
            int sponsorexp;
            {
                sponsorexp = ((sponsor.Contract.ExpirationDate.Month - 1) * 30) + sponsor.Contract.ExpirationDate.Day;
            }
            return sponsorexp;
        }
    }
}

       

    

