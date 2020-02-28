using System.Collections.Generic;

namespace Finalaplication.Common
{
    internal class DictionaryHelper
    {
        public static Dictionary<string, DictionaryHelper> d = new Dictionary<string, DictionaryHelper>();

        protected string ids;

        
        public DictionaryHelper()
        {
        }

        
        public DictionaryHelper(string ids)
        {
            this.ids = ids;
        }

        public string Ids
        {
            set
            { ids = value; }
            get
            { return ids; }
        }
    }
}