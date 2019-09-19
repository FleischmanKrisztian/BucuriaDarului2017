using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;
using static System.Net.WebRequestMethods;

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
            //try
            //{
                string fileName = @"D:\GithubProjects\BucuriaDarului\protocolceva\protocolceva\Docxfiles\template.docx";
                var doc = DocX.Load(fileName);
                HttpClient httpClient = new HttpClient();
                args[1] = args[1].Remove(0, 6);
                string url = "https://localhost:44395/api/Values/" + args[1];
                var result = httpClient.GetStringAsync(url).Result.Normalize();
                result = result.Replace("[", "");
                result = result.Replace("]", "");
                volcontract volc = new volcontract();
                volc = JsonConvert.DeserializeObject<volcontract>(result);


                doc.ReplaceText("<fullname>", volc.Firstname + " " + volc.Lastname);
                doc.ReplaceText("<birthdate>", volc.Birthdate.ToString());
                if(volc.CNP!=null)
                doc.ReplaceText("<cnp>", volc.CNP);
                if(volc.Address !=null)
                doc.ReplaceText("<address>", volc.Address);
                doc.ReplaceText("<nrofreg>", volc.NumberOfRegistration.ToString());
                doc.ReplaceText("<startdate>", volc.RegistrationDate.ToString());
                doc.ReplaceText("<expdate>", volc.ExpirationDate.ToString());
                doc.SaveAs(@"D:\GithubProjects\BucuriaDarului\protocolceva\protocolceva\Docxfiles\Contractul" + volc.NumberOfRegistration.ToString() + ".docx");

                Console.WriteLine("Succesfully saved document");
            //}
            //catch
            //{
            //    Console.WriteLine("No argument(s)");  //if there's an exception, there's no argument
            //}

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
