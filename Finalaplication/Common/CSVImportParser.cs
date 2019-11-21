//using Elm.Core.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Elm.Core.Parsers
{
    public class CSVImportParser : Parser
    {
        #region private functions

        private readonly char[] SeparatorChars = new char[]
                                                        {   
                                                            
                                                            ';',
                                                            '!',
                                                            '\t'
                                                        };

        private readonly char[] TrimChars = new char[]
                                                   {
                                                       '"',
                                                       '\'',
                                                       ' '
                                                   };

        private readonly string SEPARATOR = ";" ;
        

        private char AutoDetectColumnSeparator(string line)
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

        public List<string[]> ExtractDataFromFile(string filename)
        {
            List<string[]> list = new List<string[]>();
            RowsData = new List<string[]>();

            using (StreamReader sr = new StreamReader(filename))
            {
                int totalLines = File.ReadAllLines(filename).Length;
                int lineCnt = 0;
                string line = string.Empty;
                string firstrow = sr.ReadLine();


                while ((line = sr.ReadLine()) != null)
                {
                   
                    var ImportColumnSeparator = AutoDetectColumnSeparator(line);

                    string[] splits;
                    if (!line.Contains(SEPARATOR + ImportColumnSeparator) )
                    {
                        splits = line.Split(new char[] { ImportColumnSeparator });
                    }
                    else
                    {
                        splits = line.Split(new string[]
                        {  SEPARATOR,
                            SEPARATOR + ImportColumnSeparator,
                            ImportColumnSeparator + SEPARATOR,
                            //SEPARATOR2+ImportColumnSeparator,
                            //ImportColumnSeparator+SEPARATOR2,
                            //SEPARATOR2
                        },
                        StringSplitOptions.None);
                    }

                    for (int i = 0; i < splits.Length; i++)
                    {
                        splits[i] = splits[i].Trim(TrimChars);
                    }

                    if (lineCnt == 0)
                    {
                        HeaderData = new List<string>(splits);
                        lineCnt++;
                    }
                    else
                    {
                        if (HeaderData.Count == splits.Length)
                            RowsData.Add(splits);
                    }

                    list.Add(splits);
                }

                return list;
            }
        }

        #endregion private functions

        #region public functions

        public CSVImportParser(string filename)
        {
            try
            {
                this.RawData = this.ExtractDataFromFile(filename);
            }
            catch (Exception ex)
            {
                //ApplicationLogger.Logger.ErrorFormat("CSVImportParser function exit with message: {0}", ex.Message);
            }
        }

        #endregion public functions
    }
}