using MySql.Data.MySqlClient;
using Super.Core.Infrastruct.Extensions;
using Super.Core.Infrastruct.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace Super.Core.Infrastruct.Ado
{
    public class MySqlExecute : ISqlExecute
    {
        readonly string _connStr;
        private MySqlConnection _connection;

        public MySqlExecute(string strConn)
        {
            this._connStr = strConn;
        }

        public SimpleResult ConnectTest(string strConn = null)
        {
            if (string.IsNullOrEmpty(strConn)) strConn = this._connStr;

            SimpleResult result;
            try
            {
                this.CreateConnection(strConn);
                result = new SimpleResult { Success = true };
            }
            catch (Exception e)
            {
                result = new SimpleResult { Success = false, Message = e.Message };
            }
            return result;
        }

        public DbConnection CreateConnection(string strConn)
        {
            var conn = MySqlClientFactory.Instance.CreateConnection();
            conn.ConnectionString = strConn;
            conn.Open();
            return conn;
        }

        public DataTable Query(string sql, params DbParameter[] dbParameters)
        {
            CheckConnect();

            DataTable dt = new DataTable();
            MySqlCommand command = new MySqlCommand(sql, this._connection);
            AddParameters(command, dbParameters);
            MySqlDataAdapter da = new MySqlDataAdapter(command);
            da.Fill(dt);
            return dt;
        }

        public IEnumerable<T> QueryMany<T>(string sql, params DbParameter[] dbParameters)
        {
            CheckConnect();

            var dt = Query(sql, dbParameters);
            var lst = dt.TableToList<T>();
            return lst;
        }

        public T QuerySingle<T>(string sql, params DbParameter[] dbParameters)
        {
            CheckConnect();

            var dt = Query(sql, dbParameters);
            var lst = dt.TableToList<T>();

            if (lst == null || lst.Count == 0)
            {
                return default(T);
            }
            else
            {
                return lst[0];
            }
        }

        public int ExecuteNonQuery(string sql, params DbParameter[] dbParameters)
        {
            CheckConnect();

            MySqlCommand command = new MySqlCommand(sql, this._connection);
            AddParameters(command, dbParameters);
            return command.ExecuteNonQuery();
        }

        public T ExecuteScalar<T>(string sql, params DbParameter[] dbParameters)
        {
            CheckConnect();

            MySqlCommand command = new MySqlCommand(sql, this._connection);
            AddParameters(command, dbParameters);
            return (T)command.ExecuteScalar();
        }

        private void CheckConnect()
        {
            if (this._connection == null)
            {
                this._connection = (MySqlConnection)CreateConnection(this._connStr);
            }
            else if (this._connection.State != ConnectionState.Open)
            {
                this._connection.Open();
            }
        }

        private void AddParameters(MySqlCommand command, params DbParameter[] dbParameters)
        {
            if (dbParameters != null && dbParameters.Length > 0)
            {
                command.Parameters.AddRange(dbParameters);
            }
        }

        public void Dispose()
        {
            if (this._connection.State == System.Data.ConnectionState.Open)
            {
                this._connection.Close();
            }
            this._connection.Dispose();
        }

        public SimpleResult SqlQueryTest(string sql, params DbParameter[] dbParameters)
        {
            CheckConnect();

            MySqlCommand command = new MySqlCommand(sql, this._connection);
            AddParameters(command, dbParameters);
            SimpleResult result;

            try
            {
                command.ExecuteReader();
                //var obj= command.ExecuteScalar();
                result = new SimpleResult { Success = true };
            }
            catch (Exception e)
            {
                result = new SimpleResult { Success = false, Message = e.Message };
            }
            return result;
        }        

        #region 一些因数据库不同而有不同处理的常用业务

        public bool IsDbExist(string dbName)
        {
            string sql = $"select * from information_schema.SCHEMATA where SCHEMA_NAME='{dbName}'";
            var result = this.ExecuteScalar<object>(sql);
            return result != null;
        }

        public SimpleResult CreateDb(string dbName, string fileDirect = null)
        {

            string sql = $"CREATE DATABASE {dbName}";

            try
            {
                this.ExecuteNonQuery(sql);
                return new SimpleResult { Success = true };
            }
            catch (Exception e)
            {
                return new SimpleResult { Success = false, Message = e.Message };
            }

        }

        public Dictionary<string, List<ColumnInfo>> GetAllTables()
        {
            CheckConnect();

            string sql =
                "SELECT " +
                "t.TABLE_NAME AS TableName," +
                "t.COLUMN_NAME AS ColumnName," +
                "t.COLUMN_TYPE AS DataType," +
                "t.CHARACTER_MAXIMUM_LENGTH AS Length " +
                "FROM " +
                "information_schema.COLUMNS t " +
                "WHERE " +
                $"table_schema = '{this._connection.Database}'";

            var lstCol = this.QueryMany<ColumnInfo>(sql);

            var dic = new Dictionary<string, List<ColumnInfo>>();
            foreach (var col in lstCol)
            {
                if (!dic.ContainsKey(col.TableName)) dic[col.TableName] = new List<ColumnInfo>();
                dic[col.TableName].Add(col);
            }

            return dic;
        }

        public bool IsTableExist(string table)
        {
            string sql = $"select * from information_schema.tables WHERE TABLE_NAME='{table}'";
            var result = this.ExecuteScalar<object>(sql);
            return result != null;
        }

        #endregion

    }
}
