using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Threading.Tasks;
namespace Microsoft_OleDb
{
    public class Microsoft_OleDb
    {
        // public static string DBConnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source = C:\\Logs\\BTB.accdb";
        public static string DBConnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source = C:\\KSM\\DiaDetector\\Data\\BTB.accdb";

        public static DataSet GetDataRead(string sql)  // 읽기
        {
            DataSet DS = null;
            OleDbConnection connection = null;
            OleDbDataAdapter da = null;
            try
            {
                using (connection = new OleDbConnection(DBConnString))
                {
                    da = new OleDbDataAdapter();
                    da.SelectCommand = new OleDbCommand(sql, connection);
                    DS = new DataSet();
                    da.Fill(DS);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (da != null)
                    da.Dispose();
                if (connection != null)
                    connection.Close();
            }
            return (DS);
        }
        public static DataSet GetDataReads(string sql)  // 읽기
        {
            DataSet DS = null;
            OleDbConnection connection = null;
            OleDbDataAdapter da = null;
            try
            {
                using (connection = new OleDbConnection(DBConnString))
                {
                    da = new OleDbDataAdapter();
                    da.SelectCommand = new OleDbCommand(sql, connection);
                    DS = new DataSet();
                    da.Fill(DS);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (da != null)
                    da.Dispose();
                if (connection != null)
                    connection.Close();
            }
            return (DS);


        }
        public static DataSet GetDataTime(string sql)  // 읽기
        {

            DataSet DS = null;
            OleDbConnection connection = null;
            OleDbDataAdapter da = null;
            try
            {
                using (connection = new OleDbConnection(DBConnString))
                {
                    da = new OleDbDataAdapter();
                    da.SelectCommand = new OleDbCommand(sql, connection);
                    DS = new DataSet();
                    da.Fill(DS);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (da != null)
                    da.Dispose();
                if (connection != null)
                    connection.Close();
            }
            return (DS);

        }
        public static DataSet GetDataReadFind(string sql)  // 읽기
        {
            DataSet DS = null;
            OleDbConnection connection = null;
            OleDbDataAdapter da = null;
            try
            {
                using (connection = new OleDbConnection(DBConnString))
                {
                    da = new OleDbDataAdapter();
                    da.SelectCommand = new OleDbCommand(sql, connection);
                    DS = new DataSet();
                    da.Fill(DS);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (da != null)
                    da.Dispose();
                if (connection != null)
                    connection.Close();
            }
            return (DS);

        }
        public static DataSet GetDataNumRead(string datetime, string medel, string sql)  //모델 넘버 읽어보기
        {

            DataSet DS = null;
            OleDbConnection connection = null;
            OleDbDataAdapter da = null;
            try
            {
                using (connection = new OleDbConnection(DBConnString))
                {
                    da = new OleDbDataAdapter();
                    da.SelectCommand = new OleDbCommand(sql, connection);
                    DS = new DataSet();
                    da.Fill(DS);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (da != null)
                    da.Dispose();
                if (connection != null)
                    connection.Close();
            }
            return (DS);
        }
        public static DataSet GetDataNumWhite(string sql)  //생산수량업데이트쓰기
        {
            //   string sql;
            //if (stic == true)
            //{
            //     sql = "UPDATE Table1 SET Numbers= Numbers +1 WHERE Production = '" + datetime + "'   AND Model ='" + medel + "'";
            //}
            //else
            //{
            //    sql = "INSERT INTO Table1(Production,Model,Numbers,times) Values( '" + datetime + "' , '" + medel + "','0','0시0분0초')";

            //}

            DataSet DS = null;
            OleDbConnection connection = null;
            OleDbDataAdapter da = null;
            try
            {
                using (connection = new OleDbConnection(DBConnString))
                {
                    da = new OleDbDataAdapter();
                    da.SelectCommand = new OleDbCommand(sql, connection);
                    DS = new DataSet();
                    da.Fill(DS);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (da != null)
                    da.Dispose();
                if (connection != null)
                    connection.Close();
            }
            return (DS);

        }
        public static DataSet GetDataTimer(string sql)  //생산시간업데이트
        {

            //  string sql1 = "UPDATE Table1 SET times= '" + MakeOutTimeSt + "' WHERE Production = '" + datetime + "'   AND Model ='" + model + "'";
            DataSet DS = null;
            OleDbConnection connection = null;
            OleDbDataAdapter da = null;
            try
            {
                using (connection = new OleDbConnection(DBConnString))
                {
                    da = new OleDbDataAdapter();
                    da.SelectCommand = new OleDbCommand(sql, connection);
                    DS = new DataSet();
                    da.Fill(DS);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (da != null)
                    da.Dispose();
                if (connection != null)
                    connection.Close();
            }
            return (DS);
        }



    }
}
