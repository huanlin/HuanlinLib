using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace InfoShare.Data.Helpers
{
    public static class DbHelper
    {
        public static object ExecuteScalar(IDbConnection cn, string sql)
        {
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            IDbCommand cmd = cn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            return cmd.ExecuteScalar();
        }

        public static int ExecuteNonQuery(IDbConnection cn, string sql)
        {
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            IDbCommand cmd = cn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            return cmd.ExecuteNonQuery();
        }

        public static SqlConnection CreateSqlConnection(string cnstr)
        {
            SqlConnection cn = new SqlConnection(cnstr);
            return cn;
        }

        public static void CloseConnection(IDbConnection cn)
        {
            if (cn == null)
            {
                return;
            }
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
        }
    }
}