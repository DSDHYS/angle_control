using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace angle_control
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            chart1.MouseClick += new System.Windows.Forms.MouseEventHandler(Chart_MouseClick);
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        static void Chart_MouseClick(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.DataVisualization.Charting.Chart chart1 = sender as System.Windows.Forms.DataVisualization.Charting.Chart;
            if (e.Button == MouseButtons.Right)
            {
                chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);
            }
        }
    }
}
