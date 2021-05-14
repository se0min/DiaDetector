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
using AutoAssembler.Utilities;

namespace AutoAssembler
{
    public partial class frmJogAxis : Form
    {
        public frmJogAxis()
        {
            InitializeComponent();
        }


        private void Initialize()
        {
            SelectSpeed(MultiMotion.KSM_SPEED_SLOW);


            // Init DIO ...
            // ----------
            for (int i=0; i<64; i++)
            {
                Button btn = (Controls.Find("Btn_In" + i.ToString("00"), true)[0] as Button);

                btn.Text = DataManager.DIOSettingInfoList[i].Name + "(" + btn.Tag.ToString() + ")";
            }


            int k = 0;

            for (int i = 64; i < 128; i++)
            {
                k = i - 64;

                Button btn = (Controls.Find("Btn_Out" + k.ToString("00"), true)[0] as Button);

                btn.Text = DataManager.DIOSettingInfoList[i].Name + "(" + btn.Tag.ToString() + ")";
            }

            /*
            Button btn = (Controls.Find("Btn_Out" + i.ToString("00"), true)[0] as Button);
            btn.ImageIndex = MultiMotion.OutStatus[i];

            btn = (Controls.Find("Btn_In" + i.ToString("00"), true)[0] as Button);
            btn.ImageIndex = MultiMotion.InStatus[i];
            */
            // ----------


            timerAxis.Enabled = true;


        }

        private void frmJogAxis_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private void frmJogAxis_FormClosing(object sender, FormClosingEventArgs e)
        {
            SelectSpeed(MultiMotion.KSM_SPEED_SLOW);
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {

        }


        private void timerAxis_Tick(object sender, EventArgs e)
        {
            UpdatePos();

            UpdateDIO();
        }


        private void UpdatePos()
        {
            MultiMotion.GetCurrentPos();

            if (checkBoxDefence.Checked == true)
            {
                MultiMotion.CheckDefense();
            }


            // 1. 카메라 유닛 X ...
            // --------------------------------------------------
            if (MultiMotion.AlarmValue[MultiMotion.CAM_UNIT_X] == 1)
                btnCamUnit_X_A.ImageIndex = 1;
            else
                btnCamUnit_X_A.ImageIndex = 0;

            txtCamUnitValue_X.Text = MultiMotion.AxisValue[MultiMotion.CAM_UNIT_X].ToString("0.####");


            // 2. 카메라 유닛 Y ...
            // --------------------------------------------------
            if (MultiMotion.AlarmValue[MultiMotion.CAM_UNIT_Y] == 1)
                btnCamUnit_Y_A.ImageIndex = 1;
            else
                btnCamUnit_Y_A.ImageIndex = 0;

            txtCamUnitValue_Y.Text = MultiMotion.AxisValue[MultiMotion.CAM_UNIT_Y].ToString("0.####");


            // 3. 카메라 유닛 Z ...
            // --------------------------------------------------
            if (MultiMotion.AlarmValue[MultiMotion.CAM_UNIT_Z] == 1)
                btnCamUnit_Z_A.ImageIndex = 1;
            else
                btnCamUnit_Z_A.ImageIndex = 0;

            txtCamUnitValue_Z.Text = MultiMotion.AxisValue[MultiMotion.CAM_UNIT_Z].ToString("0.####");



            // 4. 고정축 롤링 A ...
            // --------------------------------------------------
            if (MultiMotion.AlarmValue[MultiMotion.ROLLING_FIX_1] == 1)
                btnRollingFix_A_A.ImageIndex = 1;
            else
                btnRollingFix_A_A.ImageIndex = 0;

            txtRollingValue_Fix_A.Text = MultiMotion.AxisValue[MultiMotion.ROLLING_FIX_1].ToString("0.####");



            // 5. 고정축 롤링 B ...
            // --------------------------------------------------
            if (MultiMotion.AlarmValue[MultiMotion.ROLLING_FIX_2] == 1)
                btnRollingFix_B_A.ImageIndex = 1;
            else
                btnRollingFix_B_A.ImageIndex = 0;

            txtRollingValue_Fix_B.Text = MultiMotion.AxisValue[MultiMotion.ROLLING_FIX_2].ToString("0.####");


            // 6. 이동축 롤링 A ...
            // --------------------------------------------------
            if (MultiMotion.AlarmValue[MultiMotion.ROLLING_MOVE_1] == 1)
                btnRollingMove_A_A.ImageIndex = 1;
            else
                btnRollingMove_A_A.ImageIndex = 0;

            txtRollingValue_Move_A.Text = MultiMotion.AxisValue[MultiMotion.ROLLING_MOVE_1].ToString("0.####");


            // 7. 이동축 롤링 B ...
            // --------------------------------------------------
            if (MultiMotion.AlarmValue[MultiMotion.ROLLING_MOVE_2] == 1)
                btnRollingMove_B_A.ImageIndex = 1;
            else
                btnRollingMove_B_A.ImageIndex = 0;

            txtRollingValue_Move_B.Text = MultiMotion.AxisValue[MultiMotion.ROLLING_MOVE_2].ToString("0.####");


            // 8. INDEX 고정축 R ...
            // --------------------------------------------------
            if (MultiMotion.AlarmValue[MultiMotion.INDEX_FIX_R] == 1)
                btnIndexFix_A.ImageIndex = 1;
            else
                btnIndexFix_A.ImageIndex = 0;

            txtIndexFixValue.Text = MultiMotion.AxisValue[MultiMotion.INDEX_FIX_R].ToString("0.####");


            // 9. INDEX 이동축 R ...
            // --------------------------------------------------
            if (MultiMotion.AlarmValue[MultiMotion.INDEX_MOVE_R] == 1)
                btnIndexMove_A.ImageIndex = 1;
            else
                btnIndexMove_A.ImageIndex = 0;

            txtIndexMoveValue.Text = MultiMotion.AxisValue[MultiMotion.INDEX_MOVE_R].ToString("0.####");



            // 10. INDEX X축 이동 ...
            // --------------------------------------------------
            if (MultiMotion.AlarmValue[MultiMotion.INDEX_MOVE_M] == 1)
                btnIndex_X_A.ImageIndex = 1;
            else
                btnIndex_X_A.ImageIndex = 0;

            if (MultiMotion.AlarmValue[MultiMotion.INDEX_MOVE_S] == 1)
                btnIndex_X_A2.ImageIndex = 1;
            else
                btnIndex_X_A2.ImageIndex = 0;


            txtIndex_X_Value.Text = MultiMotion.AxisValue[MultiMotion.INDEX_MOVE_M].ToString("0.####");


            // 11. V-Block Z축 이동 ...
            // --------------------------------------------------
            if (MultiMotion.AlarmValue[MultiMotion.VBLOCK_Z] == 1)
                btnVBlock_Z_A.ImageIndex = 1;
            else
                btnVBlock_Z_A.ImageIndex = 0;

            txtVBlockValue.Text = MultiMotion.AxisValue[MultiMotion.VBLOCK_Z].ToString("0.####");


            // 12. 후방 카메라 Z축 이동 ...
            // --------------------------------------------------
            if (MultiMotion.AlarmValue[MultiMotion.BACK_CAM_Z] == 1)
                btnBCam_Z_A.ImageIndex = 1;
            else
                btnBCam_Z_A.ImageIndex = 0;

            txtBackCamValue.Text = MultiMotion.AxisValue[MultiMotion.BACK_CAM_Z].ToString("0.####");
        }


#region Camera ...

        // X축 ...
        // --------------------------------------------------

        private void btnCamUnit_X_M_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.CAM_UNIT_X, 1);
        }

        private void btnCamUnit_X_M_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.CAM_UNIT_X);

            //MultiMotion.StopAll(); // 방어 코드 ...
        }

        private void btnCamUnit_X_P_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.CAM_UNIT_X, 0);
        }

        private void btnCamUnit_X_P_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.CAM_UNIT_X);

            //MultiMotion.StopAll(); // 방어 코드 ...
        }

        private void btnCamUnit_X_H_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("완료 대기를 기다리홈 이동 명령원점 프로그램을 종료하시겠습니까?", "종료여부", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)

            //MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_X, 10.0, true);

            if (checkBoxHomeClear.Checked == true)
                MultiMotion.HomeMove(MultiMotion.CAM_UNIT_X, true);
            else
                MultiMotion.HomeMove(MultiMotion.CAM_UNIT_X, false);
        }

        private void btnCamUnit_X_A_Click(object sender, EventArgs e)
        {
            AlarmReset();
        }

        // Y축 ...
        // --------------------------------------------------

        private void btnCamUnit_Y_M_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.CAM_UNIT_Y, 1);
        }

        private void btnCamUnit_Y_M_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.CAM_UNIT_Y);

            //MultiMotion.StopAll(); // 방어 코드 ...
        }

        private void btnCamUnit_Y_P_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.CAM_UNIT_Y, 0);
        }

        private void btnCamUnit_Y_P_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.CAM_UNIT_Y);

            //MultiMotion.StopAll(); // 방어 코드 ...
        }

        private void btnCamUnit_Y_H_Click(object sender, EventArgs e)
        {
            if (checkBoxHomeClear.Checked == true)
                MultiMotion.HomeMove(MultiMotion.CAM_UNIT_Y, true);
            else
                MultiMotion.HomeMove(MultiMotion.CAM_UNIT_Y, false);
        }

        private void btnCamUnit_Y_A_Click(object sender, EventArgs e)
        {
            AlarmReset();
        }

        // Z축 ...
        // --------------------------------------------------

        private void btnCamUnit_Z_M_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.CAM_UNIT_Z, 1);
        }

        private void btnCamUnit_Z_M_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.CAM_UNIT_Z);

            //MultiMotion.StopAll(); // 방어 코드 ...
        }

        private void btnCamUnit_Z_P_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.CAM_UNIT_Z, 0);
        }

        private void btnCamUnit_Z_P_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.CAM_UNIT_Z);

            //MultiMotion.StopAll(); // 방어 코드 ...
        }

        private void btnCamUnit_Z_H_Click(object sender, EventArgs e)
        {
            if (checkBoxHomeClear.Checked == true)
                MultiMotion.HomeMove(MultiMotion.CAM_UNIT_Z, true);
            else
                MultiMotion.HomeMove(MultiMotion.CAM_UNIT_Z, false);
        }

        private void btnCamUnit_Z_A_Click(object sender, EventArgs e)
        {
            AlarmReset();
        }

#endregion Camera ...


#region Rolling ...


        // 고정축 A ...
        // --------------------------------------------------


        private void btnRollingFix_A_M_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.ROLLING_FIX_1, 1);
        }

        private void btnRollingFix_A_M_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.ROLLING_FIX_1);

            //MultiMotion.StopAll(); // 방어 코드 ...
        }

        private void btnRollingFix_A_P_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.ROLLING_FIX_1, 0);
        }

        private void btnRollingFix_A_P_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.ROLLING_FIX_1);

            //MultiMotion.StopAll(); // 방어 코드 ...
        }

        private void btnRollingFix_A_H_Click(object sender, EventArgs e)
        {
            if (checkBoxHomeClear.Checked == true)
                MultiMotion.HomeMove(MultiMotion.ROLLING_FIX_1, true);
            else
                MultiMotion.HomeMove(MultiMotion.ROLLING_FIX_1, false);
        }

        private void btnRollingFix_A_A_Click(object sender, EventArgs e)
        {
            AlarmReset();
        }


        // 고정축 B ...
        // --------------------------------------------------


        private void btnRollingFix_B_M_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.ROLLING_FIX_2, 1);
        }

        private void btnRollingFix_B_M_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.ROLLING_FIX_2);

            //MultiMotion.StopAll(); // 방어 코드 ...
        }

        private void btnRollingFix_B_P_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.ROLLING_FIX_2, 0);
        }

        private void btnRollingFix_B_P_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.ROLLING_FIX_2);

            //MultiMotion.StopAll(); // 방어 코드 ...
        }

        private void btnRollingFix_B_H_Click(object sender, EventArgs e)
        {
            if (checkBoxHomeClear.Checked == true)
                MultiMotion.HomeMove(MultiMotion.ROLLING_FIX_2, true);
            else
                MultiMotion.HomeMove(MultiMotion.ROLLING_FIX_2, false);
        }

        private void btnRollingFix_B_A_Click(object sender, EventArgs e)
        {
            AlarmReset();
        }


        // 이동축 A ...
        // --------------------------------------------------


        private void btnRollingMove_A_M_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.ROLLING_MOVE_1, 1);
        }

        private void btnRollingMove_A_M_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.ROLLING_MOVE_1);

            //MultiMotion.StopAll(); // 방어 코드 ...
        }

        private void btnRollingMove_A_P_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.ROLLING_MOVE_1, 0);
        }

        private void btnRollingMove_A_P_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.ROLLING_MOVE_1);

            //MultiMotion.StopAll(); // 방어 코드 ...
        }

        private void btnRollingMove_A_H_Click(object sender, EventArgs e)
        {
            if (checkBoxHomeClear.Checked == true)
                MultiMotion.HomeMove(MultiMotion.ROLLING_MOVE_1, true);
            else
                MultiMotion.HomeMove(MultiMotion.ROLLING_MOVE_1, false);

        }

        private void btnRollingMove_A_A_Click(object sender, EventArgs e)
        {
            AlarmReset();
        }



        // 이동축 B ...
        // --------------------------------------------------


        private void btnRollingMove_B_M_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.ROLLING_MOVE_2, 1);
        }

        private void btnRollingMove_B_M_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.ROLLING_MOVE_2);

            //MultiMotion.StopAll(); // 방어 코드 ...
        }

        private void btnRollingMove_B_P_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.ROLLING_MOVE_2, 0);
        }

        private void btnRollingMove_B_P_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.ROLLING_MOVE_2);

            //MultiMotion.StopAll(); // 방어 코드 ...
        }

        private void btnRollingMove_B_H_Click(object sender, EventArgs e)
        {
            if (checkBoxHomeClear.Checked == true)
                MultiMotion.HomeMove(MultiMotion.ROLLING_MOVE_2, true);
            else
                MultiMotion.HomeMove(MultiMotion.ROLLING_MOVE_2, false);
        }

        private void btnRollingMove_B_A_Click(object sender, EventArgs e)
        {
            AlarmReset();
        }


        // 롤링, 스윙 ...
        // --------------------------------------------------

        private void btnSwingDown_Click(object sender, EventArgs e)
        {
            MultiMotion.Swing(true);
        }

        private void btnSwingUp_Click(object sender, EventArgs e)
        {
            MultiMotion.Swing(false);
        }


        private void btnRollingUp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("모델 편집 기능이 완료된 후에 적용할 예정입니다.");

            return;

            MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_SLOW);

            double dRollingValue = 90 - (DataManager.SelectedModel.dCapsulePie / 2.0); // 안전 거리 확보를 위하여 -10함.

            MultiMotion.MoveRolling(dRollingValue, false);

            MultiMotion.SetSpeed(AxisSpeed);
        }

        private void btnRollingDown_Click(object sender, EventArgs e)
        {
            MultiMotion.StopAll();

            MultiMotion.MoveRolling(1.0, true);

            MultiMotion.HomeMove(MultiMotion.ROLLING_FIX_1, true);  // 고정축 롤링 1
            MultiMotion.HomeMove(MultiMotion.ROLLING_FIX_2, true);  // 고정축 롤링 2
            MultiMotion.HomeMove(MultiMotion.ROLLING_MOVE_1, true); // 이동축 롤링 1
            MultiMotion.HomeMove(MultiMotion.ROLLING_MOVE_2, true); // 이동축 롤링 2

            MessageBox.Show("Rolling Down이 완료되었습니다.");
        }


#endregion Rolling ...


#region INDEX ...


        // INDEX 갠트리 ...
        // --------------------------------------------------


        private void btnIndexGantry_M_MouseDown(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, true) == MultiMotion.KSM_OK)
            {
                timerAxis.Enabled = true;

                MultiMotion.JogMove(MultiMotion.INDEX_FIX_R, 1);
            }
        }

        private void btnIndexGantry_M_MouseUp(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, true) == MultiMotion.KSM_OK)
            {
                MultiMotion.JogStop(MultiMotion.INDEX_FIX_R);

                //MultiMotion.StopAll(); // 방어 코드 ...
            }
        }

        private void btnIndexGantry_P_MouseDown(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, true) == MultiMotion.KSM_OK)
            {
                timerAxis.Enabled = true;

                MultiMotion.JogMove(MultiMotion.INDEX_FIX_R, 0);
            }
        }

        private void btnIndexGantry_P_MouseUp(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, true) == MultiMotion.KSM_OK)
            {
                MultiMotion.JogStop(MultiMotion.INDEX_FIX_R);

                //MultiMotion.StopAll(); // 방어 코드 ...
            }
        }


        // INDEX 고정축 ...
        // --------------------------------------------------

        private void btnIndexFix_M_MouseDown(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK)
            {
                //timerAxis.Enabled = true;

                MultiMotion.JogMove(MultiMotion.INDEX_FIX_R, 1);
            }
        }

        private void btnIndexFix_M_MouseUp(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK)
            {
                MultiMotion.JogStop(MultiMotion.INDEX_FIX_R);
            }

            //MultiMotion.StopAll(); // 방어 코드 ...
        }

        private void btnIndexFix_P_MouseDown(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK)
            {
                //timerAxis.Enabled = true;

                MultiMotion.JogMove(MultiMotion.INDEX_FIX_R, 0);
            }
        }

        private void btnIndexFix_P_MouseUp(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK)
            {
                MultiMotion.JogStop(MultiMotion.INDEX_FIX_R);
            }

            //MultiMotion.StopAll(); // 방어 코드 ...
        }

        private void btnIndexFix_H_Click(object sender, EventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK)
            {
                //timerAxis.Enabled = true;

                if (checkBoxHomeClear.Checked == true)
                    MultiMotion.HomeMove(MultiMotion.INDEX_FIX_R, true);
                else
                    MultiMotion.HomeMove(MultiMotion.INDEX_FIX_R, false);
            }
        }

        private void btnIndexFix_A_Click(object sender, EventArgs e)
        {
            AlarmReset();
        }


        // INDEX 이동축 ...
        // --------------------------------------------------


        private void btnIndexMove_M_MouseDown(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK)
            {
                //timerAxis.Enabled = true;

                MultiMotion.JogMove(MultiMotion.INDEX_MOVE_R, 1);
            }
        }

        private void btnIndexMove_M_MouseUp(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK)
            {
                MultiMotion.JogStop(MultiMotion.INDEX_MOVE_R);
            }
        }

        private void btnIndexMove_P_MouseDown(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK)
            {
                //timerAxis.Enabled = true;

                MultiMotion.JogMove(MultiMotion.INDEX_MOVE_R, 0);
            }
        }

        private void btnIndexMove_P_MouseUp(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK)
            {
                MultiMotion.JogStop(MultiMotion.INDEX_MOVE_R);
            }
        }

        private void btnIndexMove_H_Click(object sender, EventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK)
            {
                //timerAxis.Enabled = true;

                if (checkBoxHomeClear.Checked == true)
                    MultiMotion.HomeMove(MultiMotion.INDEX_MOVE_R, true);
                else
                    MultiMotion.HomeMove(MultiMotion.INDEX_MOVE_R, false);
            }
        }

        private void btnIndexMove_A_Click(object sender, EventArgs e)
        {
            AlarmReset();
        }


        // INDEX 갠트리 X축 ...
        // --------------------------------------------------


        private void btnIndex_X_M_MouseDown(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(1, true) == MultiMotion.KSM_OK)
            {
                //timerAxis.Enabled = true;

                MultiMotion.JogMove(MultiMotion.INDEX_MOVE_M, 1);
            }
        }

        private void btnIndex_X_M_MouseUp(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(1, true) == MultiMotion.KSM_OK)
            {
                MultiMotion.JogStop(MultiMotion.INDEX_MOVE_M);
            }
        }

        private void btnIndex_X_P_MouseDown(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(1, true) == MultiMotion.KSM_OK)
            {
                //timerAxis.Enabled = true;

                MultiMotion.JogMove(MultiMotion.INDEX_MOVE_M, 0);
            }
        }

        private void btnIndex_X_P_MouseUp(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(1, true) == MultiMotion.KSM_OK)
            {
                MultiMotion.JogStop(MultiMotion.INDEX_MOVE_M);
            }

        }

        private void btnIndex_X_H_Click(object sender, EventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(1, true) == MultiMotion.KSM_OK)
            {
                //timerAxis.Enabled = true;

                if (checkBoxHomeClear.Checked == true)
                    MultiMotion.HomeMove(MultiMotion.INDEX_MOVE_M, true);
                else
                    MultiMotion.HomeMove(MultiMotion.INDEX_MOVE_M, false);
            }
        }

        private void btnIndex_X_A_Click(object sender, EventArgs e)
        {
            AlarmReset();
        }

        private void btnIndex_X_A2_Click(object sender, EventArgs e)
        {
            AlarmReset();
        }


#endregion INDEX ...


#region 기타 ...


        // V-Block Z축 ...
        // --------------------------------------------------

        private void btnVBlock_Z_M_MouseDown(object sender, MouseEventArgs e)
        {
            //timerAxis.Enabled = true;

            MultiMotion.JogMove(MultiMotion.VBLOCK_Z, 1);
        }

        private void btnVBlock_Z_M_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.VBLOCK_Z);
        }

        private void btnVBlock_Z_P_MouseDown(object sender, MouseEventArgs e)
        {
            //timerAxis.Enabled = true;

            MultiMotion.JogMove(MultiMotion.VBLOCK_Z, 0);
        }

        private void btnVBlock_Z_P_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.VBLOCK_Z);
        }

        private void btnVBlock_Z_H_Click(object sender, EventArgs e)
        {
            //timerAxis.Enabled = true;

            if (checkBoxHomeClear.Checked == true)
            {
                MultiMotion.HomeMove(MultiMotion.VBLOCK_Z, true);
            }                
            else
            {
                MultiMotion.HomeMove(MultiMotion.VBLOCK_Z, false);
            }                
        }

        private void btnVBlock_Z_A_Click(object sender, EventArgs e)
        {
            AlarmReset();
        }


        // 후방 카메라 Z축 ...
        // --------------------------------------------------

        private void btnBCam_Z_M_MouseDown(object sender, MouseEventArgs e)
        {
            //timerAxis.Enabled = true;

            MultiMotion.JogMove(MultiMotion.BACK_CAM_Z, 1);
        }

        private void btnBCam_Z_M_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.BACK_CAM_Z);
        }

        private void btnBCam_Z_P_MouseDown(object sender, MouseEventArgs e)
        {
            //timerAxis.Enabled = true;

            MultiMotion.JogMove(MultiMotion.BACK_CAM_Z, 0);
        }

        private void btnBCam_Z_P_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.BACK_CAM_Z);
        }

        private void btnBCam_Z_H_Click(object sender, EventArgs e)
        {
            //timerAxis.Enabled = true;

            if (checkBoxHomeClear.Checked == true)
            {
                MultiMotion.HomeMove(MultiMotion.BACK_CAM_Z, true);
            }                
            else
            {
                MultiMotion.HomeMove(MultiMotion.BACK_CAM_Z, false);
            }                
        }

        private void btnBCam_Z_A_Click(object sender, EventArgs e)
        {
            AlarmReset();
        }


#endregion 기타 ...

        
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


            

            MultiMotion.SetSpeed(AxisSpeed);
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


#endregion 속도 ...


#region Temp ...

        private void AlarmReset()
        {
            MultiMotion.AlarmReset();
        }

#endregion Temp ...






#region Disital Input/Output ...


        private void UpdateDIO()
        {
            if (MultiMotion.GetDIOStatus() == MultiMotion.KSM_OK)
            {
                for (int i = 0; i < 64; i++)
                {

                    Button btn = (Controls.Find("Btn_Out" + i.ToString("00"), true)[0] as Button);
                    btn.ImageIndex = MultiMotion.OutStatus[i];

                    btn = (Controls.Find("Btn_In" + i.ToString("00"), true)[0] as Button);
                    btn.ImageIndex = MultiMotion.InStatus[i];
                }
            }
        }

        private void Btn_Disital_Output_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            short bitno = -1;

            if (btn != null)
            {
                bitno = Convert.ToInt16(btn.Tag);

                if (MultiMotion.OutStatus[bitno] == 1)
                {
                    NMC2.nmc_SetDIOOutputBit(DeviceManager.g_ndevIdC_4, bitno, 0);
                }
                else
                {
                    NMC2.nmc_SetDIOOutputBit(DeviceManager.g_ndevIdC_4, bitno, 1);
                }
            }            
        }


#endregion Disital Output ...

        private void btnUnlockPos_Click(object sender, EventArgs e)
        {            
            if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK)
            {
                timerAxis.Enabled = true;

                //MultiMotion.MoveAxis(MultiMotion.INDEX_FIX_R, 0.0, false);
                //MultiMotion.MoveAxis(MultiMotion.INDEX_MOVE_R, 0.0, false);

                MultiMotion.HomeMove(MultiMotion.INDEX_FIX_R, false);
                MultiMotion.HomeMove(MultiMotion.INDEX_MOVE_R, false);
            }
        }

        private void btnClearIndexAngle_Click(object sender, EventArgs e)
        {
            MultiMotion.IndexRPosClear();
        }

        private void btnStopAll_Click(object sender, EventArgs e)
        {
            MultiMotion.StopAll();
        }

#region Memo ...

        // 알람 리셋 후 => 홈, 비전 X축은 Z축 먼저 홈으로 보내고

#endregion Memo ...

    }
}
