using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;
using MySql.Data.MySqlClient;

namespace StrategyManager.Common
{
    public static class MySQLHelper
    {
        private static MySqlConnection conn;
        static MySQLHelper()
        {
            if (conn == null)
            {
                conn = new MySqlConnection();
                conn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings["mysqlstr"];
            }
        }
        
        /// <summary>
        /// 查询返回DataSet
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public static DataSet GetDataSet(string sql, MySqlParameter[] parameters = null)
        {
            try
            {
                DataSet ds = new DataSet();
                MySqlCommand sqlCmd = new MySqlCommand();
                sqlCmd.CommandText = sql;
                if (parameters != null && parameters.Length > 0)
                {
                    sqlCmd.Parameters.AddRange(parameters);
                }
                sqlCmd.Connection = conn;
                MySqlDataAdapter da = new MySqlDataAdapter(sqlCmd);
                da.Fill(ds);

                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 查询返回DataTable
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql, MySqlParameter[] parameters = null)
        {
            try
            {
                DataSet ds = GetDataSet(sql, parameters);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public static bool ExecSql(string sql, MySqlParameter[] parameters)
        {
            try
            {
                conn.Open();
                MySqlCommand sqlCmd = new MySqlCommand();
                sqlCmd.CommandText = sql;
                if (parameters != null && parameters.Length>0)
                {
                    sqlCmd.Parameters.AddRange(parameters);
                }

                sqlCmd.Connection = conn;
                if (sqlCmd.ExecuteNonQuery() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 以事务方式执行SQL语句
        /// </summary>
        /// <param name="listsqls">SQL语句list</param>
        /// <param name="listparameters">参数列表list</param>
        /// <returns>成功返回true 否则返回false</returns>
        public static bool ExecSqlByTran(List<string> listsqls, List<MySqlParameter[]> listparameters)
        {
            if (listsqls == null || listparameters == null || listsqls.Count <= 0 || listparameters.Count <= 0 || listsqls.Count != listparameters.Count)
            {
                return false;
            }
            else
            {

                conn.Open();
                MySqlTransaction tran = conn.BeginTransaction();
                try
                {
                    for (int i = 0; i < listsqls.Count; i++)
                    {
                        MySqlCommand sqlCmd = new MySqlCommand();
                        sqlCmd.CommandText = listsqls[i];
                        sqlCmd.Parameters.AddRange(listparameters[i]);
                        sqlCmd.Connection = conn;
                        sqlCmd.Transaction = tran;
                        sqlCmd.ExecuteNonQuery();
                    }
                    tran.Commit();
                    conn.Close();
                    return true;
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    conn.Close();
                    return false;
                }
                
            }
        }

    }
}
