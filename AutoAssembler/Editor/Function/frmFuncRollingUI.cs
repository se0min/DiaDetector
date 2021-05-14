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
    public partial class frmFuncRollingUI : Form
    {
        public WorkFuncInfo _WorkFuncInfo;


        public frmFuncRollingUI()
        {
            InitializeComponent();
        }

        public void Initialize()
        {

            txtMetalThick1.Text = DataManager.SelectedModel.dMetalThick1.ToString();
            txtMetalThick2.Text = DataManager.SelectedModel.dMetalThick2.ToString();

            txtFLValue.Text = DataManager.SelectedModel.dFLValue.ToString();
            txtSLValue.Text = DataManager.SelectedModel.dSLValue.ToString();
            txtCapsule.Text = DataManager.SelectedModel.dCapsulePie.ToString();
            txtWRValue.Text = DataManager.SelectedModel.dWRValue.ToString();

            txtRolling70Rate.Text = DataManager.SelectedModel.dRolling70Rate.ToString();
            txtRolling80Rate.Text = DataManager.SelectedModel.dRolling80Rate.ToString();

            txtRollingOffset.Text = DataManager.SelectedModel.dRollingOffset.ToString();
            txtRotateCount.Text = DataManager.SelectedModel.dRotateCount.ToString();

            txtVBlockFL_Limit_Value.Text = DataManager.SelectedModel.dVBlockFL_Limit_Value.ToString();
            txtVBlockFL_Offset_Value.Text = DataManager.SelectedModel.dVBlockFL_Offset_Value.ToString();




            if (UpdateFormula() == false)
            {
                MessageBox.Show("계산식 업데이트에 실패했습니다.");
            }

            timerAxis.Enabled = true;


            // 속도 설정 ...
            // --------------------------------------------------
            AxisSpeed = _WorkFuncInfo.AxisSpeed;

            SelectSpeed(AxisSpeed);

            MultiMotion.SetSpeed(AxisSpeed);
            // --------------------------------------------------
        }



        private void frmFuncRollingUI_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private void frmFuncRollingUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            MultiMotion.StopAll();

            MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_SLOW); // 기본 속도로 변경 ...

            timerAxis.Enabled = false;

            UpdateFormula();
        }

        private void timerAxis_Tick(object sender, EventArgs e)
        {
            UpdatePos();

            MultiMotion.CheckDefense();
        }

        private void UpdatePos()
        {
            MultiMotion.GetCurrentPos();


            if (UpdateFormula() == false)
                return;


            double dCalcValue = 0.0;
            double dResult = 0.0;

            txtLestLength.Text = (MultiMotion.dIndex_XPos - MultiMotion.AxisValue[MultiMotion.INDEX_MOVE_M]).ToString("##.00");


            if (double.TryParse(txtFLValue.Text, out DataManager.SelectedModel.dFLValue) == true)
            {
                dCalcValue = MultiMotion.dIndex_XPos - MultiMotion.AxisValue[MultiMotion.INDEX_MOVE_M] 
                    - (DataManager.SelectedModel.dMetalThick1 + DataManager.SelectedModel.dMetalThick2 - 4.5);

                dResult = ((dCalcValue - (DataManager.SelectedModel.dSLValue + DataManager.SelectedModel.dWRValue * 2))
                    / ((DataManager.SelectedModel.dFLValue - (DataManager.SelectedModel.dSLValue + DataManager.SelectedModel.dWRValue * 2)) * 0.01));
                
                txtRollingRate.Text = (100.0 - dResult).ToString();
            }

        }

#region JOG ...

        private void btnJogMinus_MouseDown(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(1, true) == MultiMotion.KSM_OK)
            {
                MultiMotion.StepMove(MultiMotion.INDEX_MOVE_M, 1, false);
            }
        }

        private void btnJogMinus_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void btnJogPlus_MouseDown(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(1, true) == MultiMotion.KSM_OK)
            {
                MultiMotion.StepMove(MultiMotion.INDEX_MOVE_M, 0, false);
            }
        }

        private void btnJogPlus_MouseUp(object sender, MouseEventArgs e)
        {
        }

#endregion JOG ...


#region Button ...

        private bool UpdateFormula()
        {
            double dTempValue = 0.0;

            // --------------------------------------------------
            if (double.TryParse(this.txtMetalThick1.Text, out dTempValue))
            {
                DataManager.SelectedModel.dMetalThick1 = dTempValue;
            }
            else
            {
                return false;
            }                
            
            if (double.TryParse(this.txtMetalThick2.Text, out dTempValue))
            {
                DataManager.SelectedModel.dMetalThick2 = dTempValue;
            }                
            else
            {
                return false;
            }                

            if (double.TryParse(this.txtFLValue.Text, out dTempValue))
            {
                DataManager.SelectedModel.dFLValue = dTempValue;
            }                
            else
            {
                return false;
            }                

            if (double.TryParse(this.txtSLValue.Text, out dTempValue))
            {
                DataManager.SelectedModel.dSLValue = dTempValue;
            }                
            else
            {
                return false;
            }                

            if (double.TryParse(this.txtCapsule.Text, out dTempValue))
            {
                DataManager.SelectedModel.dCapsulePie = dTempValue;
            }                
            else
            {
                return false;
            }                

            if (double.TryParse(this.txtWRValue.Text, out dTempValue))
            {
                DataManager.SelectedModel.dWRValue = dTempValue;
            }                
            else
            {
                return false;
            }                

            if (double.TryParse(this.txtRolling70Rate.Text, out dTempValue))
            {
                //double dRolling70Rate = (100.0 - dTempValue) / 100.0;
                DataManager.SelectedModel.dRolling70Rate = dTempValue;
            }                
            else
            {
                return false;
            }                

            if (double.TryParse(this.txtRolling80Rate.Text, out dTempValue))
            {
                //double dRolling80Rate = (100.0 - dTempValue) / 100.0;
                DataManager.SelectedModel.dRolling80Rate = dTempValue;
            }                
            else
            {
                return false;
            }                
            // --------------------------------------------------


            // 김호진 대리 공식
            // --------------------------------------------------
            MultiMotion.CalcRollingData();
            // --------------------------------------------------

            return true;
        }

        private void btnFLMove_Click(object sender, EventArgs e)
        {
            if (UpdateFormula() == false)
            {
                MessageBox.Show("계산식을 완료할 수 없습니다. 입력 내용을 확인해 주세요.");

                return;
            }

            double dMoveDis = MultiMotion.dIndex_XPos - MultiMotion.dRolling100Value;

            MultiMotion.GantryAxis(MultiMotion.INDEX_MOVE_M, MultiMotion.INDEX_MOVE_S, dMoveDis, false);
        }

        private void btnRolling70_Click(object sender, EventArgs e)
        {
            Rolling70();
        }

        private void btnRolling80_Click(object sender, EventArgs e)
        {
            Rolling80();
        }

        private void btnIndexRotate_Click(object sender, EventArgs e)
        {
            RotatingIndex();
        }

        private void btnSwing_Click(object sender, EventArgs e)
        {
            MultiMotion.Swing(true);
        }

        private void btnUnswing_Click(object sender, EventArgs e)
        {
            MultiMotion.Swing(false);
        }

        private void btnRollingUp_Click(object sender, EventArgs e)
        {
            //95.28(47.64) 파이 => 42.5 => 원점에서 중심까지 90

            MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_MIDIUM);

            double dTempValue = 0.0;

            if (double.TryParse(this.txtRollingOffset.Text, out dTempValue))
            {
                double dRollingValue = 90 - (DataManager.SelectedModel.dCapsulePie / 2.0) + dTempValue; // 안전 거리 확보를 위하여 -10함.

                MultiMotion.MoveRolling(dRollingValue, false);                
            }

            MultiMotion.SetSpeed(AxisSpeed);
        }

        private void btnRollingDown_Click(object sender, EventArgs e)
        {
            MultiMotion.StopAll();

            MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_MIDIUM);

            MultiMotion.MoveRolling(1.0, false);

            MultiMotion.SetSpeed(this.AxisSpeed);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            MultiMotion.StopAll();

            MultiMotion.bEAutoStop = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btn70Save_Click(object sender, EventArgs e)
        {
            txtRolling70Rate.Text = txtRollingRate.Text;

            //MultiMotion.bUpdatedSecondPoint = true;
        }

        private void btn80Save_Click(object sender, EventArgs e)
        {
            txtRolling80Rate.Text = txtRollingRate.Text;


            MultiMotion.bUpdatedSecondPoint = true;
        }

        private void btnVBlockUp_Click(object sender, EventArgs e)
        {
            double dTempValue = 0.0;
            double VBlockFL_Limit_Value = 0.0;
            double VBlockFL_Offset_Value = 0.0;
            double FL_Value = 0.0;


            if (double.TryParse(this.txtVBlockFL_Limit_Value.Text, out dTempValue))
            {
                VBlockFL_Limit_Value = dTempValue;
            }
            else
            {
                txtVBlockFL_Offset_Value.Text = "200.0";
            }

            if (double.TryParse(this.txtVBlockFL_Offset_Value.Text, out dTempValue))
            {
                VBlockFL_Offset_Value = dTempValue;
            }
            else
            {
                txtVBlockFL_Offset_Value.Text = "0.0";
            }


            if (double.TryParse(this.txtFLValue.Text, out dTempValue))
            {
                FL_Value = dTempValue;                
            }


            // --------------------------------------------------
            if (VBlockFL_Limit_Value < 0.1 || FL_Value < 0.1)
            {
                MessageBox.Show("FL 값과 V-Block FL 제한 값을 확인하지 못하여 취소합니다.");

                return;
            }


            if (VBlockFL_Limit_Value > FL_Value)
            {
                MessageBox.Show("FL 값이 V-Block FL 제한 값보다 작으므로 취소합니다.");

                return;
            }
            // --------------------------------------------------


            MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_SLOW);

            double dVBlockZ = 290.0 - (DataManager.SelectedModel.dCapsulePie / 2.0) + VBlockFL_Offset_Value;

            MultiMotion.MoveAxis(MultiMotion.VBLOCK_Z, dVBlockZ, false);

            MultiMotion.SetSpeed(AxisSpeed);
        }

        private void btnVBlockDown_Click(object sender, EventArgs e)
        {
            VBlockZDown();
        }

        private void VBlockZDown()
        {
            MultiMotion.StopAll();

            MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_SLOW);

            MultiMotion.MoveAxis(MultiMotion.VBLOCK_Z, 1.0, false);

            MultiMotion.SetSpeed(this.AxisSpeed);
        }

        private void SaveData()
        {
            double dTempValue = 0.0;

            int TempValue = 1;

            if (double.TryParse(this.txtMetalThick1.Text, out dTempValue))
            {
                DataManager.SelectedModel.dMetalThick1 = dTempValue;
            }

            if (double.TryParse(this.txtMetalThick2.Text, out dTempValue))
            {
                DataManager.SelectedModel.dMetalThick2 = dTempValue;
            }

            if (double.TryParse(this.txtFLValue.Text, out dTempValue))
            {
                DataManager.SelectedModel.dFLValue = dTempValue;
            }

            if (double.TryParse(this.txtSLValue.Text, out dTempValue))
            {
                DataManager.SelectedModel.dSLValue = dTempValue;
            }

            if (double.TryParse(this.txtCapsule.Text, out dTempValue))
            {
                DataManager.SelectedModel.dCapsulePie = dTempValue;
            }

            if (double.TryParse(this.txtWRValue.Text, out dTempValue))
            {
                DataManager.SelectedModel.dWRValue = dTempValue;
            }

            if (double.TryParse(this.txtRolling70Rate.Text, out dTempValue))
            {
                DataManager.SelectedModel.dRolling70Rate = dTempValue;
            }

            if (double.TryParse(this.txtRolling80Rate.Text, out dTempValue))
            {
                DataManager.SelectedModel.dRolling80Rate = dTempValue;
            }

            if (double.TryParse(this.txtRollingOffset.Text, out dTempValue))
            {
                DataManager.SelectedModel.dRollingOffset = dTempValue;
            }

            if (double.TryParse(this.txtRotateCount.Text, out dTempValue))
            {
                DataManager.SelectedModel.dRotateCount = dTempValue;
            }

            if (double.TryParse(this.txtVBlockFL_Limit_Value.Text, out dTempValue))
            {
                DataManager.SelectedModel.dVBlockFL_Limit_Value = dTempValue;                
            }

            if (double.TryParse(this.txtVBlockFL_Offset_Value.Text, out dTempValue))
            {
                DataManager.SelectedModel.dVBlockFL_Offset_Value = dTempValue;                
            }

            if (SelectedRB != null)
            {
                _WorkFuncInfo.AxisSpeed = Convert.ToInt32(SelectedRB.Tag);
            }
        }


#endregion Button ...


#region 속도 ...

        public RadioButton SelectedRB;
        public int AxisSpeed = 1;

        public void SelectSpeed(int speed)
        {
            radioBtnSSlow.Checked = false;
            radioBtnSlow.Checked = false;
            radioBtnMidium.Checked = false;
            radioBtnFast.Checked = false;


            // ----------
            switch (speed)
            {
                case MultiMotion.KSM_SPEED_SSLOW:
                    SelectedRB = radioBtnSSlow;
                    break;
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

                AxisSpeed = Convert.ToInt32(SelectedRB.Tag);

                MultiMotion.SetSpeed(AxisSpeed);
            }
        }

        public void Test()
        {
            // 카메라 Y축 +105 | -76 
            // V블럭 290mm
            // 롤링 80mm

            // --------------------------------------------------
            // MLAKK108706
            // ----------
            // FREE LENGTH : 127.0 // 139.00mm
            // CAPSULE : 95.22, 95.28 파이
            // METAL 1 : 7.87 + 2.54
            // METAL 2 : 6.35 + 2.54

            // S-L(밀착장) : 
            // W-R(링) : 

            // 식 = (SL + WR*2)
            // 70%L = (FL - 식)*0.3 + 식;

            //if (double.TryParse(this.txtRAxisValue.Text, out dTempValue))
            //MultiMotion.MoveAxis(MultiMotion.INDEX_FIX_R, dTempValue, false);

            // **********
            // 70, 80프로 위치값 설정 값으로 저장
        }

#endregion 속도 ...


#region Auto 모드 ...

        private bool Rolling70()
        {
            if (UpdateFormula() == false)
            {
                MessageBox.Show("계산식을 완료할 수 없습니다. 입력 내용을 확인해 주세요.");

                return false;
            }

            double dMoveDis = MultiMotion.dIndex_XPos - MultiMotion.dRolling70Value;                        // 70프로 위치로 이동 ...

            MultiMotion.GantryAxis(MultiMotion.INDEX_MOVE_M, MultiMotion.INDEX_MOVE_S, dMoveDis, true);

            return true;
        }

        private bool Rolling80()
        {
            if (UpdateFormula() == false)
            {
                MessageBox.Show("계산식을 완료할 수 없습니다. 입력 내용을 확인해 주세요.");

                return false;
            }

            double dMoveDis = MultiMotion.dIndex_XPos - MultiMotion.dRolling80Value;

            MultiMotion.GantryAxis(MultiMotion.INDEX_MOVE_M, MultiMotion.INDEX_MOVE_S, dMoveDis, true);    // 80프로 위치로 이동 ...

            return true;
        }

        private bool RotatingIndex()
        {
            double dTempValue = 0.0;

            if (double.TryParse(this.txtRotateCount.Text, out dTempValue))
            {
                DataManager.SelectedModel.dRotateCount = dTempValue;
            }                
            else
                return false;


            double dMoveValue = MultiMotion.AxisValue[MultiMotion.INDEX_FIX_R] + DataManager.SelectedModel.dRotateCount * 360.0;


            if (MultiMotion.GantryAxisEnable(0, true) == MultiMotion.KSM_OK)
            {
                MultiMotion.IndexGantryAxis(MultiMotion.INDEX_FIX_R, MultiMotion.INDEX_MOVE_R, dMoveValue, true);
            }

            return true;
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            // 일시 정지 버튼 대응 ...
            // ----------
            System.Windows.Forms.Application.DoEvents();
            MultiMotion.CheckDefense();

            if (MultiMotion.bEAutoStop == true)
            {
                MultiMotion.bEAutoStop = false;

                //MessageBox.Show("");

                return;
            }
            // ----------




            if (Rolling70() == false)
                return;




            // 일시 정지 버튼 대응 ...
            // ----------
            System.Windows.Forms.Application.DoEvents();
            MultiMotion.CheckDefense();

            if (MultiMotion.bEAutoStop == true)
            {
                MultiMotion.bEAutoStop = false;

                return;
            }
            // ----------




            // V-Block ...
            // ----------
            MultiMotion.StopAll();

            MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_SLOW);

            MultiMotion.MoveAxis(MultiMotion.VBLOCK_Z, 1.0, true);

            MultiMotion.SetSpeed(this.AxisSpeed);
            // ----------




            // 일시 정지 버튼 대응 ...
            // ----------
            System.Windows.Forms.Application.DoEvents();
            MultiMotion.CheckDefense();

            if (MultiMotion.bEAutoStop == true)
            {
                MultiMotion.bEAutoStop = false;

                return;
            }
            // ----------

            

            if (RotatingIndex() == false)
                return;


            // 일시 정지 버튼 대응 ...
            // ----------
            System.Windows.Forms.Application.DoEvents();
            MultiMotion.CheckDefense();

            if (MultiMotion.bEAutoStop == true)
            {
                MultiMotion.bEAutoStop = false;

                return;
            }
            // ----------


            if (Rolling80() == false)
                return;

            // 일시 정지 버튼 대응 ...
            // ----------
            System.Windows.Forms.Application.DoEvents();
            MultiMotion.CheckDefense();

            if (MultiMotion.bEAutoStop == true)
            {
                MultiMotion.bEAutoStop = false;

                return;
            }
            // ----------

            if (RotatingIndex() == false)
                return;

            // 일시 정지 버튼 대응 ...
            // ----------
            System.Windows.Forms.Application.DoEvents();
            MultiMotion.CheckDefense();

            if (MultiMotion.bEAutoStop == true)
            {
                MultiMotion.bEAutoStop = false;

                return;
            }
            // ----------


            MultiMotion.Swing(false);


            // 일시 정지 버튼 대응 ...
            // ----------
            System.Windows.Forms.Application.DoEvents();
            MultiMotion.CheckDefense();

            if (MultiMotion.bEAutoStop == true)
            {
                MultiMotion.bEAutoStop = false;

                return;
            }
            // ----------



            SaveData();

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }


#endregion Auto 모드 ...

        private void txtRolling70Rate_KeyDown(object sender, KeyEventArgs e)
        {
            double dTempValue = 0.0;

            if (double.TryParse(this.txtRolling70Rate.Text, out dTempValue))
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        {
                            if (dTempValue > 80.0 || dTempValue < 30.0)
                            {
                                txtRolling70Rate.Text = "70.0";

                                MessageBox.Show("롤링 70 위치는 30% 이상 80% 미만에서 입력할 수 있습니다.");

                                return;
                            }

                            Rolling70();
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void txtRolling80Rate_KeyDown(object sender, KeyEventArgs e)
        {
            double dTempValue = 0.0;

            if (double.TryParse(this.txtRolling80Rate.Text, out dTempValue))
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        {
                            if (dTempValue > 90.0 || dTempValue < 40)
                            {
                                txtRolling80Rate.Text = "80.0";

                                MessageBox.Show("롤링 80 위치는 40% 이상 90% 미만에서 입력할 수 있습니다.");

                                return;
                            }

                            Rolling80();
                        }
                        break;
                    default:
                        break;
                }
            }

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }



    }
}
