using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//////////

using DiaDetector.Data;
using DiaDetector.Drivers;
using PAIX_NMF_DEV;

namespace DiaDetector
{
    public partial class frmJog : Form
    {
        public frmJog()
        {
            InitializeComponent();
        }


        private void Initialize()
        {
            SelectSpeed(MultiMotion.KSM_SPEED_SLOW);

            
            // Init DIO ...
            // ----------
            for (int i=0; i<32; i++)
            {
                Button btn = (Controls.Find("Btn_In" + i.ToString("00"), true)[0] as Button);

                //btn.Text = DataManager.DIOSettingInfoList[i].Name + "(" + btn.Tag.ToString() + ")";
            }


            int k = 0;

            for (int i = 32; i < 64; i++)
            {
                k = i - 32;

                Button btn = (Controls.Find("Btn_Out" + k.ToString("00"), true)[0] as Button);

                //btn.Text = DataManager.DIOSettingInfoList[i].Name + "(" + btn.Tag.ToString() + ")";
            }

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
            if (MainFrame.testMod)
            {
                return;
            }

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


            // 카메라 1 ...
            // ----------
            if (MultiMotion.AlarmValue[MultiMotion.Camera1Adjust] == 1)
                Camera1Alarm.ImageIndex = 1;
            else
                Camera1Alarm.ImageIndex = 0;

            Camera1.Text = MultiMotion.AxisValue[MultiMotion.Camera1Adjust].ToString("0.####");


            // 카메라 2 ...
            // ----------
            if (MultiMotion.AlarmValue[MultiMotion.Camera2Adjust] == 1)
                Camera2Alarm.ImageIndex = 1;
            else
                Camera2Alarm.ImageIndex = 0;

            Camera2.Text = MultiMotion.AxisValue[MultiMotion.Camera2Adjust].ToString("0.####");

            // 리프트1
            // ----------
            if (MultiMotion.AlarmValue[MultiMotion.Lift1Motor] == 1)
              Lift2MotorAlarm. ImageIndex = 1;
            else
                Lift2MotorAlarm.ImageIndex = 0;

            Lift1.Text = MultiMotion.AxisValue[MultiMotion.Lift1Motor].ToString("0.####");

            


            // 리프트2
            // ----------
            if (MultiMotion.AlarmValue[MultiMotion.Lift2Motor] == 1)
                Shuttle2MotorAlarm.ImageIndex = 1;
            else
                Shuttle2MotorAlarm.ImageIndex = 0;

            Lift2.Text = MultiMotion.AxisValue[MultiMotion.Lift2Motor].ToString("0.####");

            


            // 셔틀1
            // ----------
            if (MultiMotion.AlarmValue[MultiMotion.Shuttle1Motor] == 1)
               Shuttle1MotorAlarm. ImageIndex = 1;
            else
                Shuttle1MotorAlarm.ImageIndex = 0;

           Shuttle1. Text = MultiMotion.AxisValue[MultiMotion.Shuttle1Motor].ToString("0.####");


            // 셔틀2
            // ----------
            if (MultiMotion.AlarmValue[MultiMotion.Shuttle2Motor] == 1)
                Shuttle2MotorAlarm.ImageIndex = 1;
            else
                Shuttle2MotorAlarm.ImageIndex = 0;

            Shuttle2.Text = MultiMotion.AxisValue[MultiMotion.Shuttle2Motor].ToString("0.####");



            // 로테이션
            // ----------
            if (MultiMotion.AlarmValue[MultiMotion.RotationMotor] == 1)
               RotationMotorAlarm. ImageIndex = 1;
            else
                RotationMotorAlarm.ImageIndex = 0;

            Rotation.Text = MultiMotion.AxisValue[MultiMotion.RotationMotor].ToString("0.####");

            labelShuttle2Move.Text = SmallClass.shuttleMove1.ToString();
            labelShuttle1Move.Text = SmallClass.shuttleMove2.ToString();

            label90Move.Text = DataManager.sinmove94.ToString();
            label0Move.Text = DataManager.sinmove4.ToString();
            label45Move.Text = DataManager.sinmove49.ToString();




            //// 가이드 모터 R ...
            //// ----------
            //if (MultiMotion.AlarmValue[MultiMotion.Lift2Motor] == 1)
            //    Shuttle2MotorAlarm.ImageIndex = 1;
            //else
            //    Shuttle2MotorAlarm.ImageIndex = 0;

            //Shuttle2.Text = MultiMotion.AxisValue[MultiMotion.Lift2Motor].ToString("0.####");




            /*
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




            */
        }


#region Camera ...


       

 


      //카몌라1번

        private void Camera1JogL_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.Camera1Adjust, 1);
        }

        private void Camera1JogL_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.Camera1Adjust);
        }

        private void Camera1JogR_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.Camera1Adjust, 0);
        }

        private void Camera1JogR_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.Camera1Adjust);
        }

        private void Camera1Home_Click(object sender, EventArgs e)
        {
            if (checkBoxHomeClear.Checked == true)
                MultiMotion.HomeMove(MultiMotion.Camera1Adjust, true);
            else
                MultiMotion.HomeMove(MultiMotion.Camera1Adjust, false);
        }

        private void Camera1Alarm_Click(object sender, EventArgs e)
        {
            AlarmReset();
        }


    //카메라2

        private void Camera2JogL_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.Camera2Adjust, 1);
        }

        private void Camera2JogL_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.Camera2Adjust);
        }

        private void Camera2JogR_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.Camera2Adjust, 0);
        }

        private void Camera2JogR_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.Camera2Adjust);
        }

        private void Camera2Home_Click(object sender, EventArgs e)
        {
            if (checkBoxHomeClear.Checked == true)
                MultiMotion.HomeMove(MultiMotion.Camera2Adjust, true);
            else
                MultiMotion.HomeMove(MultiMotion.Camera2Adjust, false);
        }

        private void Camera2Alarm_Click(object sender, EventArgs e)
        {
            AlarmReset();
        }

        //리프트1번

        private void Lift1MotorL_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.Lift1Motor, 1);
        }

        private void Lift1MotorL_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.Lift1Motor);
        }

        private void Lift1MotorR_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.Lift1Motor, 0);
        }

        private void Lift1MotorR_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.Lift1Motor);
        }

        private void Lift1MotorHome_Click(object sender, EventArgs e)
        {
            /*
            if (MessageBox.Show("완료 대기를 기다리홈 이동 명령원점 프로그램을 종료하시겠습니까?", 
                "종료여부", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            */

            if (checkBoxHomeClear.Checked == true)
                MultiMotion.HomeMove(MultiMotion.Lift1Motor, true);
            else
                MultiMotion.HomeMove(MultiMotion.Lift1Motor, false);
        }

        private void Lift1MotorAlarm_Click(object sender, EventArgs e)
        {
            AlarmReset();
        }



        //리프트2번

        private void Lift2MotorL_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.Lift2Motor, 1);
        }

        private void Lift2MotorL_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.Lift2Motor);
        }

        private void Lift2MotorR_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.Lift2Motor, 0);
        }

        private void Lift2MotorR_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.Lift2Motor);
        }

        private void Lift2MotorHome_Click(object sender, EventArgs e)
        {
            if (checkBoxHomeClear.Checked == true)
                MultiMotion.HomeMove(MultiMotion.Lift2Motor, true);
            else
                MultiMotion.HomeMove(MultiMotion.Lift2Motor, false);
        }

        private void Lift2MotorAlarm_Click(object sender, EventArgs e)
        {
            AlarmReset();
        }







        //셔틀1번
        private void Shuttle1MotorL_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.Shuttle1Motor, 1);
        }

        private void Shuttle1MotorL_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.Shuttle1Motor);
        }

        private void Shuttle1MotorR_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.Shuttle1Motor, 0);
        }

        private void Shuttle1MotorR_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.Shuttle1Motor);
        }


      

        private void Shuttle1MotorHome_Click(object sender, EventArgs e)
        {
            if (checkBoxHomeClear.Checked == true)
                MultiMotion.HomeMove(MultiMotion.Shuttle1Motor, true);
            else
                MultiMotion.HomeMove(MultiMotion.Shuttle1Motor, false);

        }

        private void Shuttle1MotorAlarm_Click(object sender, EventArgs e)
        {
            AlarmReset();
        }


     //셔틀2번

        private void Shuttle2MotorL_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.Shuttle2Motor, 1);
        }

        private void Shuttle2MotorL_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.Shuttle2Motor);
        }

        private void Shuttle2MotorR_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.Shuttle2Motor, 0);
        }

        private void Shuttle2MotorR_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.Shuttle2Motor);
        }

        private void Shuttle2MotorHome_Click(object sender, EventArgs e)
        {
            if (checkBoxHomeClear.Checked == true)
                MultiMotion.HomeMove(MultiMotion.Shuttle2Motor, true);
            else
                MultiMotion.HomeMove(MultiMotion.Shuttle2Motor, false);
        }

        private void Shuttle2MotorAlarm_Click(object sender, EventArgs e)
        {
            AlarmReset();
        }


        //로테이션
        private void RotationMotorL_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.RotationMotor, 1);
        }

        private void RotationMotorL_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.RotationMotor);
        }

        private void RotationMotorR_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.RotationMotor, 0);
        }

        private void RotationMotorR_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.RotationMotor);
        }
        private void RotationMotorHome_Click(object sender, EventArgs e)
        {
            //if (checkBoxHomeClear.Checked == true)
            MultiMotion.HomeMove(MultiMotion.RotationMotor, true);
            //else
            //    MultiMotion.HomeMove(MultiMotion.RotationMotor, false);
        }
        private void RotationMotorAlarm_Click(object sender, EventArgs e)
        {
            AlarmReset();
        }

       

#endregion Camera ...


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


            

        //    MultiMotion.SetSpeed(AxisSpeed);
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

                MultiMotion.SetSpeed(MultiMotion.RotationMotor,AxisSpeed);
            }            
        }


#endregion 속도 ...


#region Temp ...

        private void AlarmReset()
        {
            MultiMotion.AlarmReset();
        }

        private void btnClearIndexAngle_Click(object sender, EventArgs e)
        {
            MultiMotion.IndexRPosClear();
        }

        private void btnStopAll_Click(object sender, EventArgs e)
        {
            MultiMotion.StopAll();
        }

#endregion Temp ...


#region Disital Input/Output ...


        private void UpdateDIO()
        {
            if (MultiMotion.GetDIOStatus() == MultiMotion.KSM_OK)
            {
                for (int i = 0; i < 32; i++)
                {
                    Button btn = (Controls.Find("Btn_Out" + i.ToString("00"), true)[0] as Button);
                    btn.ImageIndex = MultiMotion.OutStatus[i];

                    btn = (Controls.Find("Btn_In" + i.ToString("00"), true)[0] as Button);
                    btn.ImageIndex = MultiMotion.InStatus[i];

                    if (i < 2)
                    {
                        btn = (Controls.Find("button_NMF_IN" + i, true)[0] as Button);
                        btn.ImageIndex = MultiMotion.InStatusNMF[i];
                    }
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
                    //Paix_NMF_Controler.SetPin(DeviceManagerS.g_ndevIdC_44, bitno, 0);
                    SmallClass.SetPin(DeviceManagerS.g_ndevIdA_4, bitno, 0);
                }
                else
                {
                    //Paix_NMF_Controler.SetPin(DeviceManagerS.g_ndevIdC_44, bitno, 1);
                    SmallClass.SetPin(DeviceManagerS.g_ndevIdA_4, bitno, 1);
                }
            }  

            /*
            Button btn = (Button)sender;

            short bitno = -1;

            if (btn != null)
            {
                bitno = Convert.ToInt16(btn.Tag);

                MultiMotion.GetDIOStatus();

                if (MultiMotion.OutStatus[bitno] == 1)
                {
                    MultiMotion.Write_Output(bitno, false);
                }
                else
                {
                    MultiMotion.Write_Output(bitno, true);
                }                
            }    
            */
        }


        #endregion Disital Output ...

        private void btn90move_Click(object sender, EventArgs e)
        {
            MultiMotion.MoveAxis(MultiMotion.RotationMotor, DataManager.sinmove94, false);
        }

        private void btn00move_Click(object sender, EventArgs e)
        {
            MultiMotion.MoveAxis(MultiMotion.RotationMotor, DataManager.sinmove4, false);
        }

        private void btn45move_Click(object sender, EventArgs e)
        {
            MultiMotion.MoveAxis(MultiMotion.RotationMotor, DataManager.sinmove49, false);
        }


        private void btn90save_Click(object sender, EventArgs e)
        {
            double dtemp = Convert.ToDouble(MultiMotion.AxisValue[MultiMotion.RotationMotor].ToString("0.####"));
            if (dtemp < DataManager.bsinmove94 - DataManager.dgab || dtemp > DataManager.bsinmove94 + DataManager.dgab)
            {
                MessageBox.Show("범위에서 벗어난 위치입니다. 재조정 후 저장하십시오");
                return;
            }

            if (MessageBox.Show("선택한 값으로 변경하시겠습니까?", "값 변경",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataManager.sinmove94 = Convert.ToDouble(MultiMotion.AxisValue[MultiMotion.RotationMotor].ToString("0.####"));

                DeviceManagerS.Write_MotionPos();
            }


        }

        private void btn00save_Click(object sender, EventArgs e)
        {
            double dtemp = Convert.ToDouble(MultiMotion.AxisValue[MultiMotion.RotationMotor].ToString("0.####"));
            if (dtemp < DataManager.bsinmove4 - DataManager.dgab || dtemp > DataManager.bsinmove4 + DataManager.dgab)
            {
                MessageBox.Show("범위에서 벗어난 위치입니다. 재조정 후 저장하십시오");
                return;
            }

            if (MessageBox.Show("선택한 값으로 변경하시겠습니까?", "값 변경",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataManager.sinmove4 = Convert.ToDouble(MultiMotion.AxisValue[MultiMotion.RotationMotor].ToString("0.####"));

                DeviceManagerS.Write_MotionPos();
            }


        }

        private void btn45save_Click(object sender, EventArgs e)
        {
            double dtemp = Convert.ToDouble(MultiMotion.AxisValue[MultiMotion.RotationMotor].ToString("0.####"));
            if (dtemp < DataManager.bsinmove49 - DataManager.dgab || dtemp > DataManager.bsinmove49 + DataManager.dgab)
            {
                MessageBox.Show("범위에서 벗어난 위치입니다. 재조정 후 저장하십시오");
                return;
            }

            if (MessageBox.Show("선택한 값으로 변경하시겠습니까?", "값 변경",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataManager.sinmove49 = Convert.ToDouble(MultiMotion.AxisValue[MultiMotion.RotationMotor].ToString("0.####"));

                DeviceManagerS.Write_MotionPos();
            }

        }

        private void buttonShuttle1AbsMove_Click(object sender, EventArgs e)
        {
            double value = textBoxShuttle1Abs.Text == "" ? 0 : Convert.ToDouble(textBoxShuttle1Abs.Text);

            MultiMotion.MoveAxis(MultiMotion.Shuttle1Motor, value, false);
        }

        private void buttonShuttle2AbsMove_Click(object sender, EventArgs e)
        {
            double value = textBoxShuttle2Abs.Text == "" ? 0 : Convert.ToDouble(textBoxShuttle2Abs.Text);

            MultiMotion.MoveAxis(MultiMotion.Shuttle2Motor, value, false);
        }

        private void buttonAbsMove_Click(object sender, EventArgs e)
        {
            double value = textBoxAbs.Text == "" ? 0 : Convert.ToDouble(textBoxAbs.Text);

            MultiMotion.MoveAxis(MultiMotion.RotationMotor, value, false);
        }

        private void buttonShuttle1Move_Click(object sender, EventArgs e)
        {
            MultiMotion.MoveAxis(MultiMotion.Shuttle2Motor, SmallClass.shuttleMove1, false);
        }

        private void buttonShuttle2Move_Click(object sender, EventArgs e)
        {
            MultiMotion.MoveAxis(MultiMotion.Shuttle2Motor, SmallClass.shuttleMove2, false);
        }

        private void buttonShuttle1Save_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("선택한 값으로 변경하시겠습니까?", "값 변경",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SmallClass.shuttleMove1 = Convert.ToDouble(MultiMotion.AxisValue[MultiMotion.Shuttle1Motor].ToString("0.####"));
            }
        }

        private void buttonShuttle2Save_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("선택한 값으로 변경하시겠습니까?", "값 변경",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SmallClass.shuttleMove1 = Convert.ToDouble(MultiMotion.AxisValue[MultiMotion.Shuttle1Motor].ToString("0.####"));
            }
        }
    }
}
