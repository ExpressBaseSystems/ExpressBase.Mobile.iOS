using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using ExpressBase.Mobile.Data;
using ExpressBase.Mobile.Helpers;
using ExpressBase.Mobile.iOS.Data;
using ExpressBase.Mobile.Models;
using Foundation;
using Mono.Data.Sqlite;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(DataBaseIOS))]
namespace ExpressBase.Mobile.iOS.Data
{
    public class DataBaseIOS : IDataBase
    {
        public string DbPath { get; set; }

        public int CreateDB(string sid)
        {
            try
            {
                if (string.IsNullOrEmpty(sid))
                    return 0;

                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), string.Format("{0}.db3", sid));

                if (!File.Exists(dbPath))
                {
                    Mono.Data.Sqlite.SqliteConnection.CreateFile(dbPath);
                    this.DbPath = dbPath;
                    return 1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return 0;
        }

        public void SetDbPath(string sid)
        {
            try
            {
                if (string.IsNullOrEmpty(sid))
                    return;

                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), string.Format("{0}.db3", sid));
                if (!File.Exists(dbPath))
                    Mono.Data.Sqlite.SqliteConnection.CreateFile(dbPath);

                this.DbPath = dbPath;
            }
            catch (Exception ex)
            {
                EbLog.Error(ex.Message);
            }
        }

        public EbDataTable DoQuery(string query, params DbParameter[] parameters)
        {
            EbDataTable dt = new EbDataTable();
            try
            {
                using (SqliteConnection con = new SqliteConnection("Data Source=" + this.DbPath))
                {
                    con.Open();
                    using (SqliteCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = query;

                        if (parameters != null && parameters.Length > 0)
                            cmd.Parameters.AddRange(this.DbParamToSqlParam(parameters));

                        using (var reader = cmd.ExecuteReader())
                        {
                            PrepareDataTable(reader, dt);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dt;
        }

        public EbDataSet DoQueries(string query, params DbParameter[] parameters)
        {
            EbDataSet ds = new EbDataSet();
            try
            {
                using (SqliteConnection con = new SqliteConnection("Data Source=" + this.DbPath))
                {
                    con.Open();
                    using (SqliteCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = query;

                        if (parameters != null && parameters.Length > 0)
                            cmd.Parameters.AddRange(this.DbParamToSqlParam(parameters));

                        using (var reader = cmd.ExecuteReader())
                        {
                            do
                            {
                                EbDataTable dt = new EbDataTable();
                                PrepareDataTable(reader, dt);
                                ds.Tables.Add(dt);
                                ds.RowNumbers += dt.Rows.Count.ToString() + ",";
                            }
                            while (reader.NextResult());
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                EbLog.Error("DataBaseAndroid.DoQueries---" + ex.Message);
            }
            return ds;
        }

        public int DoNonQuery(string query, params DbParameter[] parameters)
        {
            try
            {
                using (SqliteConnection con = new SqliteConnection("Data Source=" + this.DbPath))
                {
                    con.Open();
                    using (SqliteCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = query;

                        if (parameters != null && parameters.Length > 0)
                            cmd.Parameters.AddRange(this.DbParamToSqlParam(parameters));

                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return 0;
        }

        public void DoNonQueryBatch(EbDataTable Table)
        {
            try
            {
                using (SqliteConnection con = new SqliteConnection("Data Source=" + this.DbPath))
                {
                    con.Open();
                    string query = "INSERT INTO {0} ({1}) VALUES ({2});";
                    List<string> _cols = new List<string>();
                    List<string> _vals = new List<string>();

                    using (SqliteCommand cmd = con.CreateCommand())
                    {
                        using (SqliteTransaction transaction = con.BeginTransaction())
                        {
                            for (int k = 0; k < Table.Rows.Count; k++)
                            {
                                cmd.Parameters.Clear();

                                if (k >= 1000)
                                    break;

                                for (int i = 0; i < Table.Rows[k].Count; i++)
                                {
                                    EbDataColumn column = Table.Columns.Find(item => item.ColumnIndex == i);
                                    if (k == 0)
                                    {
                                        _cols.Add(column.ColumnName);
                                        _vals.Add("@" + column.ColumnName);
                                    }

                                    cmd.Parameters.Add(new SqliteParameter { ParameterName = "@" + column.ColumnName, Value = Table.Rows[k][i] });
                                }

                                cmd.CommandText = string.Format(query, Table.TableName, string.Join(",", _cols.ToArray()), string.Join(",", _vals.ToArray()));
                                int rowAffected = cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public object DoScalar(string query, params DbParameter[] parameters)
        {
            try
            {
                using (SqliteConnection con = new SqliteConnection("Data Source=" + this.DbPath))
                {
                    con.Open();
                    using (SqliteCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = query;

                        if (parameters != null && parameters.Length > 0)
                            cmd.Parameters.AddRange(this.DbParamToSqlParam(parameters));

                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        protected void PrepareDataTable(SqliteDataReader reader, EbDataTable dt)
        {
            int _fieldCount = reader.FieldCount;

            for (int i = 0; i < _fieldCount; i++)
            {
                dt.Columns.Add(new EbDataColumn
                {
                    ColumnName = reader.GetName(i),
                    ColumnIndex = i,
                    Type = DbTypeConverter.ConvertToDbType(reader.GetFieldType(i))
                });
            }

            while (reader.Read())
            {
                EbDataRow dr = dt.NewDataRow();
                object[] oArray = new object[_fieldCount];
                reader.GetValues(oArray);
                dr.AddRange(oArray);
                dt.Rows.Add(dr);
            }
        }

        private SqliteParameter[] DbParamToSqlParam(params DbParameter[] parameters)
        {
            List<SqliteParameter> SqlP = new List<SqliteParameter>();

            foreach (DbParameter param in parameters)
            {
                SqlP.Add(new SqliteParameter
                {
                    ParameterName = param.ParameterName,
                    DbType = (DbType)param.DbType,
                    Value = param.Value
                });
            }
            return SqlP.ToArray();
        }
    }
}