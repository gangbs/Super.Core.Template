using Super.Core.Infrastruct.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super.Core.Infrastruct.Ado
{
    public class SqlServerBatch<T> : IBatch<T>
    {
        readonly string _tableName;
        readonly SqlConnection _conn;

        public SqlServerBatch(string tbName, string connStr)
        {
            this._tableName = tbName;
            this._conn = (SqlConnection)this.CreateConnection(connStr);
        }

        public SqlServerBatch(string tbName, DbConnection conn)
        {
            this._tableName = tbName;
            this._conn = (SqlConnection)conn;
        }

        public string TableName => this._tableName;

        private DbConnection CreateConnection(string strConn)
        {
            var conn = SqlClientFactory.Instance.CreateConnection();
            conn.ConnectionString = strConn;
            conn.Open();
            return conn;
        }

        public int BatchInsert(List<T> lstData)
        {
            var dt = lstData.ListToTable();
            using (SqlBulkCopy sbc = new SqlBulkCopy(this._conn))
            {
                sbc.BatchSize = dt.Rows.Count;
                sbc.BulkCopyTimeout = 1000;
                sbc.DestinationTableName = this._tableName;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sbc.ColumnMappings.Add(dt.Columns[i].ColumnName, i);
                }
                //全部写入数据库
                sbc.WriteToServer(dt);
            }
            return lstData.Count();
        }

        public async Task<int> BatchInsertAsync(List<T> lstData)
        {
            var dt = lstData.ListToTable();
            using (SqlBulkCopy sbc = new SqlBulkCopy(this._conn))
            {
                sbc.BatchSize = dt.Rows.Count;
                sbc.BulkCopyTimeout = 1000;
                sbc.DestinationTableName = this._tableName;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sbc.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                }
                //全部写入数据库
                await sbc.WriteToServerAsync(dt);
            }

            return lstData.Count;
        }

        public void Dispose()
        {
            if (this._conn.State == System.Data.ConnectionState.Open)
            {
                this._conn.Close();
            }
            this._conn.Dispose();
        }
    }
}
