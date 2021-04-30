using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace BackupDatabaseApp
{
    public partial class UserControl2 : UserControl
    {
        private string filename;
        public UserControl2()
        {
            InitializeComponent();
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            panel1.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void UserControl2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string strfilename;
                strfilename = saveFileDialog1.FileName;
                richTextBox1.Text = strfilename;
                filename = strfilename;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string my_directory = Path.GetDirectoryName(filename);
            Process.Start(my_directory);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DatabaseMethods databaseMethods = new DatabaseMethods();
            string message=databaseMethods.BackupDatabase(filename);
            richTextBox2.Text = message;
            if (message.Contains("succesfuly"))
            {
                panel1.Show();
            }
        }
    }
}
