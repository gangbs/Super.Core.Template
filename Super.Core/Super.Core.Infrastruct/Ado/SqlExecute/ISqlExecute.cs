﻿using Super.Core.Infrastruct.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Super.Core.Infrastruct.Ado
{
    public interface ISqlExecute : IDisposable
    {
        SimpleResult ConnectTest(string strConn);

        DbConnection CreateConnection(string strConn);

        SimpleResult SqlQueryTest(string sql, params DbParameter[] dbParameters);

        DataTable Query(string sql, params DbParameter[] dbParameters);

        IEnumerable<T> QueryMany<T>(string sql, params DbParameter[] dbParameters);

        T QuerySingle<T>(string sql, params DbParameter[] dbParameters);

        T ExecuteScalar<T>(string sql, params DbParameter[] dbParameters);

        int ExecuteNonQuery(string sql, params DbParameter[] dbParameters);

        Dictionary<string, List<ColumnInfo>> GetAllTables();

        bool IsDbExist(string dbName);

        bool IsTableExist(string table);

        SimpleResult CreateDb(string dbName, string fileDirect = null);

    }

    public class ColumnInfo
    {
        public string ColumnName { get; set; }
        public string TableName { get; set; }
        public string DataType { get; set; }
        public string Length { get; set; }
    }
}
