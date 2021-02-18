using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using VolCommon;

namespace Finalaplication.Models
{
    public class Volunteer : VolunteerBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string VolunteerID { get; set; }

        public static int Volbd(Volunteer vol)
        {
            int voldays;
            {
                voldays = (vol.Birthdate.Month - 1) * 30 + vol.Birthdate.Day;
            }
            return voldays;
        }

        public static int Nowdate()
        {
            string todaydate = DateTime.Today.ToString("dd-MM-yyyy");
            string[] dates = todaydate.Split('-');
            int Day = Convert.ToInt16(dates[0]);
            int Month = Convert.ToInt16(dates[1]);
            int Year = Convert.ToInt16(dates[2]);
            /*Day = 28;
            Month = 12;
            Year = 2019;*/
            Day = (Month - 1) * 30 + Day;
            return Day;
        }
        
        public void MappHeader()
        {
            

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