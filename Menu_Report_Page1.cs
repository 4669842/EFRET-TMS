using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace efretTMS
{
    public partial class Menu_Report_Page1 : Telerik.WinControls.UI.RadForm
    {
        public Menu_Report_Page1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            RadForm1 radForm1 = new RadForm1();
            radForm1.Show();
        }
    }
}
