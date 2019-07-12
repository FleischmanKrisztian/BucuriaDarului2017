using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VolCommon;

namespace Finalaplication.Models
{
    public enum TypeOfSupport
    {
        Money, Goods
    }
    public class Sponsor
    {
        [BsonId]
        public ObjectId SponsorID { get; set; }
        public string NameOfSponsor { get; set; }

        public Sponsorship Sponsorships { get; set; }

        public ContactInformation Contact { get; set; }

        public Contract Contract { get; set; }

        public static int Sponsorexp(Sponsor sponsor)
        {
            int sponsorexp;
            {
                sponsorexp = (sponsor.Contract.ExpirationDate.Month - 1) * 30 + sponsor.Contract.ExpirationDate.Day;
            }
            return sponsorexp;
        }
    }
}

       

    

