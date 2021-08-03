using System;
using System.Windows.Forms;

namespace BackupDatabaseApp
{
    public partial class UserControl4 : UserControl
    {
        public UserControl4()
        {
            InitializeComponent();
        }

        private void UserControl4_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
               DatabaseMethods.DeleteDatabase(textBox1.Text);
               MessageBox.Show("The Database has been successfully deleted");
            }
            catch
            {
                MessageBox.Show("This action  failed! Please insert a valid database name.");
            }
        }
    }
}