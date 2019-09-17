using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;

namespace protocolceva
{
    class Program
    {
        static void Main()
        {
            string[] args = Environment.GetCommandLineArgs();

            //args[0] is always the path to the application
            RegisterMyProtocol(args[0]);
            //^the method posted before, that edits registry      
            try
            {
                // Modify to suut your machine:
                string fileName = @"D:\GithubProjects\BucuriaDarului\protocolceva\protocolceva\Docxfiles\template.docx";

                // Create a document in memory:
                var doc = DocX.Load(fileName);

                doc.ReplaceText("<Fullname>", "Krisztian");
                doc.ReplaceText("<age>", "22");
                doc.ReplaceText("<hobby>", "fotbal");
                doc.ReplaceText("<adjectiv>", "frumos");
                doc.InsertParagraph(args[1]);

                // Save to the output directory:
                doc.Save();


                Console.WriteLine("Succesfully saved document");               
            }
            catch
            {
                Console.WriteLine("No argument(s)");  //if there's an exception, there's no argument
            }

            Console.ReadLine(); //pauses the program - so you can see the result
        }

        static void RegisterMyProtocol(string myAppPath)  //myAppPath = full path to your application
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey("myApp");  //open myApp protocol's subkey

            if (key == null)  //if the protocol is not registered yet...we register it
            {
                key = Registry.ClassesRoot.CreateSubKey("myApp");
                key.SetValue(string.Empty, "URL: myApp Protocol");
                key.SetValue("URL Protocol", string.Empty);

                key = key.CreateSubKey(@"shell\open\command");
                key.SetValue(string.Empty, myAppPath + " " + "%1");
                //%1 represents the argument - this tells windows to open this program with an argument / parameter
            }

            key.Close();
        }

    }


}
