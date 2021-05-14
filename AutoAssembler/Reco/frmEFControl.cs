using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AutoAssembler.Data;
using AutoAssembler.Drivers;
using AutoAssembler.VisionLibrary; 
using AutoAssembler.Utilities;


using DXFImportReco;

namespace AutoAssembler
{
    public struct DXFDataInfo
    {
        public CADImage FCADImage;// = new CADImage();
        public float FScale;
        public Point Base;
        public float FS_W_Base, FS_H_Base, FS_W, FS_H;
        public bool StartCalcFlag;

    }

    public struct EF_DXFDataInfo
    {
        public float StartPos;
        public float EndPos;
        public float CompressPos80;
        public float CompressPos100;
    }

    public partial class frmEFControl : Form
    {
        public WorkFuncInfo _WorkFuncInfo;  // 홍동성 => 시나리오 정보 저장

        public string LoadDXFFilesA;
        public string LoadDXFFilesB;
        private DXFDataInfo DXFDatA;
        private DXFDataInfo DXFDatB;
        private Bitmap BlankA;
        private Bitmap BlankB;

        public frmEFControl()
        {
            InitializeComponent();
        }

        private void btDXFOpenA_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "CAD Files|*.dxf";
            openFileDialog1.Title = "Select a CAD File";

            if (openFileDialog1.ShowDialog(this) != DialogResult.OK) return;
            if (openFileDialog1.FileName != null)
            {
                LoadDXFFilesA = openFileDialog1.FileName;
                DXFDatA.StartCalcFlag = true;
                if (DXFDatA.FCADImage != null)
                {
                    DXFDatA.FCADImage = null;
                }
                DXFDatA.FCADImage = new CADImage();
                DXFDatA.Base.X = picDXFIndexA.Width / 2;
                DXFDatA.Base.Y = picDXFIndexA.Width / 2;

                //FCADImage.Base.Y = Bottom - 400;
                //FCADImage.Base.X = 100;
                DXFDatA.FScale = 1.0f;
                DXFDatA.FCADImage.LoadFromFile(openFileDialog1.FileName);
                //DrawCADImage();
                Bitmap tmp = BlankA;
                //FCADImage.FScale = 4.0f;
                DrawDXFImageCalc(tmp.Width, tmp.Height, ref DXFDatA);
                DrawDXFImage2((Bitmap)tmp, ref DXFDatA);
                picDXFIndexA.Image = (Image)tmp.Clone();
                EF_DXFDataInfo GetEFDXFData = GetEF_DXFData(ref DXFDatA);
                DisplayGetEFData(GetEFDXFData);
            }
        }

        private void DisplayGetEFData(EF_DXFDataInfo GetEFDXFData)
        {
            txtDXFStartPos.Text = GetEFDXFData.StartPos.ToString();
            txtDXFEndPos.Text = GetEFDXFData.EndPos.ToString();
            txtDXFPress80Pos.Text = GetEFDXFData.CompressPos80.ToString();
            txtDXFPress100Pos.Text = GetEFDXFData.CompressPos100.ToString();
        }

        private void frmEFControl_Load(object sender, EventArgs e)
        {
            BlankA = DrawFilledRectangle(picDXFIndexA.Width, picDXFIndexA.Height);
            BlankB = DrawFilledRectangle(picDXFIndexB.Width, picDXFIndexB.Height);
        }

        private Bitmap DrawFilledRectangle(int x, int y)
        {
            Bitmap bmp = new Bitmap(x, y);
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                Rectangle ImageSize = new Rectangle(0, 0, x, y);
                graph.FillRectangle(Brushes.Black, ImageSize);
            }
            return bmp;
        }


        public void DrawDXFImageCalc(int DrImgW, int DrImgH, ref DXFDataInfo DrawDXFDat)
        {
            string GetEntityName;
            DXFLine dxLine;
            DXFCircle dxCircle;
            DXFArc dxArc;
            SFPoint P1, P2;
            float rd1, rd2;
            float MinX, MinY, MaxX, MaxY;

            MinX = 99999.0f;
            MinY = 99999.0f;
            MaxX = -99999.0f;
            MaxY = -99999.0f;
            if (DrawDXFDat.FCADImage == null)
                return;

            //Bitmap tmp = new Bitmap(bmp, bmp.Width, bmp.Height);

            for (int i = 0; i < DrawDXFDat.FCADImage.FEntities.Entities.Count; i++)
            {
                GetEntityName = DrawDXFDat.FCADImage.FEntities.Entities[i].ToString();
                if (GetEntityName == "DXFImportReco.DXFLine")
                {
                    /*
                    dxLine = (DXFLine)FCADImage.FEntities.Entities[i];

                    blackPen.Color = dxLine.FColor;

                    P1 = GetPoint(dxLine.Point1);
                    P2 = GetPoint(dxLine.Point2);

                    graphics.DrawLine(blackPen, P1.X, P1.Y, P2.X, P2.Y);
                    if (MinX > P1.X)
                    {
                        MinX = P1.X;
                    }
                    if (MaxX < P1.X)
                    {
                        MaxX = P1.X;
                    }
                    if (MinY > P1.Y)
                    {
                        MinY = P1.Y;
                    }
                    if (MaxY < P1.Y)
                    {
                        MaxY = P1.Y;
                    }

                    if (MinX > P2.X)
                    {
                        MinX = P2.X;
                    }
                    if (MaxX < P2.X)
                    {
                        MaxX = P2.X;
                    }
                    if (MinY > P2.Y)
                    {
                        MinY = P2.Y;
                    }
                    if (MaxY < P2.Y)
                    {
                        MaxY = P2.Y;
                    }
                    */
                }
                else if (GetEntityName == "DXFImportReco.DXFCircle")
                {
                    dxCircle = (DXFCircle)DrawDXFDat.FCADImage.FEntities.Entities[i];

                    if (DrawDXFDat.StartCalcFlag == true)
                    {
                        dxCircle.SelectObjFlag = false;
                    }

                    rd1 = dxCircle.radius;
                    P1 = dxCircle.Point1;
                    //rd1 = rd1;// *FScale;

                    if (MinX > P1.X - rd1)
                    {
                        MinX = P1.X - rd1;
                    }
                    if (MaxX < P1.X + rd1)
                    {
                        MaxX = P1.X + rd1;
                    }
                    if (MinY > P1.Y - rd1)
                    {
                        MinY = P1.Y - rd1;
                    }
                    if (MaxY < P1.Y + rd1)
                    {
                        MaxY = P1.Y + rd1;
                    }


                }
                else if (GetEntityName == "DXFImportReco.DXFArc")
                {
                    /*
                    dxArc = (DXFArc)FCADImage.FEntities.Entities[i];

                    blackPen.Color = dxArc.FColor;

                    if (dxArc.pt1.X == 0)
                    {
                        rd1 = dxArc.radius; //Math.Abs(dxArc.radius * dxArc.ratio);
                        rd2 = dxArc.radius;
                    }
                    else
                    {
                        rd1 = dxArc.radius;
                        rd2 = dxArc.radius; //Math.Abs(dxArc.radius * ratio);
                    }

                    rd1 = dxArc.radius;
                    P1 = GetPoint(dxArc.Point1);
                    rd1 = rd1 * FScale;
                    rd2 = rd2 * FScale;
                    P1.X = P1.X - rd1;
                    P1.Y = P1.Y - rd1;
                    float sA = -dxArc.startAngle, eA = -dxArc.endAngle;
                    if (dxArc.endAngle < dxArc.startAngle) sA = Conversion_Angle(sA);
                    eA -= sA;

                    if (eA == 0)
                    {
                        graphics.DrawEllipse(blackPen, P1.X, P1.Y, rd1 * 2, rd2 * 2);
                    }
                    else
                    {
                        graphics.DrawArc(blackPen, P1.X, P1.Y, rd1 * 2, rd2 * 2, 0, 360);//sA, eA);
                    }
                    if (MinX > P1.X - rd1)
                    {
                        MinX = P1.X - rd1;
                    }
                    if (MaxX < P1.X + rd1)
                    {
                        MaxX = P1.X + rd1;
                    }
                    if (MinY > P1.Y - rd1)
                    {
                        MinY = P1.Y - rd1;
                    }
                    if (MaxY < P1.Y + rd1)
                    {
                        MaxY = P1.Y + rd1;
                    }
                    */
                }
                else
                {
                    //OutTxtStr += "\r\n" + GetEntityName;
                }
            }
            if (DrawDXFDat.StartCalcFlag == true)
            {
                DrawDXFDat.FS_W = MaxX - MinX;
                DrawDXFDat.FS_H = MaxY - MinY;
                DrawDXFDat.FS_W_Base = MinX;
                DrawDXFDat.FS_H_Base = MinY;

                float W_FScale, H_FScale;
                W_FScale = ((float)DrImgW * 80.0f / 100.0f) / DrawDXFDat.FS_W;
                H_FScale = ((float)DrImgH * 80.0f / 100.0f) / DrawDXFDat.FS_H;
                if (W_FScale < H_FScale)
                {
                    DrawDXFDat.FScale = W_FScale;
                }
                else
                {
                    DrawDXFDat.FScale = H_FScale;
                }


                DrawDXFDat.StartCalcFlag = false;
            }
            //bmp = tmp;
        }


        public EF_DXFDataInfo GetEF_DXFData(ref DXFDataInfo DrawDXFDat)
        {
            EF_DXFDataInfo ResOutEF_DXFDat;
            string GetEntityName;
            DXFLine dxLine;
            DXFCircle dxCircle;
            DXFArc dxArc;
            SFPoint P1, P2;
            float rd1, rd2;
            float MinX, MaxX;
            float IntvalX,IntvalY;
            float CurPress80, CurPress100;

            ResOutEF_DXFDat.StartPos = 0.0f;
            ResOutEF_DXFDat.EndPos = 0.0f;
            ResOutEF_DXFDat.CompressPos80 = 0.0f;
            ResOutEF_DXFDat.CompressPos100 = 0.0f;
            
            MinX = 9999999.0f;
            MaxX = -9999999.0f;
            CurPress80 = 0.0f;
            CurPress100 = 0.0f;

            if (DrawDXFDat.FCADImage == null)
                return ResOutEF_DXFDat;

            //Bitmap tmp = new Bitmap(bmp, bmp.Width, bmp.Height);

            for (int i = 0; i < DrawDXFDat.FCADImage.FEntities.Entities.Count; i++)
            {
                GetEntityName = DrawDXFDat.FCADImage.FEntities.Entities[i].ToString();
                if (GetEntityName == "DXFImportReco.DXFLine")
                {
                    dxLine = (DXFLine)DrawDXFDat.FCADImage.FEntities.Entities[i];

                    IntvalX = dxLine.Point1.X - dxLine.Point2.X;
                    IntvalY = dxLine.Point1.Y - dxLine.Point2.Y;
                    IntvalX = Math.Abs(IntvalX);
                    IntvalY = Math.Abs(IntvalY);
                    if(IntvalX<0.001 && IntvalY > 1.0)
                    {
                        if (MinX > dxLine.Point1.X)
                        {
                            MinX = dxLine.Point1.X;
                        }
                        if (MaxX < dxLine.Point1.X)
                        {
                            MaxX = dxLine.Point1.X;
                        }
                        if (dxLine.FColor == Color.Blue)
                        {
                            CurPress80 = dxLine.Point1.X;
                        }
                        if (dxLine.FColor == Color.Red)
                        {
                            CurPress100 = dxLine.Point1.X;
                        }
                    }

                }
                else if (GetEntityName == "DXFImportReco.DXFCircle")
                {
                    /*
                    dxCircle = (DXFCircle)DrawDXFDat.FCADImage.FEntities.Entities[i];

                    if (DrawDXFDat.StartCalcFlag == true)
                    {
                        dxCircle.SelectObjFlag = false;
                    }

                    rd1 = dxCircle.radius;
                    P1 = dxCircle.Point1;
                    //rd1 = rd1;// *FScale;
                    */


                }
                else if (GetEntityName == "DXFImportReco.DXFArc")
                {
                    /*
                    dxArc = (DXFArc)FCADImage.FEntities.Entities[i];

                    blackPen.Color = dxArc.FColor;

                    if (dxArc.pt1.X == 0)
                    {
                        rd1 = dxArc.radius; //Math.Abs(dxArc.radius * dxArc.ratio);
                        rd2 = dxArc.radius;
                    }
                    else
                    {
                        rd1 = dxArc.radius;
                        rd2 = dxArc.radius; //Math.Abs(dxArc.radius * ratio);
                    }

                    rd1 = dxArc.radius;
                    P1 = GetPoint(dxArc.Point1);
                    rd1 = rd1 * FScale;
                    rd2 = rd2 * FScale;
                    P1.X = P1.X - rd1;
                    P1.Y = P1.Y - rd1;
                    float sA = -dxArc.startAngle, eA = -dxArc.endAngle;
                    if (dxArc.endAngle < dxArc.startAngle) sA = Conversion_Angle(sA);
                    eA -= sA;

                    if (eA == 0)
                    {
                        graphics.DrawEllipse(blackPen, P1.X, P1.Y, rd1 * 2, rd2 * 2);
                    }
                    else
                    {
                        graphics.DrawArc(blackPen, P1.X, P1.Y, rd1 * 2, rd2 * 2, 0, 360);//sA, eA);
                    }
                    if (MinX > P1.X - rd1)
                    {
                        MinX = P1.X - rd1;
                    }
                    if (MaxX < P1.X + rd1)
                    {
                        MaxX = P1.X + rd1;
                    }
                    if (MinY > P1.Y - rd1)
                    {
                        MinY = P1.Y - rd1;
                    }
                    if (MaxY < P1.Y + rd1)
                    {
                        MaxY = P1.Y + rd1;
                    }
                    */
                }
                else
                {
                    //OutTxtStr += "\r\n" + GetEntityName;
                }
            }

            ResOutEF_DXFDat.StartPos = 0.0f;
            ResOutEF_DXFDat.EndPos = MaxX - MinX;
            ResOutEF_DXFDat.CompressPos80 = MaxX - CurPress80;
            ResOutEF_DXFDat.CompressPos100 = MaxX - CurPress100;

            return ResOutEF_DXFDat;
        }



        public void DrawDXFImage2(Bitmap bmp, ref DXFDataInfo DrawDXFDat)
        {
            string GetEntityName;
            DXFLine dxLine;
            DXFCircle dxCircle;
            DXFArc dxArc;
            SFPoint P1, P2;
            float rd1, rd2;
            float MinX, MinY, MaxX, MaxY;

            MinX = 99999.0f;
            MinY = 99999.0f;
            MaxX = -99999.0f;
            MaxY = -99999.0f;
            if (DrawDXFDat.FCADImage == null)
                return;

            Pen blackPen = new Pen(Color.LightGreen, 2);

            Pen SelectPen = new Pen(Color.Black, 2);
            //Bitmap tmp = new Bitmap(bmp, bmp.Width, bmp.Height);

            // Draw line to screen.
            using (var graphics = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < DrawDXFDat.FCADImage.FEntities.Entities.Count; i++)
                {
                    GetEntityName = DrawDXFDat.FCADImage.FEntities.Entities[i].ToString();
                    if (GetEntityName == "DXFImportReco.DXFLine")
                    {
                        dxLine = (DXFLine)DrawDXFDat.FCADImage.FEntities.Entities[i];

                        blackPen.Color = dxLine.FColor;

                        P1 = GetPoint(dxLine.Point1, ref DrawDXFDat);
                        P2 = GetPoint(dxLine.Point2, ref DrawDXFDat);

                        if (dxLine.SelectObjFlag == true)
                        {
                            graphics.DrawLine(SelectPen, P1.X, P1.Y, P2.X, P2.Y);
                        }
                        else
                        {
                            graphics.DrawLine(blackPen, P1.X, P1.Y, P2.X, P2.Y);
                        }

                    }
                    else if (GetEntityName == "DXFImportReco.DXFCircle")
                    {
                        dxCircle = (DXFCircle)DrawDXFDat.FCADImage.FEntities.Entities[i];

                        blackPen.Color = dxCircle.FColor;

                        rd1 = dxCircle.radius;
                        P1 = GetPoint(dxCircle.Point1, ref DrawDXFDat);
                        rd1 = rd1 * DrawDXFDat.FScale;
                        P1.X = P1.X - rd1;
                        P1.Y = P1.Y - rd1;

                        if (dxCircle.SelectObjFlag == true)
                        {
                            graphics.DrawEllipse(blackPen, P1.X, P1.Y, rd1 * 2, rd1 * 2);
                            //graphics.DrawEllipse(SelectPen, P1.X, P1.Y, rd1 * 2, rd1 * 2);
                        }
                        else
                        {
                            graphics.DrawEllipse(blackPen, P1.X, P1.Y, rd1 * 2, rd1 * 2);
                        }
                    }
                    else if (GetEntityName == "DXFImportReco.DXFArc")
                    {
                        /*
                        dxArc = (DXFArc)FCADImage.FEntities.Entities[i];

                        blackPen.Color = dxArc.FColor;

                        if (dxArc.pt1.X == 0)
                        {
                            rd1 = dxArc.radius; //Math.Abs(dxArc.radius * dxArc.ratio);
                            rd2 = dxArc.radius;
                        }
                        else
                        {
                            rd1 = dxArc.radius;
                            rd2 = dxArc.radius; //Math.Abs(dxArc.radius * ratio);
                        }

                        rd1 = dxArc.radius;
                        P1 = GetPoint(dxArc.Point1);
                        rd1 = rd1 * FScale;
                        rd2 = rd2 * FScale;
                        P1.X = P1.X - rd1;
                        P1.Y = P1.Y - rd1;
                        float sA = -dxArc.startAngle, eA = -dxArc.endAngle;
                        if (dxArc.endAngle < dxArc.startAngle) sA = Conversion_Angle(sA);
                        eA -= sA;

                        if (eA == 0)
                        {
                            graphics.DrawEllipse(blackPen, P1.X, P1.Y, rd1 * 2, rd2 * 2);
                        }
                        else
                        {
                            graphics.DrawArc(blackPen, P1.X, P1.Y, rd1 * 2, rd2 * 2, 0, 360);//sA, eA);
                        }
                        */
                    }
                    else
                    {
                        //OutTxtStr += "\r\n" + GetEntityName;
                    }
                }
            }
        }

        public SFPoint GetPoint(SFPoint Point, ref DXFDataInfo DrawDXFDat)
        {
            SFPoint P;
            P.X = DrawDXFDat.Base.X + DrawDXFDat.FScale * (Point.X - DrawDXFDat.FS_W / 2 - DrawDXFDat.FS_W_Base);// * FParams.Scale.X);
            P.Y = DrawDXFDat.Base.Y - DrawDXFDat.FScale * (Point.Y - DrawDXFDat.FS_H / 2 - DrawDXFDat.FS_H_Base);// * FParams.Scale.Y);
            P.Z = Point.Z * DrawDXFDat.FScale;
            return P;
        }

        private void btDXFOpenB_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "CAD Files|*.dxf";
            openFileDialog1.Title = "Select a CAD File";

            if (openFileDialog1.ShowDialog(this) != DialogResult.OK) return;
            if (openFileDialog1.FileName != null)
            {
                LoadDXFFilesB = openFileDialog1.FileName;
                DXFDatB.StartCalcFlag = true;
                if (DXFDatB.FCADImage != null)
                {
                    DXFDatB.FCADImage = null;
                }
                DXFDatB.FCADImage = new CADImage();
                DXFDatB.Base.X = picDXFIndexB.Width / 2;
                DXFDatB.Base.Y = picDXFIndexB.Width / 2;

                //FCADImage.Base.Y = Bottom - 400;
                //FCADImage.Base.X = 100;
                DXFDatB.FScale = 1.0f;
                DXFDatB.FCADImage.LoadFromFile(openFileDialog1.FileName);
                //DrawCADImage();
                Bitmap tmp = BlankB;
                //FCADImage.FScale = 4.0f;
                DrawDXFImageCalc(tmp.Width, tmp.Height, ref DXFDatB);
                DrawDXFImage2((Bitmap)tmp, ref DXFDatB);
                picDXFIndexB.Image = (Image)tmp.Clone();
            }
        }

        private void picDXFIndexA_MouseDown(object sender, MouseEventArgs e)
        {
            SelectDXFObj(e.X, e.Y, picDXFIndexA.Width, picDXFIndexA.Height, picDXFIndexA.Image.Width, picDXFIndexA.Image.Height, ref DXFDatA);
            Bitmap tmp = BlankA;
            DrawDXFImageCalc(picDXFIndexA.Image.Width, picDXFIndexA.Image.Height, ref DXFDatA);
            DrawDXFImage2((Bitmap)tmp, ref DXFDatA);
            picDXFIndexA.Image = (Image)tmp.Clone();

        }

        private void frmEFControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            BlankA = null;
            BlankB = null;
        }

        public void SelectDXFObj(int Sel_X, int Sel_Y, int Obj_W, int Obj_H, int Data_W, int Data_H, ref DXFDataInfo DrawDXFDat)
        {
            string GetEntityName;
            DXFLine dxLine;
            DXFCircle dxCircle;
            DXFArc dxArc;
            SFPoint P1, P2;
            float rd1, rd2;
            //float MinX, MinY, MaxX, MaxY;
            float MinSelDisCalc;
            float MinSelDisNum = 999999.0f;
            int MinSelIndex;
            float Real_X, Real_Y;
            float IntvalX, IntvalY;
            SFPoint P3; 

            Real_X = Sel_X * Data_W / Obj_W;
            Real_Y = Sel_Y * Data_H / Obj_H;

            MinSelIndex = -1;

            if (DrawDXFDat.FCADImage == null)
                return;

            for (int i = 0; i < DrawDXFDat.FCADImage.FEntities.Entities.Count; i++)
            {
                GetEntityName = DrawDXFDat.FCADImage.FEntities.Entities[i].ToString();
                if (GetEntityName == "DXFImportReco.DXFLine")
                {

                    dxLine = (DXFLine)DrawDXFDat.FCADImage.FEntities.Entities[i];

                    P1 = GetPoint(dxLine.Point1, ref DrawDXFDat);
                    P2 = GetPoint(dxLine.Point2, ref DrawDXFDat);
                    IntvalX = P1.X - P2.X;
                    IntvalY = P1.Y - P2.Y;
                    IntvalX = Math.Abs(IntvalX);
                    IntvalY = Math.Abs(IntvalY);
                    if (IntvalX < 0.001 && IntvalY > 1.0)
                    {
                        P3.X = (P1.X - P2.X) / 2 + P2.X;
                        P3.Y = (P1.Y - P2.Y) / 2 + P2.Y;
                        MinSelDisCalc = (float)Math.Sqrt(9 * (Real_X - P3.X) * (Real_X - P3.X) + (Real_Y - P3.Y) * (Real_Y - P3.Y));

                        if (MinSelDisCalc < 0)
                        {
                            MinSelDisCalc = 0 - MinSelDisCalc;
                        }

                        if (MinSelDisCalc < MinSelDisNum && MinSelDisCalc < 90)
                        {
                            MinSelDisNum = MinSelDisCalc;
                            MinSelIndex = i;
                        }
                        else
                        {
                        }
                    }
                }
                else if (GetEntityName == "DXFImportReco.DXFCircle")
                {
                    /*
                    dxCircle = (DXFCircle)DrawDXFDat.FCADImage.FEntities.Entities[i];

                    rd1 = dxCircle.radius;
                    P1 = GetPoint(dxCircle.Point1, ref DrawDXFDat);
                    rd1 = rd1 * DrawDXFDat.FScale;

                    MinSelDisCalc = (float)Math.Sqrt((Real_X - P1.X) * (Real_X - P1.X) + (Real_Y - P1.Y) * (Real_Y - P1.Y)) - rd1;

                    if (MinSelDisCalc < 0)
                    {
                        MinSelDisCalc = 0 - MinSelDisCalc;
                    }

                    if (MinSelDisCalc < MinSelDisNum && MinSelDisCalc < 80)
                    {
                        MinSelDisNum = MinSelDisCalc;
                        MinSelIndex = i;
                    }
                    else
                    {
                        //dxCircle.SelectObjFlag = false;
                    }
                    */

                }
                else if (GetEntityName == "DXFImportReco.DXFArc")
                {
                    /*
                    dxArc = (DXFArc)FCADImage.FEntities.Entities[i];

                    blackPen.Color = dxArc.FColor;

                    if (dxArc.pt1.X == 0)
                    {
                        rd1 = dxArc.radius; //Math.Abs(dxArc.radius * dxArc.ratio);
                        rd2 = dxArc.radius;
                    }
                    else
                    {
                        rd1 = dxArc.radius;
                        rd2 = dxArc.radius; //Math.Abs(dxArc.radius * ratio);
                    }

                    rd1 = dxArc.radius;
                    P1 = GetPoint(dxArc.Point1);
                    rd1 = rd1 * FScale;
                    rd2 = rd2 * FScale;
                    P1.X = P1.X - rd1;
                    P1.Y = P1.Y - rd1;
                    float sA = -dxArc.startAngle, eA = -dxArc.endAngle;
                    if (dxArc.endAngle < dxArc.startAngle) sA = Conversion_Angle(sA);
                    eA -= sA;

                    if (eA == 0)
                    {
                        graphics.DrawEllipse(blackPen, P1.X, P1.Y, rd1 * 2, rd2 * 2);
                    }
                    else
                    {
                        graphics.DrawArc(blackPen, P1.X, P1.Y, rd1 * 2, rd2 * 2, 0, 360);//sA, eA);
                    }
                    */
                }
                else
                {
                    //OutTxtStr += "\r\n" + GetEntityName;
                }
            }

            for (int i = 0; i < DrawDXFDat.FCADImage.FEntities.Entities.Count; i++)
            {
                GetEntityName = DrawDXFDat.FCADImage.FEntities.Entities[i].ToString();
                if (GetEntityName == "DXFImportReco.DXFLine")
                {
                    dxLine = (DXFLine)DrawDXFDat.FCADImage.FEntities.Entities[i];
                    if (MinSelIndex == i)
                    {
                        dxLine.SelectObjFlag = true;
                    }
                    else
                    {
                        dxLine.SelectObjFlag = false;
                    }
                }
                else if (GetEntityName == "DXFImportReco.DXFCircle")
                {
                    /*
                    dxCircle = (DXFCircle)DrawDXFDat.FCADImage.FEntities.Entities[i];
                    if (MinSelIndex == i)
                    {
                        dxCircle.SelectObjFlag = true;
                    }
                    else
                    {
                        dxCircle.SelectObjFlag = false;
                    }
                    */
                }
            }
        }

        private void picDXFIndexB_MouseDown(object sender, MouseEventArgs e)
        {
            SelectDXFObj(e.X, e.Y, picDXFIndexB.Width, picDXFIndexB.Height, picDXFIndexB.Image.Width, picDXFIndexB.Image.Height, ref DXFDatB);
            Bitmap tmp = BlankB;
            DrawDXFImageCalc(picDXFIndexB.Image.Width, picDXFIndexB.Image.Height, ref DXFDatB);
            DrawDXFImage2((Bitmap)tmp, ref DXFDatB);
            picDXFIndexB.Image = (Image)tmp.Clone();

        }

        private void btSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }
    }
}
