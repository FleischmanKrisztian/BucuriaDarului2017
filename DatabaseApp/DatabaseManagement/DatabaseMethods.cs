using System;
using System.Diagnostics;
using System.IO;

namespace BackupDatabaseApp
{
    internal class DatabaseMethods
    {
        internal static void BackupDatabase(string filename)
        {
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.RedirectStandardInput = true;
            info.UseShellExecute = false;

            p.StartInfo = info;
            p.Start();

            string pathtomongodump = Environment.GetEnvironmentVariable(Common.MongoConstants.MONGO_PATH);
            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("cd " + pathtomongodump);
                    var dumpCommand = "mongodump --host " + Environment.GetEnvironmentVariable(Common.MongoConstants.SERVER_NAME_COMMON) + " --port " + Environment.GetEnvironmentVariable(Common.MongoConstants.SERVER_PORT_COMMON) + " -d " + Environment.GetEnvironmentVariable(Common.MongoConstants.DATABASE_NAME_COMMON) + " --out " + filename;
                    sw.WriteLine(dumpCommand);
                }
            }
        }

        internal static void RestoreDatabase(string filename)
        {
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.RedirectStandardInput = true;
            info.UseShellExecute = false;

            p.StartInfo = info;
            p.Start();

            string pathcommand = "cd " + Environment.GetEnvironmentVariable(Common.MongoConstants.MONGO_PATH);
            string restorecommand = "mongorestore --host " + Environment.GetEnvironmentVariable(Common.MongoConstants.SERVER_NAME_COMMON) + " --port " + Environment.GetEnvironmentVariable(Common.MongoConstants.SERVER_PORT_COMMON) + " -d " + Environment.GetEnvironmentVariable(Common.MongoConstants.DATABASE_NAME_COMMON) + " " + filename;

            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine(pathcommand);
                    sw.WriteLine(restorecommand);
                }
            }
        }

        internal static void DeleteDatabase(string databasename)
        {
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.RedirectStandardInput = true;
            info.UseShellExecute = false;

            p.StartInfo = info;
            p.Start();

            string pathcommand = "cd " + Environment.GetEnvironmentVariable(Common.MongoConstants.MONGO_PATH);

            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine(pathcommand);
                    sw.WriteLine("mongo");
                    sw.WriteLine("use " + databasename);
                    sw.WriteLine("db.dropDatabase()");
                }
            }
        }
    }
}