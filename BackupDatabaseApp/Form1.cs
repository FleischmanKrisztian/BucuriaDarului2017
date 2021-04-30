using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackupDatabaseApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //string[] args = Environment.GetCommandLineArgs();
            ////args[0] is always the path to the application
            //RegisterMyProtocol(args[0]);
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            //^the method posted before, that edits registry
        }

        //private static void RegisterMyProtocol(string DatabaseManagement)  //myAppPath = full path to your application
        //{
        //    RegistryKey key = Registry.ClassesRoot.OpenSubKey("DatabaseManagement");  //open myApp protocol's subkey

        //    if (key == null)  //if the protocol is not registered yet...we register it
        //    {
        //        key = Registry.ClassesRoot.CreateSubKey("DatabaseManagement");
        //        key.SetValue(string.Empty, "URL:DatabaseManagement Protocol");
        //        key.SetValue("URL Protocol", string.Empty);
        //        key = key.CreateSubKey(@"shell\open\command");
        //        key.SetValue(string.Empty, DatabaseManagement + " " + "%1");
        //        //%1 represents the argument - this tells windows to open this program with an argument / parameter
        //    }

        //    key.Close();
        //}

        private void Form1_Load(object sender, EventArgs e)
        {
            userControl11.Show();
            panel1.Hide();
            userControl21.Hide();
            userControl31.Hide();
            userControl41.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            userControl11.Hide();
            userControl21.Hide();
            userControl31.Hide();
            userControl41.Show();
            panel1.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            userControl11.Hide();
            userControl21.Show();
            userControl31.Hide();
            userControl41.Hide();
            panel1.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            userControl11.Hide();
            userControl21.Hide();
            userControl31.Show();
            userControl41.Hide();
            panel1.Show();
        }

        private void userControl41_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

            Form tmp = this.FindForm();
            tmp.Close();
            tmp.Dispose();
        }

        private void userControl21_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            if (userControl21.Visible)
            {
                userControl21.Hide();
                userControl11.Show();
                panel1.Hide();
            }
            if (userControl31.Visible)
            {
                userControl31.Hide();
                userControl11.Show();
                panel1.Hide();
            }
            if (userControl41.Visible)
            {
                userControl41.Hide();
                userControl11.Show();
                panel1.Hide();
            }
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
