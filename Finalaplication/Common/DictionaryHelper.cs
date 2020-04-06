using System.Collections.Generic;

namespace Finalaplication.Common
{
    public static class DictionaryHelper
    {

        public static Dictionary<string, string> d = new Dictionary<string, string>();

        public static string GetPlural(string word)
        {
            // Try to get the result in the static Dictionary
            string result;
            if (d.TryGetValue(word, out result))
            {
                return result;
            }
            else
            {
                return null;
            }

        }
    }
}