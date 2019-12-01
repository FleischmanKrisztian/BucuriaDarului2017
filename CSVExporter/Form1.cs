﻿using Microsoft.Win32;
using System;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;

namespace CSVExporter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string[] args = Environment.GetCommandLineArgs();
            //args[0] is always the path to the application
            RegisterMyProtocol(args[0]);
            //^the method posted before, that edits registry
        }

        private static void RegisterMyProtocol(string CSVExporterappPath)  //myAppPath = full path to your application
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey("CSVExporterapp");  //open myApp protocol's subkey

            if (key == null)  //if the protocol is not registered yet...we register it
            {
                key = Registry.ClassesRoot.CreateSubKey("CSVExporterapp");
                key.SetValue(string.Empty, "URL: CSVExporterapp Protocol");
                key.SetValue("URL Protocol", string.Empty);
                key = key.CreateSubKey(@"shell\open\command");
                key.SetValue(string.Empty, CSVExporterappPath + " " + "%1");
                //%1 represents the argument - this tells windows to open this program with an argument / parameter
            }

            key.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            Stream myStream;
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    myStream.Close();
                    string strfilename = saveFileDialog1.FileName;
                    richTextBox1.Text = strfilename;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string[] args = Environment.GetCommandLineArgs();
                HttpClient httpClient = new HttpClient();
                args[1] = args[1].Remove(0, 15);
                //probabil trebuie modificat
                string url = "http://localhost:5000/api/ExcelPrinter/" + args[1];
                var result = httpClient.GetStringAsync(url).Result.Normalize();
                string path = richTextBox1.Text;
                string csvasstring = "";
                if (args[1].Contains("volunteers"))
                {
                    csvasstring = StringtoCsv.Methods.VolunteersToCSVFormat(result);
                }
                if (args[1].Contains("beneficiaries"))
                {
                    csvasstring = StringtoCsv.Methods.BeneficiariesToCSVFormat(result);
                }
                if (args[1].Contains("sponsors"))
                {
                    csvasstring = StringtoCsv.Methods.SponsorsToCSVFormat(result);
                }
                if (args[1].Contains("events"))
                {
                    csvasstring = StringtoCsv.Methods.EventsToCSVFormat(result);
                }
                //File.WriteAllText(path, "Name,Email,Phone Number,Address
                System.IO.StreamWriter objWriter;
                objWriter = new StreamWriter(path);
                objWriter.Write(csvasstring);
                objWriter.Close();
                richTextBox2.Text = "Your file has been successfully saved";
            }
            catch
            {
                richTextBox2.Text = "An error has been encountered";
            }
        }
    }
}