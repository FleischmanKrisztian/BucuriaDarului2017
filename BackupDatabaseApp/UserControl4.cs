using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackupDatabaseApp
{
    public partial class UserControl4 : UserControl
    {
        public UserControl4()
        {
            InitializeComponent();
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            
        }

        private void UserControl4_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            DatabaseMethods databaseMethods = new DatabaseMethods();
            if (textBox1.Text.Contains(Common.MongoConstants.DATABASE_NAME_COMMON))
            {
                databaseMethods.DeleteDatabase();

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("This action  failed!Please inseret a valid database name.");
            }
        }
    }
}
