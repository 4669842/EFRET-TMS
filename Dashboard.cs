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
    public partial class Dashboard : Telerik.WinControls.UI.RadTabbedForm
    {
        public Dashboard()
        {
            InitializeComponent();

            this.AllowAero = true;
            this.ShowInTaskbar = true;
            this.ShowIcon = true;

        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'axsNewCODataSet.NewCO' table. You can move, or remove it, as needed.
            this.newCOTableAdapter.Fill(this.axsNewCODataSet.NewCO);


        }

        private void fillToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.newCOTableAdapter.Fill(this.axsNewCODataSet.NewCO);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            //We do some workflow logic depending on the column of the record clicked.

            var record = dataGridView1.CurrentCell.Value.ToString();
            int clickedColumnIndex = e.ColumnIndex;
            MessageBox.Show(record);
            // if Column selected we show charging order
            if(clickedColumnIndex == 0||clickedColumnIndex == 1)
            {

            }
            // If column selected we show Project44 Info window
            if(clickedColumnIndex == 2)
            {
               P44COHist p44COHist = new P44COHist(record);
                p44COHist.Show();
            }
        }
    }
}
