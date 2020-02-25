using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finalaplication.Common
{
    class DictionaryHelper
    {
        public static Dictionary<string, DictionaryHelper> d = new Dictionary<string, DictionaryHelper>();//this line is added.
        
        protected string ids;
       


        //Default constructor
        public DictionaryHelper()
        {
        }

        //Overload metode som tager 3 parametre
        public DictionaryHelper(string ids)
        {
            this.ids= ids;
            
        }

       

        public string Ids
        {
            set
            { ids = value; }
            get
            { return ids; }
        }

       

        public void Indsætdata()
        {

        }
    }

}

