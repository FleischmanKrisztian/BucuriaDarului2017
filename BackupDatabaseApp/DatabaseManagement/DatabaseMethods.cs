using BackupDatabaseApp.DatabaseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupDatabaseApp
{
    class DatabaseMethods
    {
        private MongoDBContextCommon dbContextCommon = new MongoDBContextCommon();

        internal string BackupDatabase(string filename)
        {
            string message = "Please choose a valid file!";
            if (filename!= null)
                message = "This action was completead succesfuly!";

            return message;
        }

        internal string RestoreDatabase(string path)
        {
            string message = "Please choose a valid file!";
            if (path!=null)
             message = "This action was completead succesfuly!";

            return message;
        }

        internal void DeleteDatabase()
        { 

        }
    }
}
