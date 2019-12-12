using ChoETL;
using CsvHelper;
using Newtonsoft.Json;
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
            csvasstring = csvasstring + jsontocsvlasttry(result);
            return csvasstring;
        }

        public static string BeneficiariesToCSVFormat(string result)
        {
            string csvasstring = "";
            csvasstring = csvasstring + jsontocsvlasttry(result);
            return csvasstring;
        }

        public static string SponsorsToCSVFormat(string result)
        {
            string csvasstring = "";
            csvasstring = csvasstring + jsontocsvlasttry(result);

            return csvasstring;
        }

        public static string EventsToCSVFormat(string result)
        {
            string csvasstring = "";
            csvasstring = csvasstring + jsontocsvlasttry(result);
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
                    .WithDelimiter(";")
                    )
                {
                    w.Write(p);
                }
            }
            return csv.ToString();
        }

        public static string jsonToCSV(string jsonContent, string delimiter = ";")
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
