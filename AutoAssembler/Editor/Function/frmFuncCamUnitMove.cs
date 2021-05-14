using System;
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
    public partial class frmFuncCamUnitMove : Form
    {
        public WorkFuncInfo _WorkFuncInfo;

        public frmFuncCamUnitMove()
        {
            InitializeComponent();
        }

        private void frmFuncMove_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private void frmFuncMove_FormClosing(object sender, FormClosingEventArgs e)
        {            
            MultiMotion.StopAll();

            timerAxis.Enabled = false;
        }

        public void Initialize()
        {
            txtXAxisValue.Text = _WorkFuncInfo.WFMoveX.ToString();
            txtYAxisValue.Text = _WorkFuncInfo.WFMoveY.ToString();
            txtZAxisValue.Text = _WorkFuncInfo.WFMoveZ.ToString();

            timerAxis.Enabled = true;
        }


#region Button ...

        private void btnOk_Click(object sender, EventArgs e)
        {
            _WorkFuncInfo.WFMoveX = double.Parse(txtXAxisValue.Text);
            _WorkFuncInfo.WFMoveY = double.Parse(txtYAxisValue.Text);
            _WorkFuncInfo.WFMoveZ = double.Parse(txtZAxisValue.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

#endregion Button ...


#region JOG ...

        // X축 ...
        // --------------------------------------------------

        private void btnJogXPlus_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.StepMove(MultiMotion.CAM_UNIT_X, 0, false);
        }

        private void btnJogXPlus_MouseUp(object sender, MouseEventArgs e)
        {
            //MultiMotion.JogStop(MultiMotion.CAM_UNIT_X);
        }

        private void btnJogXMinus_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.StepMove(MultiMotion.CAM_UNIT_X, 1, false);

            timerAxis.Enabled = true;
        }

        private void btnJogXMinus_MouseUp(object sender, MouseEventArgs e)
        {
            //MultiMotion.JogStop(MultiMotion.CAM_UNIT_X);
        }

        private void btnJogXHome_Click(object sender, EventArgs e)
        {
            MultiMotion.HomeMove(MultiMotion.CAM_UNIT_X, true);

            MessageBox.Show("원점 이동이 완료되었습니다.");
        }

        // Y축 ...
        // --------------------------------------------------

        private void btnJogYPlus_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.StepMove(MultiMotion.CAM_UNIT_Y, 0, false);
        }

        private void btnJogYPlus_MouseUp(object sender, MouseEventArgs e)
        {
            //MultiMotion.JogStop(MultiMotion.CAM_UNIT_Y);
        }

        private void btnJogYMinus_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.StepMove(MultiMotion.CAM_UNIT_Y, 1, false);
        }

        private void btnJogYMinus_MouseUp(object sender, MouseEventArgs e)
        {
            //MultiMotion.JogStop(MultiMotion.CAM_UNIT_Y);
        }

        private void btnJogYHome_Click(object sender, EventArgs e)
        {
            MultiMotion.HomeMove(MultiMotion.CAM_UNIT_Y, true);

            MessageBox.Show("원점 이동이 완료되었습니다.");
        }

        // Z축 ...
        // --------------------------------------------------

        private void btnJogZPlus_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.StepMove(MultiMotion.CAM_UNIT_Z, 0, false);
        }

        private void btnJogZPlus_MouseUp(object sender, MouseEventArgs e)
        {
            //MultiMotion.JogStop(MultiMotion.CAM_UNIT_Z);
        }

        private void btnJogZMinus_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.StepMove(MultiMotion.CAM_UNIT_Z, 1, false);
        }

        private void btnJogZMinus_MouseUp(object sender, MouseEventArgs e)
        {
            //MultiMotion.JogStop(MultiMotion.CAM_UNIT_Z);
        }

        private void btnJogZHome_Click(object sender, EventArgs e)
        {
            MultiMotion.HomeMove(MultiMotion.CAM_UNIT_Z, true);

            MessageBox.Show("원점 이동이 완료되었습니다.");
        }

#endregion JOG ...


#region JOG + ...

        private void txtXAxisValue_KeyDown(object sender, KeyEventArgs e)
        {
            double dTempValue = 0.0;

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    {
                        if (double.TryParse(this.txtXAxisValue.Text, out dTempValue))
                        {
                            MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_X, dTempValue, false);
                        }

                        timerAxis.Enabled = true;
                    }
                    break;
                default:
                    timerAxis.Enabled = false;
                    break;
            }

        }

        private void txtYAxisValue_KeyDown(object sender, KeyEventArgs e)
        {
            double dTempValue = 0.0;

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    {
                        if (double.TryParse(this.txtYAxisValue.Text, out dTempValue))
                        {
                            MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_Y, dTempValue, false);
                        }

                        timerAxis.Enabled = true;
                    }
                    break;
                default:
                    timerAxis.Enabled = false;
                    break;
            }

        }

        private void txtZAxisValue_KeyDown(object sender, KeyEventArgs e)
        {
            double dTempValue = 0.0;

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    {
                        if (double.TryParse(this.txtZAxisValue.Text, out dTempValue))
                        {
                            MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_Z, dTempValue, false);
                        }

                        timerAxis.Enabled = true;
                    }
                    break;
                default:
                    timerAxis.Enabled = false;
                    break;
            }

        }

        private void timerAxis_Tick(object sender, EventArgs e)
        {
            UpdatePos();

            if (MultiMotion.CheckDefense() != MultiMotion.KSM_OK)
            {

            }
        }

        private void UpdatePos()
        {
            MultiMotion.GetCurrentPos();

            txtXAxisValue.Text = MultiMotion.AxisValue[MultiMotion.CAM_UNIT_X].ToString();
            txtYAxisValue.Text = MultiMotion.AxisValue[MultiMotion.CAM_UNIT_Y].ToString();
            txtZAxisValue.Text = MultiMotion.AxisValue[MultiMotion.CAM_UNIT_Z].ToString();            
        }

#endregion JOG + ...


    }
}
