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
            //^the method posted before, that edits registry
            panel1.Hide();
        }

        private void UserControl4_Load(object sender, EventArgs e)
        {

        }
    }
}
