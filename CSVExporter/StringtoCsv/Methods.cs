using ChoETL;
using CsvHelper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;

namespace CSVExporter.StringtoCsv
{
    public class Methods
    {
        public static string JsontoCSV(string result)
        {
            string csvasstring = "";
            csvasstring = csvasstring + Jsontocsvlasttry(result);
            return csvasstring;
        }

        public static DataTable JsonStringToTable(string jsonContent)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonContent);
            return dt;
        }

        public static string Jsontocsvlasttry(string jsonContent)
        {
            StringBuilder csv = new StringBuilder();
            using (var p = new ChoJSONReader(new StringReader(jsonContent)))
            {
                using (var w = new ChoCSVWriter(new StringWriter(csv))
                    .WithFirstLineHeader()
                    .WithDelimiter(";")
                    )
                {
                    w.Write(p);
                }
            }
            return csv.ToString();
        }

        public static string JsonToCSV(string jsonContent, string delimiter = ";")
        {
            StringWriter csvString = new StringWriter();
            using (var csv = new CsvWriter((ISerializer)csvString))
            {
                //csv.Configuration.SkipEmptyRecords = true;
                //csv.Configuration.WillThrowOnMissingField = false;
                csv.Configuration.Delimiter = delimiter;

                using (var dt = JsonStringToTable(jsonContent))
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
        public static Dictionary<string,string> Xmldecoder()
        {
            //Loading Xml
            string xmlPath = @"D:\Bucuria Darului\ConfigFile.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);

            // Getting Node
            XmlNodeList locationDetails = xmlDoc.SelectNodes("configuration/LinkValues");
            Dictionary<string, string> dicPlacePinXml = new Dictionary<string, string>();

            //looping
            foreach (XmlNode locationNode in locationDetails)
            {
                foreach (XmlNode elementLoc in locationNode.ChildNodes)
                {
                    dicPlacePinXml.Add(elementLoc.Attributes["key"].Value, elementLoc.Attributes["value"].Value);
                }
            }
            return dicPlacePinXml;
        }
    }
}
