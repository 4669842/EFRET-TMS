using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace efretTMS
{
    public partial class RadForm1 : Telerik.WinControls.UI.RadForm
    {
        public RadForm1()
        {
            InitializeComponent();
            this.Text = "Welcome to EFRET "+Environment.UserName;
        }


        private void RadForm1_Load(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        /*
         * This button is for the Dashboard. We show the charging orders assigned / created by the logged in user.
         */
        private void button1_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.AllowAero = true;
            dashboard.AllowTheming = true;
            dashboard.AutoScaleMode = AutoScaleMode.Inherit;
            dashboard.Show();
        }
        /*
         * This button shows first menu of report options menu.
         */
        private void button2_Click(object sender, EventArgs e)
        {
            Menu_Report_Page1 report_Page1 = new Menu_Report_Page1();
            report_Page1.Show();

        }
private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var UpdateSrv = new Updater();
            UpdateSrv.Visible = true;

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }
        //Price List Menu Button.
        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
