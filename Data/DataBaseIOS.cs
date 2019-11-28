using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using ExpressBase.Mobile.Data;
using ExpressBase.Mobile.iOS.Data;
using Foundation;
using Mono.Data.Sqlite;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(DataBaseIOS))]
namespace ExpressBase.Mobile.iOS.Data
{
    public class DataBaseIOS : IDataBase
    {
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
                    App.DbPath = dbPath;
                    return 1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return 0;
        }

        public void DoQuery(string query, params DbParameter[] parameters)
        {
            EbDataTable dt = new EbDataTable();
            try
            {
                using (SqliteConnection con = new SqliteConnection("Data Source=" + App.DbPath))
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
        }

        public void DoQueries(string query, params DbParameter[] parameters)
        {

        }

        public int DoNonQuery(string query, params DbParameter[] parameters)
        {
            try
            {
                using (SqliteConnection con = new SqliteConnection("Data Source=" + App.DbPath))
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

        protected void PrepareDataTable(SqliteDataReader reader, EbDataTable dt)
        {
            int _fieldCount = reader.FieldCount;

            DataTable _t = reader.GetSchemaTable();

            for (int i = 0; i < _t.Columns.Count; i++)
            {
                dt.Columns.Add(new EbDataColumn
                {
                    ColumnName = _t.Columns[i].ColumnName,
                    ColumnIndex = i
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