using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AutoAssembler
{
    public partial class frmLevelAlign : Form
    {

        public const int SETTYPE_NONE = 700;
        public const int SETTYPE_FIX_FRONT = 710;
        public const int SETTYPE_FIX_BACK = 720;
        public const int SETTYPE_MOVE_FRONT = 730;
        public const int SETTYPE_MOVE_BACK = 740;
        public const int SETTYPE_MOVE_TOP = 750;

        public int SetTypeIndex = SETTYPE_NONE;

        public double In_Level_X = 0;
        public double In_Level_Y = 0;
        public double In_Level_Angle = 0;
        public double In_Level_Distance = 0;

        public double In_Level_X_B = 0;
        public double In_Level_Y_B = 0;
        public double In_Level_Angle_B = 0;
        public double In_Level_Distance_B = 0;

        public double In_Cur_GapAngle = 0;

        public double Out_Level_Gap_Angle = 0.0;

        public double Out_FinalGapAngle = 0.0;

        public bool AngleActionFlag;

        public frmLevelAlign()
        {
            InitializeComponent();
        }

        private void frmLevelAlign_Load(object sender, EventArgs e)
        {
            txtHole_X.Text = In_Level_X.ToString("#.0000");
            txtHole_Y.Text = In_Level_Y.ToString("#.0000");
            txtHole_Angle.Text = In_Level_Angle.ToString("#.0000");
            txtHole_Distance.Text = In_Level_Distance.ToString("#.0000");

            txtHole_X_B.Text = In_Level_X_B.ToString("#.0000");
            txtHole_Y_B.Text = In_Level_Y_B.ToString("#.0000");
            txtHole_Angle_B.Text = In_Level_Angle_B.ToString("#.0000");
            txtHole_Distance_B.Text = In_Level_Distance_B.ToString("#.0000");

            txtCurGapAngle.Text = In_Cur_GapAngle.ToString("#.0000");

            AngleActionFlag = false;
        }

        private void btCalcLevel_Click(object sender, EventArgs e)
        {
            double CalcLevelGap;
            double CalcHoleDIstance;
            double CalcRealHoleAngle;
            
            int GetScreenPlanIndex;

            GetScreenPlanIndex = GetScreenPlan(In_Level_X, In_Level_Y);
            CalcLevelGap = Convert.ToDouble(txtLevelGap.Text);
            CalcHoleDIstance = Math.Sqrt((In_Level_X - In_Level_X_B) * (In_Level_X - In_Level_X_B) + (In_Level_Y - In_Level_Y_B) * (In_Level_Y - In_Level_Y_B));

            CalcRealHoleAngle = Math.Asin(CalcLevelGap / CalcHoleDIstance) * 180.0 / Math.PI;
            //CalcRealHoleAngle = Math.Asin((In_Level_Y + CalcLevelGap / 2.0) / In_Level_Distance) * 180.0 / Math.PI;

            /*
            if (CalcRealHoleAngle < 0)
            {
                CalcRealHoleAngle = 360 + CalcRealHoleAngle;
            }

            if (Math.Abs(In_Level_Angle - CalcRealHoleAngle) > 2.0)
            {
                if (CalcRealHoleAngle > 180)
                {
                    CalcRealHoleAngle = CalcRealHoleAngle - 180;
                }
                else
                {
                    CalcRealHoleAngle = 180 - CalcRealHoleAngle;
                }
            }
            */

            if (SetTypeIndex == SETTYPE_FIX_FRONT)
            {
                if (In_Level_X<0)
                {
                    //CalcRealHoleAngle = CalcRealHoleAngle;
                }
                else
                {
                    CalcRealHoleAngle = 0 - CalcRealHoleAngle;
                }
            }
            else if (SetTypeIndex == SETTYPE_MOVE_FRONT)
            {
                if (In_Level_X < 0)
                {
                    CalcRealHoleAngle = 0 - CalcRealHoleAngle;
                }
                else
                {
                    //CalcRealHoleAngle = CalcRealHoleAngle;
                }
            }

            Out_Level_Gap_Angle = CalcRealHoleAngle;

            txtResAngle.Text = Out_Level_Gap_Angle.ToString("#.####");

            Out_FinalGapAngle = In_Cur_GapAngle + Out_Level_Gap_Angle;

            txtResFinalGapAngle.Text = Out_FinalGapAngle.ToString("#.####");

        }

        private int GetScreenPlan(double ScrPl_X, double ScrPl_Y)
        {
            int ScreenPlanIndex;
            ScreenPlanIndex = 1;
            if (ScrPl_X < 0)
            {
                if (ScrPl_Y < 0)
                {
                    ScreenPlanIndex = 3;
                }
                else
                {
                    ScreenPlanIndex = 2;
                }
            }
            else
            {
                if (ScrPl_Y < 0)
                {
                    ScreenPlanIndex = 4;
                }
                else
                {
                    ScreenPlanIndex = 1;
                }
            }
            return ScreenPlanIndex;
        }

        private void btAdjustAngle_Click(object sender, EventArgs e)
        {
            AngleActionFlag = true;
            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        private void btAdjustOnlyAngle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }

        private void txtLevelGap_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                btCalcLevel_Click(this, new EventArgs());
            }
        }
    }
}

