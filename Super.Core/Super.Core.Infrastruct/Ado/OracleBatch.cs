using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super.Core.Infrastruct.Ado
{
    public class OracleBatch<T> : IBatch<T>
    {
        readonly string _tableName;
        readonly OracleConnection _conn;

        public OracleBatch(string tbName, string connStr)
        {
            this._tableName = tbName;
            this._conn = (OracleConnection)this.CreateConnection(connStr);
        }

        public OracleBatch(string tbName, DbConnection conn)
        {
            this._tableName = tbName;
            this._conn = (OracleConnection)conn;
        }


        public string TableName => this._tableName;


        public int BatchInsert(List<T> lstData)
        {
            if (lstData == null || lstData.Count < 1)
                return 0;

            //1.提取列表中每一列的数据
            var dic = this.List2Dic(lstData);

            //2.每一列生成OracleParameter
            var lstOracleParameter = new List<OracleParameter>();
            foreach (var kv in dic)
                lstOracleParameter.Add(this.GetOracleParameter(kv.Key, kv.Value));

            //3.生成插入语句，得到OracleCommand
            var lstParamName = from item in dic.Keys
                               select $":\"{item}\"";
            var cmdtxt = $"insert into {this.TableName} values({string.Join(",", lstParamName)})";
            var cmd = this.GetOracleCmd(this._conn, cmdtxt, lstData.Count, lstOracleParameter);

            //4.执行OracleCommand
            var sw = new Stopwatch();
            sw.Start();
            int num = cmd.ExecuteNonQuery();
            sw.Stop();
            long time = sw.ElapsedMilliseconds / 1000;

            System.Console.WriteLine($"表 {this.TableName},插入 {num} 条数据，耗时 {time} 秒");

            return num;
        }

        public async Task<int> BatchInsertAsync(List<T> lstData)
        {
            if (lstData == null || lstData.Count < 1)
                return 0;

            //1.提取列表中每一列的数据
            var dic = this.List2Dic(lstData);

            //2.每一列生成OracleParameter
            var lstOracleParameter = new List<OracleParameter>();
            foreach (var kv in dic)
                lstOracleParameter.Add(this.GetOracleParameter(kv.Key, kv.Value));

            //3.生成插入语句，得到OracleCommand
            var lstParamName = from item in dic.Keys
                               select $":{item}";
            var cmdtxt = $"insert into {this.TableName} values({string.Join(",", lstParamName)})";
            var cmd = this.GetOracleCmd(this._conn, cmdtxt, lstData.Count, lstOracleParameter);

            //4.执行OracleCommand
            var sw = new Stopwatch();
            sw.Start();
            int num = await cmd.ExecuteNonQueryAsync();
            sw.Stop();
            long time = sw.ElapsedMilliseconds / 1000;

            System.Console.WriteLine($"表 {this.TableName},插入 {num} 条数据，耗时 {time} 秒");

            return num;
        }

        private DbConnection CreateConnection(string strConn)
        {
            var conn = OracleClientFactory.Instance.CreateConnection();
            conn.ConnectionString = strConn;
            conn.Open();
            return conn;
        }

        private Dictionary<string, List<object>> List2Dic(List<T> lstData)
        {
            var dic = new Dictionary<string, List<object>>();
            var props = typeof(T).GetProperties();

            foreach (var p in props)
            {
                var lst = new List<object>();
                foreach (var d in lstData)
                {
                    var val = p.GetValue(d);
                    lst.Add(val);
                }
                dic[p.Name] = lst;
            }
            return dic;
        }

        private OracleParameter GetOracleParameter(string fieldName, List<object> lst)
        {
            OracleParameter parameter = new OracleParameter();
            parameter.ParameterName = fieldName;
            parameter.Direction = ParameterDirection.Input;

            Type tp = lst[0].GetType();
            if (tp == typeof(int))
            {
                int[] arry = new int[lst.Count];
                for (int i = 0; i < lst.Count; i++)
                    arry[i] = int.Parse(lst[i].ToString());
                //parameter.OracleDbType = OracleDbType.Int32;
                parameter.Value = arry;
            }
            else if (tp == typeof(double))
            {
                double[] arry = new double[lst.Count];
                for (int i = 0; i < lst.Count; i++)
                    arry[i] = double.Parse(lst[i].ToString());
                parameter.Value = arry;
            }
            else if (tp == typeof(string))
            {
                string[] arry = new string[lst.Count];
                for (int i = 0; i < lst.Count; i++)
                    arry[i] = lst[i].ToString();
                parameter.Value = arry;
            }
            else if (tp == typeof(DateTime))
            {
                DateTime[] arry = new DateTime[lst.Count];
                for (int i = 0; i < lst.Count; i++)
                    arry[i] = Convert.ToDateTime(lst[i]);
                parameter.Value = arry;
            }
            else if (tp == typeof(bool))
            {
                bool[] arry = new bool[lst.Count];
                for (int i = 0; i < lst.Count; i++)
                    arry[i] = Convert.ToBoolean(lst[i]);
                parameter.Value = arry;
            }
            else if (tp.IsEnum)
            {
                int[] arry = new int[lst.Count];
                for (int i = 0; i < lst.Count; i++)
                    arry[i] = int.Parse(lst[i].ToString());
                parameter.Value = arry;
            }
            else
            {
                throw new Exception("the data type is not valid !");
            }
            return parameter;
        }

        private OracleCommand GetOracleCmd(OracleConnection conn, string cmd, int num, List<OracleParameter> parameters)
        {
            OracleCommand command = new OracleCommand();
            command.Connection = conn;
            command.CommandText = cmd;
            command.ArrayBindCount = num;
            foreach (var p in parameters)
                command.Parameters.Add(p);

            return command;
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
