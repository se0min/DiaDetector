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
    public partial class frmFuncAxisMove : Form
    {
        public WorkFuncInfo _WorkFuncInfo;
        

        // 속도 ...
        // ----------
        public RadioButton SelectedRB;
        public int AxisSpeed = 1;
        // ----------


        public frmFuncAxisMove()
        {
            InitializeComponent();
        }

        private void Initialize()
        {
            // ...
            // ----------
            foreach (var item in DataManager.AxisWorkList)
            {
                cboSelectAxis.Items.Add(item);
            }

            cboSelectAxis.SelectedIndex = _WorkFuncInfo.SelectedAxis;


            // 원점 복귀 ...
            // ----------
            cboSelectHome.SelectedIndex = _WorkFuncInfo.ReturnHome;
            cboEndWait.SelectedIndex = _WorkFuncInfo.AxisEndWait;


            txtAxisValue.Text = _WorkFuncInfo.WFMoveX.ToString();


            // 속도 설정 ...
            // ----------
            AxisSpeed = _WorkFuncInfo.AxisSpeed;

            SelectSpeed(AxisSpeed);            
        }

        public void SelectSpeed(int speed)
        {
            radioBtnSlow.Checked = false;
            radioBtnMidium.Checked = false;
            radioBtnFast.Checked = false;


            // ----------
            switch (speed)
            {
            case MultiMotion.KSM_SPEED_SLOW:
                SelectedRB = radioBtnSlow;
                break;
            case MultiMotion.KSM_SPEED_MIDIUM:
                SelectedRB = radioBtnMidium;
                break;
            case MultiMotion.KSM_SPEED_FAST:
                SelectedRB = radioBtnFast;
                break;
            }
            // ----------


            SelectedRB.Checked = true;
        }

        private void frmFuncAxisMove_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private void frmFuncAxisMove_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            double dTempValue = 0.0;
            
            if (double.TryParse(this.txtAxisValue.Text, out dTempValue))
            {
                _WorkFuncInfo.WFMoveX = dTempValue;
            }                
            else
            {
                return;
            }

            _WorkFuncInfo.SelectedAxis = cboSelectAxis.SelectedIndex;
            _WorkFuncInfo.ReturnHome = cboSelectHome.SelectedIndex;
            _WorkFuncInfo.AxisEndWait = cboEndWait.SelectedIndex;

            switch (_WorkFuncInfo.SelectedAxis)
            {
                case 0: // INDEX(고정축) 회전(R) 기능                    
                    break;
                case 1: // INDEX(이동축) 회전(R) 기능
                    break;
                case 2: // INDEX(고정축) Rolling(1) 기능
                    break;
                case 3: // INDEX(고정축) Rolling(2) 기능
                    break;
                case 4: // INDEX(이동축) Rolling(1) 기능
                    break;

                case 5: // INDEX(이동축) Rolling(2) 기능
                    break;
                case 6: // INDEX(이동축) X축 갠트리(M) 제어
                    _WorkFuncInfo.TestProcName = "INDEX(이동축) X축 이동";
                    break;
                case 7: // INDEX(이동축) X축 갠트리(S) 제어
                    break;
                case 8: // 카메라 유닛 X축 이동 기능
                    _WorkFuncInfo.TestProcName = "카메라 X축 이동";
                    break;
                case 9: // 카메라 유닛 Y축 이동 기능
                    _WorkFuncInfo.TestProcName = "카메라 Y축 이동";
                    break;

                case 10: // 카메라 유닛 Z축 이동 기능
                    _WorkFuncInfo.TestProcName = "카메라 Z축 이동";
                    break;
                case 11: // 후면 카메라 Z축 이동 기능
                    _WorkFuncInfo.TestProcName = "후방 카메라 Z축 이동";
                    break;
                case 12: // V블럭 위치 이동(Z) 기능
                    _WorkFuncInfo.TestProcName = "V블럭 Z축 이동";
                    break;
                case 13: // 용접 로봇 Tilting(R) 기능
                    break;
                case 14: // 
                    break;

            }


            if (SelectedRB != null)
            {
                _WorkFuncInfo.AxisSpeed = Convert.ToInt32(SelectedRB.Tag);
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void txtAxisValue_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtAxisValue_KeyDown(object sender, KeyEventArgs e)
        {
            double dTempValue = 0.0;

            if (double.TryParse(this.txtAxisValue.Text, out dTempValue))
            {

            }
            else
            {
                this.txtAxisValue.Text = "0.0";
            }
        }

        private void radioBtn_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb == null)
            {
                return;
            }

            if (rb.Checked)
            {
                SelectedRB = rb;                
            }
        }

    }
}
