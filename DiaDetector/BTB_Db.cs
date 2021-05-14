using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft_OleDb;
using System.Collections;
namespace DiaDetector
{
    public partial class BTB_Db : Form
    {
        public BTB_Db()
        {
            InitializeComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save as Excel File";
            sfd.Filter = "Excel Files(2013)|*.xls";
            sfd.FileName = "";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                dataGridView_ExportToExcel(sfd.FileName, dataGridView1);
            }       
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Read();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            this.Close();
        }
        public void Columns()
        {
            try
            {
                this.Controls.Add(dataGridView1);
                dataGridView1.ColumnCount = 2;
                dataGridView1.Columns[0].Name = "번호";
                dataGridView1.Columns[1].Name = "생산 시간";
            }
            catch (Exception ex)
            {
            }
        }
               private void SetDoNotSort(DataGridView dgv)
        {
            foreach (DataGridViewColumn i in dgv.Columns)
            {
                i.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public void ColumnsSize()
        {
            try
            {
                dataGridView1.Columns[0].Width = 40;
                dataGridView1.Columns[1].Width = 100;
                dataGridView1.Columns[2].Width = 90;
                dataGridView1.Columns[3].Width = 125;
                dataGridView1.Columns[4].Width = 70;
                dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                SetDoNotSort(dataGridView1);    //모드를 풀고
                dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    //가운데 정렬
              

             
               dataGridView1.Columns["번호"].DisplayIndex = 0;
                dataGridView1.Columns["Production"].DisplayIndex = 1;
                dataGridView1.Columns["Model"].DisplayIndex = 2;
                dataGridView1.Columns["Numbers"].DisplayIndex = 3;
                dataGridView1.Columns["생산 시간"].DisplayIndex = 4;

                this.dataGridView1.Columns[2].HeaderText = "생산 날짜";
                this.dataGridView1.Columns[3].HeaderText = "모델명";
                this.dataGridView1.Columns[4].HeaderText = "CV개수";
            
            }
            catch (Exception ex)
            {
            }
        }
        List<string> arry = new List<string>();
       public int[] Modellist;
        public void Read()
        {
            try
            {
                arry.Clear();
                Modellist = Enumerable.Repeat<int>(0, 0).ToArray<int>();
                Columns();
                string start = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                string end = dateTimePicker2.Value.ToString("yyyy-MM-dd");
               // label1.Text = start;
                //label2.Text = end;
                string sql = " SELECT * FROM Table1 WHERE  (Production >= '" + start + "' AND Production <= '" + end + "') ";
                DataSet ds = Microsoft_OleDb.Microsoft_OleDb.GetDataRead(sql);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    //arry.Add((string)row["times"].ToString());
                    arry.Add((string)row["times"]);
                }
                Modellist = new int[arry.Count];
                for (int i = 0; i < arry.Count; i++)
                {
                    Modellist[i] = Int32.Parse(arry[i]);
                }
                string sql1 = " SELECT Production,Model,Numbers FROM Table1 WHERE  (Production >= '" + start + "' AND Production <= '" + end + "') ORDER BY Production";
                DataSet ds1 = Microsoft_OleDb.Microsoft_OleDb.GetDataReads(sql1);

                dataGridView1.DataSource = ds1.Tables[0];
                for (int i = 1; i < this.dataGridView1.Rows.Count + 1; i++)
                {
                    dataGridView1["번호", i - 1].Value = i.ToString();
                }
                for (int i = 1; i <= arry.Count; i++)
                {
                    dataGridView1["생산 시간", i - 1].Value = Modellist[i - 1] / 3600 + "시간:" + Modellist[i - 1] % 3600 / 60 + "분" + Modellist[i - 1] % 3600 % 60 + "초"; 
                    //LBLMakeOut.Text = (ClassType.MakeOutTime / 3600).ToString() + "시:" + (ClassType.MakeOutTime % 3600 / 60) Ou+ "분" + (ClassType.MaketTime % 3600 % 60) + "초";
                 //   dataGridView1["생산 시간", i - 1].Value = i.ToString();
                }
                ds.Dispose();
                ds1.Dispose();
                ColumnsSize();
            }
            catch (Exception EX)
            {
            }
        }

               public static void dataGridView_ExportToExcel(string fileName, DataGridView dgv)
        {
            try
            {
                Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                if (excelApp == null)
                {
                    MessageBox.Show("엑셀이 설치되지 않았습니다");
                    return;
                }
                Excel.Workbook wb = excelApp.Workbooks.Add(true);
                Excel._Worksheet workSheet = wb.Worksheets.get_Item(1) as Excel._Worksheet;
                workSheet.Name = "BTB";

                if (dgv.Rows.Count == 0)
                {
                    MessageBox.Show("출력할 데이터가 없습니다");
                    return;
                }

                // 헤더 출력
                for (int i = 0; i < dgv.Columns.Count ; i++)
                {
                    workSheet.Cells[1, i + 1] = dgv.Columns[i].HeaderText;
   
                }

                //내용 출력
                for (int r = 0; r < dgv.Rows.Count; r++)
                {
                    for (int i = 0; i < dgv.Columns.Count ; i++)
                    {
                        workSheet.Cells[r + 2, i + 1] = dgv.Rows[r].Cells[i].Value;
                        workSheet.Columns.AutoFit();
                        workSheet.Range["A:E"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; //오른쪽 정렬
                    }
                }

                wb.SaveAs(fileName, Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                wb.Close(Type.Missing, Type.Missing, Type.Missing);
                excelApp.Quit();
                releaseObject(excelApp);
                releaseObject(workSheet);
                releaseObject(wb);
            }
            catch (Exception ex)
            {
            }
        }

        #region 메모리해제
        private static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception e)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
        #endregion

     
    }
    
}
