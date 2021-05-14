using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// ----------

using AutoAssembler.Data;
using AutoAssembler.Drivers;


namespace AutoAssembler
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
        }

        private void timerHomeReturn_Tick(object sender, EventArgs e)
        {
            UpdateHomeStatus();
        }

        private void UpdateHomeStatus()
        {
            // 카메라 유닛 원점 복귀 ...
            // --------------------------------------------------
            if (MultiMotion.ReadyHome[MultiMotion.CAM_UNIT_Z] == true)
            {
                btnCamZ.Text = "OK";
                btnCamZ.BackColor = Color.Green;
            }
            else
            {
                btnCamZ.Text = "Ready";
                btnCamZ.BackColor = Color.Gray;
            }


            if (MultiMotion.ReadyHome[MultiMotion.CAM_UNIT_Y] == true)
            {
                btnCamY.Text = "OK";
                btnCamY.BackColor = Color.Green;
            }                
            else
            {
                btnCamY.Text = "Ready";
                btnCamY.BackColor = Color.Gray;
            }
                

            if (MultiMotion.ReadyHome[MultiMotion.CAM_UNIT_X] == true)
            {
                btnCamX.Text = "OK";
                btnCamX.BackColor = Color.Green;
            }                
            else
            {
                btnCamX.Text = "Ready";
                btnCamX.BackColor = Color.Gray;
            }                


            // 후방 카메라 원점 복귀 ...
            // --------------------------------------------------
            if (MultiMotion.ReadyHome[MultiMotion.BACK_CAM_Z] == true)
            {
                btnBackCamZ.Text = "OK";
                btnBackCamZ.BackColor = Color.Green;
            }
            else
            {
                btnBackCamZ.Text = "Ready";
                btnBackCamZ.BackColor = Color.Gray;
            }

            if (MultiMotion.ReadyHome[MultiMotion.INDEX_MOVE_M] == true)
            {
                btnIndexX.Text = "OK";
                btnIndexX.BackColor = Color.Green;
            }
            else
            {
                btnIndexX.Text = "Ready";
                btnIndexX.BackColor = Color.Gray;
            }


            // V블럭 원점 복귀 ...
            // --------------------------------------------------
            if (MultiMotion.ReadyHome[MultiMotion.VBLOCK_Z] == true)
            {
                btnVBlockZ.Text = "OK";
                btnVBlockZ.BackColor = Color.Green;
            }
            else
            {
                btnVBlockZ.Text = "Ready";
                btnVBlockZ.BackColor = Color.Gray;
            }


            // 롤링 유닛 원점 복귀 ...
            // --------------------------------------------------
            if (MultiMotion.ReadyHome[MultiMotion.ROLLING_FIX_1] == true)
            {
                btnRollingFixA.Text = "OK";
                btnRollingFixA.BackColor = Color.Green;
            }
            else
            {
                btnRollingFixA.Text = "Ready";
                btnRollingFixA.BackColor = Color.Gray;
            }

            if (MultiMotion.ReadyHome[MultiMotion.ROLLING_FIX_2] == true)
            {
                btnRollingFixB.Text = "OK";
                btnRollingFixB.BackColor = Color.Green;
            }
            else
            {
                btnRollingFixB.Text = "Ready";
                btnRollingFixB.BackColor = Color.Gray;
            }

            if (MultiMotion.ReadyHome[MultiMotion.ROLLING_MOVE_1] == true)
            {
                btnRollingMoveA.Text = "OK";
                btnRollingMoveA.BackColor = Color.Green;
            }
            else
            {
                btnRollingMoveA.Text = "Ready";
                btnRollingMoveA.BackColor = Color.Gray;
            }

            if (MultiMotion.ReadyHome[MultiMotion.ROLLING_MOVE_2] == true)
            {
                btnRollingMoveB.Text = "OK";
                btnRollingMoveB.BackColor = Color.Green;
            }
            else
            {
                btnRollingMoveB.Text = "Ready";
                btnRollingMoveB.BackColor = Color.Gray;
            }
        }

    }
}
