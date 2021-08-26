using System;
using System.Windows.Forms;

namespace BackupDatabaseApp
{
    public partial class UserControl3 : UserControl
    {
        public UserControl3()
        {
            InitializeComponent();
        }

        private void UserControl3_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string strfilename = folderBrowserDialog.SelectedPath;
                richTextBox1.Text = strfilename;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string path = richTextBox1.Text;
            DatabaseMethods.RestoreDatabase(path);
            richTextBox2.Text = "Restore was successful";
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }
    }
}