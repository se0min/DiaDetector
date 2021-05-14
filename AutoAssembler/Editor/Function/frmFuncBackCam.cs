﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//////////

using AutoAssembler.Data;
using AutoAssembler.Drivers;

namespace AutoAssembler
{
    public partial class frmFuncBackCam : Form
    {
        public WorkFuncInfo _WorkFuncInfo;

        public frmFuncBackCam()
        {
            InitializeComponent();
        }

        private void frmFuncBackCam_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private void frmFuncBackCam_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MultiMotion.StopAll();

            timerAxis.Enabled = false;
        }

        public void Initialize()
        {
            txtAxisValue.Text = _WorkFuncInfo.WFMoveZ.ToString();

            timerAxis.Enabled = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _WorkFuncInfo.WFMoveZ = double.Parse(txtAxisValue.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

#region JOG ...

        private void UpdatePos()
        {
            MultiMotion.GetCurrentPos();

            txtAxisValue.Text = MultiMotion.AxisValue[MultiMotion.BACK_CAM_Z].ToString();
        }

        private void timerAxis_Tick(object sender, EventArgs e)
        {
            UpdatePos();

            /*
            if (MultiMotion.CheckDefense() == false)
            {

            }
            */
        }

        private void btnJogMinus_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.StepMove(MultiMotion.BACK_CAM_Z, 0, false);
        }

        private void btnJogMinus_MouseUp(object sender, MouseEventArgs e)
        {
            //MultiMotion.JogStop(MultiMotion.BACK_CAM_Z);
        }

        private void btnJogPlus_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.StepMove(MultiMotion.BACK_CAM_Z, 1, false);
        }

        private void btnJogPlus_MouseUp(object sender, MouseEventArgs e)
        {
            //MultiMotion.JogStop(MultiMotion.BACK_CAM_Z);
        }

        private void btnJogHome_Click(object sender, EventArgs e)
        {
            MultiMotion.HomeMove(MultiMotion.BACK_CAM_Z, true);

            MessageBox.Show("원점 이동이 완료되었습니다.");
        }

        private void txtAxisValue_KeyDown(object sender, KeyEventArgs e)
        {
            double dTempValue = 0.0;

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    {
                        if (double.TryParse(this.txtAxisValue.Text, out dTempValue))
                        {
                            MultiMotion.MoveAxis(MultiMotion.BACK_CAM_Z, dTempValue, false);
                        }

                        timerAxis.Enabled = true;
                    }
                    break;
                default:
                    timerAxis.Enabled = false;
                    break;
            }
        }
        
#endregion JOG ...


    }
}
