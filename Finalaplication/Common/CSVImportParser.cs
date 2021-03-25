using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Elm.Core.Parsers
{
    public class CSVImportParser
    {
        #region private functions

        private static readonly char[] SeparatorChars = new char[]
                                                        {
                                                            ';',
                                                            '!',
                                                            '\t'
                                                        };

        private static readonly char[] TrimChars = new char[]
                                                   {
                                                       '"',
                                                       '\'',
                                                       ' '
                                                   };

        private static readonly string SEPARATOR = ";";

        private static char AutoDetectColumnSeparator(string line)
        {
            Dictionary<char, int> sepCount = new Dictionary<char, int>(SeparatorChars.Length);

            foreach (char c in line)
            {
                foreach (char t in SeparatorChars)
                {
                    if (t == c)
                    {
                        if (!sepCount.ContainsKey(c))
                        {
                            sepCount[c] = 1;
                        }
                        else
                        {
                            sepCount[c]++;
                        }
                    }
                }
            }

            if (sepCount.Count == 0)
            {
                return SeparatorChars[0];
            }

            int max = sepCount.Values.Max();
            if (max > 0)
            {
                foreach (KeyValuePair<char, int> kvp in sepCount)
                {
                    if (kvp.Value == max)
                    {
                        return kvp.Key;
                    }
                }
            }

            return SeparatorChars[0];
        }



        public static string[] GetHeader(string filename)
        {
            string firstLine;
            using (StreamReader sr = new StreamReader(filename))
            {
                firstLine = sr.ReadLine();
            }

            string[] header = firstLine.Split(";");
            return header;
        }

        public static string TypeOfExport(string[] header)
        {
            string type = string.Empty;
            if (header.Count() >= 22 && header.Contains("Gender") == true)
            {
                type = "MyApp";
            }
            if (header[1].Contains("CNP") == true)
            {
                type = "BucuriaDarului";
            }
            if (header[2].Contains("Nume cap familie") == true)
            {
                type = "BucuriaDarului";
            }
            if (header[8].Contains("CNP") == true)
            {
                type = "MyApp";
            }
            return type;
        }
        public static List<string[]> GetListFromCSV(string filename)
        {
            List<string[]> list = new List<string[]>();

            using (StreamReader sr = new StreamReader(filename))
            {
                int totalLines = File.ReadAllLines(filename).Length;
                string line = string.Empty;
                string firstrow = sr.ReadLine();

                while ((line = sr.ReadLine()) != null)
                {
                    var ImportColumnSeparator = AutoDetectColumnSeparator(line);

                    string[] splits;
                    if (!line.Contains(SEPARATOR + ImportColumnSeparator))
                    {
                        splits = line.Split(new char[] { ImportColumnSeparator });
                    }
                    else
                    {
                        splits = line.Split(new string[]
                        {  SEPARATOR,
                            SEPARATOR + ImportColumnSeparator,
                            ImportColumnSeparator + SEPARATOR,
                        },
                        StringSplitOptions.None);
                    }

                    for (int i = 0; i < splits.Length; i++)
                    {
                        splits[i] = splits[i].Trim(TrimChars);
                    }

                    list.Add(splits);
                }

                return list;
            }
        }

        internal static bool DefaultVolunteerCSVFormat(string path)
        {
            string firstrow = "";
            using (StreamReader sr = new StreamReader(path))
            {
                int totalLines = File.ReadAllLines(path).Length;
                firstrow = sr.ReadLine();
            }
            if (firstrow.Contains("nume si prenume"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        internal static bool DefaultBeneficiaryCSVFormat(string path)
        {
            string firstrow = "";
            using (StreamReader sr = new StreamReader(path))
            {
                int totalLines = File.ReadAllLines(path).Length;
                firstrow = sr.ReadLine();
            }
            if (firstrow.Contains("prenume benef"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion private functions

        #region public functions

        #endregion public functions
    }
}