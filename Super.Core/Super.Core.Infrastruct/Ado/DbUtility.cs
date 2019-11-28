using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace Super.Core.Infrastruct.Ado
{
    public class DbConnectionStringModel
    {
        public DatabaseType DbType { get; set; }
        public string Server { get; set; }
        public int? Port { get; set; }

        public string DbName { get; set; }
        /// <summary>
        /// oracle专用
        /// </summary>
        public string ServerName { get; set; }

        public string UserId { get; set; }

        public string Pwd { get; set; }

        public string CreateConnectionString()
        {
            string str = "";
            switch (this.DbType)
            {
                case DatabaseType.sqlserver:
                    str = CreateSqlConnectionString(this);
                    break;
                case DatabaseType.mysql:
                    str = CreateMysqlConnectionString(this);
                    break;
                case DatabaseType.oracle:
                    str = CreateOracleConnectingString(this);
                    break;
            }
            return str;
        }


        public static string CreateSqlConnectionString(DbConnectionStringModel model)
        {
            var builder = new SqlConnectionStringBuilder();

            if (!model.Port.HasValue || model.Port == 0)//sqlserver默认端口为1433
                builder.DataSource = $"{model.Server}";
            else
                builder.DataSource = $"{model.Server},{model.Port}";

            if (!string.IsNullOrEmpty(model.DbName))
                builder.InitialCatalog = model.DbName;

            builder.UserID = model.UserId;
            builder.Password = model.Pwd;

            //builder.ConnectTimeout = 15;//15s未连通则认为连不通
            //builder.Pooling = true;            //是否汇入连接池
            //builder.MinPoolSize = 1;          //最小连接池连接数
            //builder.MaxPoolSize = 20;          //最大连接数
            //builder.IntegratedSecurity = false;     //false:用户名密码验证      true：windows身份验证
            //builder.MultipleActiveResultSets = true;      //是否允许保留多活动结果集 (MARS)

            return builder.ConnectionString;
        }

        public static string CreateMysqlConnectionString(DbConnectionStringModel model)
        {
            if (!model.Port.HasValue) model.Port = 3306;
            var builder = new MySqlConnectionStringBuilder();
            builder.Server = model.Server;
            builder.Port = (uint)model.Port;

            if (!string.IsNullOrEmpty(model.DbName))
                builder.Database = model.DbName;

            builder.UserID = model.UserId;
            builder.Password = model.Pwd;

            //builder.ConnectionTimeout = 15;
            //builder.Pooling = true;
            //builder.MinimumPoolSize = 1;
            //builder.MaximumPoolSize = 20;
            //builder.IntegratedSecurity = true;

            return builder.ConnectionString;
        }

        public static string CreateOracleConnectingString(DbConnectionStringModel model)
        {
            if (!model.Port.HasValue) model.Port = 1521;
            var builder = new OracleConnectionStringBuilder();
            builder.DataSource = $"{model.Server}/{model.ServerName}:{model.Port}";
            builder.UserID = model.UserId;
            builder.Password = model.Pwd;

            //builder.ConnectionTimeout = 15;
            //builder.Pooling = true;
            //builder.MinPoolSize = 1;
            //builder.MaxPoolSize = 20;

            return builder.ConnectionString;
        }


    }


    public class DbUtility
    {
        public static DbConnection CreateConnection(DbProviderFactory factory, string strConn)
        {
            DbConnection conn = factory.CreateConnection();
            conn.ConnectionString = strConn;
            return conn;
        }
    }
}
