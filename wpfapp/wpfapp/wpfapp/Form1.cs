﻿using System;
using System.IO;
using System.Windows.Forms;
using System.Net.Http;
using Xceed.Words.NET;
using Newtonsoft.Json;
using Microsoft.Win32;
using static System.Net.WebRequestMethods;

namespace wpfapp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            //args[0] is always the path to the application
            RegisterMyProtocol(args[0]);
            //^the method posted before, that edits registry 
            
            Stream myStream;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if((myStream = openFileDialog1.OpenFile()) != null)
                {
                    string strfilename = openFileDialog1.FileName;
                    richTextBox1.Text = strfilename;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string[] args = Environment.GetCommandLineArgs();
                    RegisterMyProtocol(args[0]);
                    var doc = DocX.Load(richTextBox1.Text);
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
                    if (volc.CNP != null)
                        doc.ReplaceText("<cnp>", volc.CNP);
                    if (volc.Address != null)
                        doc.ReplaceText("<address>", volc.Address);
                    doc.ReplaceText("<nrofreg>", volc.NumberOfRegistration.ToString());
                    doc.ReplaceText("<startdate>", volc.RegistrationDate.ToString());
                    doc.ReplaceText("<expdate>", volc.ExpirationDate.ToString());
                    doc.SaveAs(saveFileDialog1.FileName);
                    richTextBox2.Text = saveFileDialog1.FileName;
                    richTextBox3.Text = "File Saved succesfully";

                }
            }
            catch
            {
                richTextBox3.Text = "an error has occured";
            }
        }
        

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

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

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
