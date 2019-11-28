using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using Super.Core.Infrastruct.Extensions;
using Super.Core.Infrastruct.Model;

namespace Super.Core.Infrastruct.Ado
{
    public class SqlServerExecute : ISqlExecute
    {
        readonly string _connStr;
        private SqlConnection _connection;

        public SqlServerExecute(string strConn)
        {
            this._connStr = strConn;
        }

        public SimpleResult ConnectTest(string strConn)
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
            var conn = SqlClientFactory.Instance.CreateConnection();
            conn.ConnectionString = strConn;
            conn.Open();
            return conn;
        }

        public int ExecuteNonQuery(string sql, params DbParameter[] dbParameters)
        {
            CheckConnect();

            SqlCommand command = new SqlCommand(sql, this._connection);
            AddParameters(command, dbParameters);
            return command.ExecuteNonQuery();
        }

        public T ExecuteScalar<T>(string sql, params DbParameter[] dbParameters)
        {
            CheckConnect();

            SqlCommand command = new SqlCommand(sql, this._connection);
            AddParameters(command, dbParameters);
            return (T)command.ExecuteScalar();
        }

        public DataTable Query(string sql, params DbParameter[] dbParameters)
        {
            CheckConnect();

            DataTable dt = new DataTable();
            SqlCommand command = new SqlCommand(sql, this._connection);
            AddParameters(command, dbParameters);
            SqlDataAdapter da = new SqlDataAdapter(command);
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

        public SimpleResult SqlQueryTest(string sql, params DbParameter[] dbParameters)
        {
            CheckConnect();

            SqlCommand command = new SqlCommand(sql, this._connection);
            AddParameters(command, dbParameters);
            SimpleResult result;

            try
            {
                command.ExecuteReader();
                result = new SimpleResult { Success = true };
            }
            catch (Exception e)
            {
                result = new SimpleResult { Success = false, Message = e.Message };
            }
            return result;
        }        

        public void Dispose()
        {
            if (this._connection.State == System.Data.ConnectionState.Open)
            {
                this._connection.Close();
            }
            this._connection.Dispose();
        }

        private void CheckConnect()
        {
            if (this._connection == null)
            {
                this._connection = (SqlConnection)CreateConnection(this._connStr);
            }
            else if (this._connection.State != ConnectionState.Open)
            {
                this._connection.Open();
            }
        }

        private void AddParameters(SqlCommand command, params DbParameter[] dbParameters)
        {
            if (dbParameters != null && dbParameters.Length > 0)
            {
                command.Parameters.AddRange(dbParameters);
            }
        }

        #region 一些因数据库不同而有不同处理的常用业务

        public bool IsDbExist(string dbName)
        {
            string sql = $" select * from master.dbo.sysdatabases where name='{dbName}'";
            var result= this.ExecuteScalar<object>(sql);
            return result != null;
        }

        public SimpleResult CreateDb(string dbName, string fileDirect = null)
        {
            string sql = "";
            if (string.IsNullOrEmpty(fileDirect))
            {
                sql = $"CREATE DATABASE [{dbName}]";
            }
            else
            {
                sql = $"CREATE DATABASE [{dbName}] " +
                $"ON PRIMARY(NAME=[{dbName}_data],FILENAME='{fileDirect}{dbName}.mdf')" +//,SIZE=5MB,MAXSIZE=UNLIMITED,FILEGROWTH=50MB
                $"log on(NAME=[{dbName}_log],FILENAME='{fileDirect}{dbName}.ldf')";//,SIZE=5MB,MAXSIZE=100GB,FILEGROWTH=10%
            }


            try
            {
                this.ExecuteNonQuery(sql);
                return new SimpleResult { Success = true };
            }
            catch(Exception e)
            {
                return new SimpleResult { Success = false, Message = e.Message };
            }

        }

        public Dictionary<string, List<ColumnInfo>> GetAllTables()
        {
            CheckConnect();

            string sql = "SELECT " +
                         "t.TABLE_NAME TableName," +
                         "t.COLUMN_NAME ColumnName," +
                         "t.DATA_TYPE DataType," +
                         "t.CHARACTER_MAXIMUM_LENGTH Length " +
                         "FROM INFORMATION_SCHEMA.COLUMNS t " +
                         "ORDER BY t.TABLE_NAME";

            var lstCol = this.QueryMany<ColumnInfo>(sql);

            var dic = new Dictionary<string, List<ColumnInfo>>();
            foreach (var col in lstCol)
            {
                if (!dic.ContainsKey(col.TableName))
                    dic[col.TableName] = new List<ColumnInfo>();
                dic[col.TableName].Add(col);
            }

            return dic;
        }

        public bool IsTableExist(string table)
        {
            string sql = $"select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME='[{table}]' ";
            var result = this.ExecuteScalar<object>(sql);
            return result != null;
        }

        #endregion
    }
}
