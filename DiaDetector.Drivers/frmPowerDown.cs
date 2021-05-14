using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiaDetector.Drivers
{
    public partial class frmPowerDown : Form
    {
        public frmPowerDown()
        {
            InitializeComponent();
        }

       private void btnStopAll_Click(object sender, EventArgs e)
       {
          MultiMotion.StopAll();
       }

        private void btnSystemDown_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmPowerDown_Load(object sender, EventArgs e)
        {
            MultiMotion.StopAll();
        }
    }
}
