using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

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
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog  = new System.Windows.Forms.FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string strfilename = folderBrowserDialog.SelectedPath;
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
            DatabaseMethods.BackupDatabase(filename);
            richTextBox2.Text = "Backup was successful";
            panel1.Show();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }
    }
}