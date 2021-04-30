
using System;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;

namespace BackupDatabaseApp
{
    class DatabaseMethods
    {
        
        internal string BackupDatabase(string filename)
        {
            string path  = Path.GetDirectoryName(filename);
            string navigate_to_mongo_command = "/c cd " + Environment.GetEnvironmentVariable(Common.MongoConstants.MONGO_Path);

            string backup_command = @"/k mongodump --host =" + Environment.GetEnvironmentVariable(Common.MongoConstants.SERVER_NAME_COMMON)
                + " --port =" + Environment.GetEnvironmentVariable(Common.MongoConstants.SERVER_PORT_COMMON)
                + " --db = " + Environment.GetEnvironmentVariable(Common.MongoConstants.DATABASE_NAME_COMMON)
                + " --viewsAsCollections "
                + "--archive = " + filename;


            ProcessStartInfo ps = new ProcessStartInfo();
            ps.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            ps.FileName = "cmd.exe";
            Process.Start(ps);




            string message = "Error!!";
            if (File.Exists(path))
            { message = "This action was completead succesfuly!"; }
            else
            { message = "Error!!!"; }
            
            return message;
        }

        internal string RestoreDatabase(string path)
        {
           
            string navigate_to_mongo_command = "/c cd " + Environment.GetEnvironmentVariable(Common.MongoConstants.MONGO_Path);

            string backup_command = @"/k mongorestore --host =" + Environment.GetEnvironmentVariable(Common.MongoConstants.SERVER_NAME_COMMON)
                + " --port =" + Environment.GetEnvironmentVariable(Common.MongoConstants.SERVER_PORT_COMMON)
                + " --db = " + Environment.GetEnvironmentVariable(Common.MongoConstants.DATABASE_NAME_COMMON)
                + "(${" + Environment.GetEnvironmentVariable(Common.MongoConstants.DATABASE_NAME_COMMON )+ "})  --archive =" + path;


            ProcessStartInfo ps = new ProcessStartInfo();
            ps.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            //ps.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            ps.FileName = "cmd.exe";
            Process.Start(ps);

            string message = "Please choose a valid file!";
            //if (path!=null)
            // message = "This action was completead succesfuly!";

            return message;
        }

        internal void DeleteDatabase(string dbname)
        {
            //string command= "mongo <"+ dbname +"> --eval "db.dropDatabase()"";
            string command="mongo";
            ProcessStartInfo ps = new ProcessStartInfo();
            ps.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            //ps.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            ps.FileName = "cmd.exe";
            Process.Start("cmd.exe", command);
        }
    }
}
