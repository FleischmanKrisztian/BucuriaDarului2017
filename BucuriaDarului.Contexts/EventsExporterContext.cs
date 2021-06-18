﻿using System.Collections.Generic;

namespace BucuriaDarului.Contexts
{
    public class EventsExporterContext
    {
        public Dictionary<string, string> Execute(string stringOfIDs, bool all, bool allocatedSponsors, bool allocatedVolunteers, bool duration, bool typeOfEvent, bool nameOfEvent, bool placeOfEvent, bool dateOfEvent, bool typeOfActivities, string header)
        {
            string idsAndFields = GetIdAndFieldString(stringOfIDs, all, allocatedSponsors, allocatedVolunteers, duration, typeOfEvent, nameOfEvent, placeOfEvent, dateOfEvent, typeOfActivities);
            string key1 = Constants.EVENTSESSION;
            string key2 = Constants.EVENTHEADER;
            return CreateDictionaries(key1, key2, idsAndFields, header);
        }

        private string GetIdAndFieldString(string stringOfIDs, bool all, bool allocatedSponsors, bool allocatedVolunteers, bool duration, bool typeOfEvent, bool nameOfEvent, bool placeOfEvent, bool dateOfEvent, bool typeOfActivities)
        {
            {
                string ids_and_options = stringOfIDs + "(((";
                if (all)
                    ids_and_options += "0";
                if (nameOfEvent)
                    ids_and_options += "1";
                if (placeOfEvent)
                    ids_and_options += "2";
                if (dateOfEvent)
                    ids_and_options += "3";
                if (typeOfActivities)
                    ids_and_options += "4";
                if (typeOfEvent)
                    ids_and_options += "5";
                if (duration)
                    ids_and_options += "6";
                if (allocatedVolunteers)
                    ids_and_options += "7";
                if (allocatedSponsors)
                    ids_and_options += "8";
                return ids_and_options;
            }
        }

        private Dictionary<string, string> CreateDictionaries(string key1, string key2, string idsAndFields, string header)
        {
            var result = new Dictionary<string, string>();

            if (result.ContainsKey(key1) == true)
            {
                result[key1] = idsAndFields;
            }
            else
            {
                result.Add(key1, idsAndFields);
            }
            if (result.ContainsKey(key2) == true)
            {
                result[key2] = header;
            }
            else
            {
                result.Add(key2, header);
            }
            return result;
        }
    }
}