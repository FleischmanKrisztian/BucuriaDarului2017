using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;
using Xceed.Words.NET;

namespace wpfapp
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

        private void button1_Click(object sender, EventArgs e)
        {
            Stream myStream;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if ((myStream = openFileDialog1.OpenFile()) != null)
                {
                    string strfilename = openFileDialog1.FileName;
                    richTextBox1.Text = strfilename;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (richTextBox1.Text.Contains("ContractBeneficiar") == true || richTextBox1.Text.Contains("Contract_cadru_asistati_Fundatie") == true || richTextBox1.Text.Contains("beneficiar") == true || richTextBox1.Text.Contains("Beneficiar") == true)
                    {
                        string[] args = Environment.GetCommandLineArgs();
                        RegisterMyProtocol(args[0]);
                        var doc = DocX.Load(richTextBox1.Text);
                        HttpClient httpClient = new HttpClient();
                        args[1] = args[1].Remove(0, 16);
                         string url = "http://localhost:5000/api/BeneficiaryValues/" + args[1];
                       // string url = "https://localhost:44395/api/BeneficiaryValues/" + args[1];
                        var result = httpClient.GetStringAsync(url).Result.Normalize();
                        result = result.Replace("[", "");
                        result = result.Replace("]", "");
                        beneficiarycontract volc = new beneficiarycontract();
                        volc = JsonConvert.DeserializeObject<beneficiarycontract>(result);

                        try
                        {
                            string phrase = volc.Address;
                            string[] words = phrase.Split(',');
                        }
                        catch { }
                        doc.ReplaceText("<nrreg>", volc.NumberOfRegistration);
                        doc.ReplaceText("<todaydate>", volc.RegistrationDate.ToShortDateString());
                        doc.ReplaceText("<Fullname>", volc.Fullname);
                        if (volc.CNP != null)
                            doc.ReplaceText("<CNP>", volc.CNP);
                        if (volc.CIinfo != null)
                            doc.ReplaceText("<CiInfo>", volc.CIinfo);
                        if (volc.Address != null)
                            doc.ReplaceText("<Address>", volc.Address);
                        if (volc.Nrtel != null)
                            doc.ReplaceText("<tel>", volc.Nrtel);
                        if (volc.IdApplication != null)
                            doc.ReplaceText("<IdAplication>", volc.IdApplication);
                        if (volc.IdInvestigation != null)
                            doc.ReplaceText("<IdInvestigation>", volc.IdInvestigation);

                        string aaa = textBox1.Text;
                        string value = "test";
                        if (radioButton1.Checked)
                        {
                            value = radioButton1.Text.ToString();
                        }
                        else if (radioButton2.Checked)
                        { value = radioButton2.Text.ToString(); }
                        else if (radioButton3.Checked)
                        { value = radioButton3.Text.ToString(); }
                        else if (radioButton4.Checked)
                        { value = radioButton4.Text.ToString(); }
                        else
                            if (aaa != null)
                        { value = aaa; }
                        else { value = ""; }
                        volc.myOption = value;

                        doc.ReplaceText("<option>", volc.myOption);
                        doc.ReplaceText("<NumberOfPortions>", volc.NumberOfPortion);
                        doc.ReplaceText("<RegistrationDate> ", volc.RegistrationDate.ToShortDateString());
                        doc.ReplaceText("<ExpirationDate>", volc.ExpirationDate.ToShortDateString());
                        if (saveFileDialog1.FileName.Contains(".docx") == true)
                        {
                            doc.SaveAs(saveFileDialog1.FileName);
                        }
                        else
                        {
                            doc.SaveAs(saveFileDialog1.FileName + "." + "docx");
                        }

                        richTextBox2.Text = saveFileDialog1.FileName;
                        richTextBox3.Text = "File Saved succesfully";
                    }
                    else
                    {
                        string[] args = Environment.GetCommandLineArgs();
                        var doc = DocX.Load(richTextBox1.Text);
                        HttpClient httpClient = new HttpClient();
                        args[1] = args[1].Remove(0, 16);
                        //probabil trebuie modificat
                        string url = "http://localhost:5000/api/Values/" + args[1];
                        var result = httpClient.GetStringAsync(url).Result.Normalize();
                        result = result.Replace("[", "");
                        result = result.Replace("]", "");
                        volcontract volc = new volcontract();
                        volc = JsonConvert.DeserializeObject<volcontract>(result);
                        string phrase = volc.Address;
                        string[] words = phrase.Split(',');
                        doc.ReplaceText("<nrreg>", volc.NumberOfRegistration);
                        doc.ReplaceText("<todaydate>", volc.RegistrationDate.ToShortDateString());
                        doc.ReplaceText("<Fullname>", volc.Firstname + " " + volc.Lastname);
                        if (volc.CNP != null)
                            doc.ReplaceText("<CNP>", volc.CNP);
                        if (volc.CIseria != null)
                            doc.ReplaceText("<Seria>", volc.CIseria);
                        if (volc.CINr != null)
                            doc.ReplaceText("<Nr>", volc.CINr);
                        if (volc.CIEliberat != null)
                            doc.ReplaceText("<eliberat>", volc.CIEliberat.ToShortDateString());
                        if (volc.CIeliberator != null)
                            doc.ReplaceText("<eliberator>", volc.CIeliberator);
                        if (words[1] != null)
                            doc.ReplaceText("<oras>", words[1]);
                        if (words[2] != null)
                            doc.ReplaceText("<str>", words[2]);
                        if (words[3] != null)
                            doc.ReplaceText("<nr>", words[3]);
                        if (words[0] != null)
                            doc.ReplaceText("<jud>", words[0]);
                        if (volc.Nrtel != null)
                            doc.ReplaceText("<tel>", volc.Nrtel);
                        doc.ReplaceText("<startdate>", volc.RegistrationDate.ToShortDateString());
                        doc.ReplaceText("<finishdate>", volc.ExpirationDate.ToShortDateString());
                        doc.ReplaceText("<hourcount>", volc.HourCount.ToString());
                        if (saveFileDialog1.FileName.Contains(".docx") == true)
                        {
                            doc.SaveAs(saveFileDialog1.FileName);
                        }
                        else
                        {
                            doc.SaveAs(saveFileDialog1.FileName + "." + "docx");
                            saveFileDialog1.FileName = saveFileDialog1.FileName + ".docx";
                        }

                        richTextBox2.Text = saveFileDialog1.FileName;
                        richTextBox3.Text = "File Saved succesfully";
                    }
                }
            }
            catch
            {
                richTextBox2.Text = saveFileDialog1.FileName;
                richTextBox3.Text = "an error has occured";
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private static void RegisterMyProtocol(string ContractPrinterPath)  //myAppPath = full path to your application
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey("ContractPrinter");  //open myApp protocol's subkey

            if (key == null)  //if the protocol is not registered yet...we register it
            {
                key = Registry.ClassesRoot.CreateSubKey("ContractPrinter");
                key.SetValue(string.Empty, "URL:ContractPrinter Protocol");
                key.SetValue("URL Protocol", string.Empty);
                key = key.CreateSubKey(@"shell\open\command");
                key.SetValue(string.Empty, ContractPrinterPath + " " + "%1");
                //%1 represents the argument - this tells windows to open this program with an argument / parameter
            }

            key.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {
        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
        }
    }
}