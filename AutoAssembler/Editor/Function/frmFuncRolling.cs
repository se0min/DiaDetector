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

namespace AutoAssembler
{
    public partial class frmFuncRolling : Form
    {
        public WorkFuncInfo _WorkFuncInfo;

        public bool _bMinusJog = false;

        public frmFuncRolling()
        {
            InitializeComponent();
        }

        private void frmFuncRolling_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        public void Initialize()
        {
            txtAxisValue.Text = _WorkFuncInfo.dWFRollingValue.ToString();
        }


#region Button ...

        private void btnOk_Click(object sender, EventArgs e)
        {
            _WorkFuncInfo.dWFRollingValue = double.Parse(txtAxisValue.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

#endregion Button ...


#region JOG ...

        private void timerAxis_Tick(object sender, EventArgs e)
        {
            double dRollingValue = double.Parse(txtAxisValue.Text);

            if (_bMinusJog == true) // 마이너스
            {
                dRollingValue -= 0.1;
                if (dRollingValue < 0)
                {
                    dRollingValue = 0;
                }
            }
            else
            {
                dRollingValue += 0.1;
                if (dRollingValue > 7.5)
                {
                    dRollingValue = 7.5;
                }                
            }

            txtAxisValue.Text = dRollingValue.ToString();
        }

        private void btnJogMinus_MouseDown(object sender, MouseEventArgs e)
        {
            _bMinusJog = true;

            timerAxis.Enabled = true;        
        }

        private void btnJogMinus_MouseUp(object sender, MouseEventArgs e)
        {
            _bMinusJog = true;

            timerAxis.Enabled = false;        
        }

        private void btnJogPlus_MouseDown(object sender, MouseEventArgs e)
        {
            _bMinusJog = false;

            timerAxis.Enabled = true;        
        
        }

        private void btnJogPlus_MouseUp(object sender, MouseEventArgs e)
        {
            _bMinusJog = false;

            timerAxis.Enabled = false;        
        
        }

        private void btnJogHome_Click(object sender, EventArgs e)
        {
            txtAxisValue.Text = "0.0";
        }

#endregion JOG ...

        private void frmFuncRolling_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MultiMotion.StopAll();
        }



    }
}
