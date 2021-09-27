using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace efretTMS
{
    public partial class P44COHist : Telerik.WinControls.UI.ShapedForm
    {
        public P44COHist(string record)
        {
            var chargingOrderID = record;
            InitializeComponent();
            groupBox1.Text = "ShipmentID: " + chargingOrderID;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
