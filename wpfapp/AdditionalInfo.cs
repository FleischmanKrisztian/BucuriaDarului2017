using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

namespace wpfapp
{
    public partial class AdditionalInfo : UserControl
    {
        private string myValue;

        public string MyVal
            
        {
            get { return myValue; }
            set { myValue = value; }
        }

        public AdditionalInfo()
        {
            InitializeComponent();

        }

        private void AdditionalInfo_Load(object sender, EventArgs e)
        {
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
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

            MyVal = value;


        }
    }
}
