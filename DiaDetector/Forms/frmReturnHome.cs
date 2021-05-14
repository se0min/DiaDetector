using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// ----------

using DiaDetector.Data;
using DiaDetector.Drivers;
using System.Threading;


namespace DiaDetector
{
    public partial class frmReturnHome : Form
    {
        public frmReturnHome()
        {
            InitializeComponent();
       
        }

        private void frmReturnHome_Load(object sender, EventArgs e)
        {
            UpdateHomeStatus();
           // Thred1();
        }
        public void Thred1()
        {
            ThreadStart start = this.UpdateHomeStatus;
            Thread thread = new Thread(start);
            thread.Start();
        }
        private void timerHomeReturn_Tick(object sender, EventArgs e)
        {
            UpdateHomeStatus();
         //   Thred1();
        }
        
        private void UpdateHomeStatus()
        {

            try {
            if (MultiMotion.ReadyHome[MultiMotion.Camera1Adjust] == true )
            {
                btnCamA.Text = "OK";
                btnCamA.BackColor = Color.Green;
            }
            else
            {
                btnCamA.Text = "Ready";
                btnCamA.BackColor = Color.Gray;

            }

            if (MultiMotion.ReadyHome[MultiMotion.Shuttle1Motor] == true )
            {
                btnCamB.Text = "OK";
                btnCamB.BackColor = Color.Green;
            }
            else
            {
                btnCamB.Text = "Ready";
                btnCamB.BackColor = Color.Gray;
            }


            if (MultiMotion.ReadyHome[MultiMotion.Shuttle2Motor] == true)
            {
                btnCamC.Text = "OK";
                btnCamC.BackColor = Color.Green;
            }
            else
            {
                btnCamC.Text = "Ready";
                btnCamC.BackColor = Color.Gray;
            }


            if (MultiMotion.ReadyHome[MultiMotion.RotationMotor] == true )
            {
                btnCamZ.Text = "OK";
                btnCamZ.BackColor = Color.Green;
            }
            else
            {
                btnCamZ.Text = "Ready";
                btnCamZ.BackColor = Color.Gray;
            }


            if (MultiMotion.ReadyHome[MultiMotion.Camera2Adjust] == true)
            {
                this.btnCamRotate.Text = "OK";
                btnCamRotate.BackColor = Color.Green;
            }
            else
            {
                btnCamRotate.Text = "Ready";
                btnCamRotate.BackColor = Color.Gray;
            }


            if (MultiMotion.ReadyHome[MultiMotion.Lift2Motor] == true)
            {
                this.GuideAdjustMotorR.Text = "OK";
                GuideAdjustMotorR.BackColor = Color.Green;
            }
            else
            {
                GuideAdjustMotorR.Text = "Ready";
                GuideAdjustMotorR.BackColor = Color.Gray;
            }


            if (MultiMotion.ReadyHome[MultiMotion.Lift1Motor] == true )
            {
                this.GuideAdjustMotorL.Text = "OK";
                GuideAdjustMotorL.BackColor = Color.Green;
            }
            else
            {
                GuideAdjustMotorL.Text = "Ready";
                GuideAdjustMotorL.BackColor = Color.Gray;
            }
                 }
            catch(Exception ex)
            {

            }
        }

    }
}
