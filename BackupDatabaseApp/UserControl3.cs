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

namespace BackupDatabaseApp
{
    public partial class UserControl3 : UserControl
    {
        public UserControl3()
        {
            InitializeComponent();
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            
        }

        private void UserControl3_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stream myStream;
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if ((myStream = openFileDialog1.OpenFile()) != null)
                {
                    string strfilename = openFileDialog1.FileName;
                    richTextBox1.Text = strfilename;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string path = richTextBox1.Text;
            DatabaseMethods databaseMethods = new DatabaseMethods();
            string message =databaseMethods.RestoreDatabase(path);
            richTextBox2.Text = message;
           //System.Windows.Forms.MessageBox.Show(message);
        }
    }
}
