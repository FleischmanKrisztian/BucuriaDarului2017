using ChoETL;
using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Text;

namespace CSVExporter.StringtoCsv
{
    internal class Methods
    {
        public static string VolunteersToCSVFormat(string result)
        {
            string csvasstring = "";
            //string csvasstring = "Firstname,Lastname,Birthdate,Gender,CNP,Occupation,Filed_of_activity,Desired_workplace,InActivity,HourCount,HasCar,HasDrivingLicence,Remark,District,City,Number,Street,MailAddres,PhoneNumber\n";
            csvasstring = csvasstring + jsontocsvlasttry(result);
            return csvasstring;
        }

        public static string BeneficiariesToCSVFormat(string result)
        {
            string csvasstring = "Firstname,Lastname,Active,Weekly package, Canteen, Home Delivery Driver, HAS GDPR,District,City,Street,Number,CNP,Has ID, IDSerie, IDNr, IDEliberat, IdEliberator, IDAplication, IDInvestigation, IDContract, Number Of Portions, Last Time Active, Comments, Birthdate, Phone Number,Birth place, Studies, Profession, Occupation, Seniority In Workfield, Health State,Disability,Chronic Condition, Addictions, Health Insurance,Health Card, Married, Spouse Name,Has Home, Housing Type,Income,Expenses,Gender,Has Contract, Number Of Registration, Registration Date,Expiration Date\n";
            csvasstring = csvasstring + jsonToCSV(result);
            return csvasstring;
        }

        public static string SponsorsToCSVFormat(string result)
        {
            string csvasstring = "NameOfSponsor,PhoneNumber,MailAdress,HasContract,NumberOfRegistration,RegistrationDate,ExpirationDate,DateOfSponsorships,MoneyAmount,WhatGoods,GoodsAmount\n";
            csvasstring = csvasstring + jsonToCSV(result);

            return csvasstring;
        }

        public static string EventsToCSVFormat(string result)
        {
            string csvasstring = "NameOfEvent,DateOfEvent,Duration,NumberOfVolunteersNeeded,PlaceOfEvent,TypeOfActivities,TypeOfEvent,AllocatedVolunteers,AllocatedSponsors\n";
            csvasstring = csvasstring + jsonToCSV(result);
            return csvasstring;
        }

        public static DataTable jsonStringToTable(string jsonContent)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonContent);
            return dt;
        }

        public static string jsontocsvlasttry(string jsonContent)
        {
            StringBuilder csv = new StringBuilder();
            using (var p = new ChoJSONReader(new StringReader(jsonContent)))
            {
                using (var w = new ChoCSVWriter(new StringWriter(csv))
                    .WithFirstLineHeader()
                    )
                {
                    w.Write(p);
                }
            }
            return csv.ToString();
        }

        //public static string jsonToCSV(string jsonContent)
        //{
        //    StringWriter csvString = new StringWriter();
        //    using (var csv = new CsvWriter(csvString))
        //    {
        //        using (var dt = jsonStringToTable(jsonContent))
        //        {
        //            foreach (DataColumn column in dt.Columns)
        //            {
        //                csv.WriteField(column.ColumnName);
        //            }
        //            csv.NextRecord();

        //            foreach (DataRow row in dt.Rows)
        //            {
        //                for (var i = 0; i < dt.Columns.Count; i++)
        //                {
        //                    csv.WriteField(row[i]);
        //                }
        //                csv.NextRecord();
        //            }
        //        }
        //    }
        //    return csvString.ToString();
        //}
        public static string jsonToCSV(string jsonContent, string delimiter = ",")
        {
            StringWriter csvString = new StringWriter();
            using (var csv = new CsvWriter(csvString))
            {
                //csv.Configuration.SkipEmptyRecords = true;
                //csv.Configuration.WillThrowOnMissingField = false;
                csv.Configuration.Delimiter = delimiter;

                using (var dt = jsonStringToTable(jsonContent))
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        csv.WriteField(column.ColumnName);
                    }
                    csv.NextRecord();

                    foreach (DataRow row in dt.Rows)
                    {
                        for (var i = 0; i < dt.Columns.Count; i++)
                        {
                            csv.WriteField(row[i]);
                        }
                        csv.NextRecord();
                    }
                }
            }
            return csvString.ToString();
        }
    }
}
