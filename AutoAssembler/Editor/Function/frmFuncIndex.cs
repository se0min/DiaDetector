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
    public partial class frmFuncIndex : Form
    {
        public WorkFuncInfo _WorkFuncInfo;

        public frmFuncIndex()
        {
            InitializeComponent();
        }

        private void frmFuncIndex_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private void frmFuncIndex_FormClosing(object sender, FormClosingEventArgs e)
        {
            MultiMotion.StopAll();

            timerAxis.Enabled = false;
        }

        public void Initialize()
        {
            txtAxisValue.Text = _WorkFuncInfo.WFRotationAngle.ToString();

            timerAxis.Enabled = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _WorkFuncInfo.WFRotationAngle = double.Parse(txtAxisValue.Text);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }


#region JOG ...

        private void btnJogMinus_MouseDown(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, true) == MultiMotion.KSM_OK)
            {
                MultiMotion.StepMove(MultiMotion.INDEX_FIX_R, 0, false);
            }
        }

        private void btnJogMinus_MouseUp(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, true) == MultiMotion.KSM_OK)
            {
                //MultiMotion.JogStop(MultiMotion.INDEX_FIX_R);
            }
        }

        private void btnJogPlus_MouseDown(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, true) == MultiMotion.KSM_OK)
            {
                MultiMotion.StepMove(MultiMotion.INDEX_FIX_R, 1, false);
            }
        }

        private void btnJogPlus_MouseUp(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, true) == MultiMotion.KSM_OK)
            {
                //MultiMotion.JogStop(MultiMotion.INDEX_FIX_R);
            }
        }

        private void btnJogHome_Click(object sender, EventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, true) == MultiMotion.KSM_OK)
            {
                MultiMotion.HomeMove(MultiMotion.INDEX_FIX_R, true);

                MessageBox.Show("원점 이동이 완료되었습니다.");
            }
        }

#endregion JOG ...


        private void timerAxis_Tick(object sender, EventArgs e)
        {
            UpdatePos();

            /*if (MultiMotion.CheckDefense() == false) {
            }*/
        }

        private void UpdatePos()
        {
            MultiMotion.GetCurrentPos();

            //txtAxisValue.Text = MultiMotion.AxisValue[MultiMotion.INDEX_MOVE_M].ToString();
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
                            /*if (MultiMotion.GantryAxisEnable(0, true) == true)
                            {
                                MultiMotion.GantryAxis(MultiMotion.INDEX_MOVE_M, MultiMotion.INDEX_MOVE_S, dTempValue, true);
                            }*/
                        }

                        timerAxis.Enabled = true;
                    }
                    break;
                default:
                    timerAxis.Enabled = false;
                    break;
            }
        }
    }
}
