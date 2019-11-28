using Oracle.ManagedDataAccess.Client;
using Super.Core.Infrastruct.Extensions;
using Super.Core.Infrastruct.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Super.Core.Infrastruct.Ado
{
    public class OracleExecute : ISqlExecute
    {
        readonly string _connStr;
        private OracleConnection _connection;

        public OracleExecute(string strConn)
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
            var conn = OracleClientFactory.Instance.CreateConnection();
            conn.ConnectionString = strConn;
            conn.Open();
            return conn;
        }

        public DataTable Query(string sql, params DbParameter[] dbParameters)
        {
            CheckConnect();

            DataTable dt = new DataTable();
            OracleCommand command = new OracleCommand(sql, this._connection);
            AddParameters(command, dbParameters);
            OracleDataAdapter da = new OracleDataAdapter(command);
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

            OracleCommand command = new OracleCommand(sql, this._connection);
            AddParameters(command, dbParameters);
            return command.ExecuteNonQuery();
        }

        public T ExecuteScalar<T>(string sql, params DbParameter[] dbParameters)
        {
            CheckConnect();

            OracleCommand command = new OracleCommand(sql, this._connection);
            AddParameters(command, dbParameters);
            return (T)command.ExecuteScalar();
        }

        private void CheckConnect()
        {
            if (this._connection == null)
            {
                this._connection = (OracleConnection)CreateConnection(this._connStr);
            }
            else if (this._connection.State != ConnectionState.Open)
            {
                this._connection.Open();
            }
        }

        private void AddParameters(OracleCommand command, params DbParameter[] dbParameters)
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

            OracleCommand command = new OracleCommand(sql, this._connection);
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

        public Dictionary<string, List<ColumnInfo>> GetAllTables()
        {
            CheckConnect();

            string sql = "select " +
                "t.TABLE_NAME  as TableName," +
                "t.COLUMN_NAME as ColumnName," +
                "t.DATA_TYPE   as DataType," +
                "t.DATA_LENGTH as Length " +
                "from user_tab_columns t";


            var lstCol = this.QueryMany<ColumnInfo>(sql);

            var dic = new Dictionary<string, List<ColumnInfo>>();
            foreach (var col in lstCol)
            {
                if (!dic.ContainsKey(col.TableName)) dic[col.TableName] = new List<ColumnInfo>();
                dic[col.TableName].Add(col);
            }

            return dic;
        }

        public bool IsDbExist(string dbName)
        {
            throw new NotImplementedException();
        }

        public SimpleResult CreateDb(string dbName, string fileDirect = null)
        {
            throw new NotImplementedException();
        }

        public bool IsTableExist(string table)
        {
            string sql = $"select * from user_tables where table_name='{table}'";
            var result = this.ExecuteScalar<object>(sql);
            return result != null;
        }

        #endregion
    }
}
