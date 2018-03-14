/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2018 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using Dev2.Common.Interfaces.Services.Sql;
using Dev2.Services.Sql;
using Warewolf.Storage.Interfaces;
using System.Data;
using WarewolfParserInterop;

namespace Dev2.Activities.AdvancedRecordset
{
	public class AdvancedRecordset : IAdvancedRecordset
	{
		readonly SqliteServer dbManager = new SqliteServer("Data Source=:memory:");
		public IExecutionEnvironment Environment { get; set; }
		public AdvancedRecordset(IExecutionEnvironment env)
		{
			Environment = env;
		}
		public DataSet ReturnDataSet(string sql)
		{
			try
			{
				var command = dbManager.CreateCommand();
				command.CommandText = sql;
				command.CommandType = CommandType.Text;
				var ds = dbManager.FetchDataSet(command);
				return ds;
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}
		public DataTable DataTable(string sql)
		{
			try
			{
				var command = dbManager.CreateCommand();
				command.CommandText = sql;
				command.CommandType = CommandType.Text;
				var dt = dbManager.FetchDataTable(command);
				return dt;
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}
		public DataSet ExecuteQuery(string sql)
		{
			try
			{
				var command = dbManager.CreateCommand();
				command.CommandText = sql;
				command.CommandType = CommandType.Text;
				var ds = dbManager.FetchDataSet(command);
				return ds;
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}
		public string ExecuteScalar(string sql)
		{
			try
			{
				var command = dbManager.CreateCommand();
				command.CommandText = sql;
				command.CommandType = CommandType.Text;
				var dt = dbManager.ExecuteScalar(command);
				return dt.ToString();
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}
		public int ExecuteNonQuery(string sql)
		{
			try
			{
				var command = dbManager.CreateCommand();
				command.CommandText = sql;
				command.CommandType = CommandType.Text;
				var recordsAffected = dbManager.ExecuteNonQuery(command);
				return recordsAffected;
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}
		public void LoadRecordsetAsTable(string recordsetName)
		{
			var table = Environment.EvalAsTable("[[" + recordsetName + "(*)]]", 0);
			LoadIntoSqlite(recordsetName, table);
		}
		void LoadIntoSqlite(string recordsetName, IEnumerable<Tuple<string, DataStorage.WarewolfAtom>[]> tableData)
		{
			var enumerator = tableData.GetEnumerator();
			if (enumerator.MoveNext())
			{
				var sql = "DROP TABLE IF EXISTS " + recordsetName;
				var com = dbManager.CreateCommand();
				com.CommandText = sql;
				com.CommandType = CommandType.Text;
				using (var command = com)
				{
					command.ExecuteNonQuery();

					sql = "CREATE TABLE IF NOT EXISTS " + recordsetName + "([Primary_Id] INTEGER NOT NULL, CONSTRAINT[PK_" + recordsetName + "] PRIMARY KEY([Primary_Id]))";
					command.CommandText = sql;
					command.CommandType = CommandType.Text;
					dbManager.ExecuteNonQuery(command);
					int i = 0;

					do
					{
						var insertSql = "INSERT INTO " + recordsetName + " select " + i + ",";
						foreach (var (key, value) in enumerator.Current)
						{
							var colType = value.GetType().Name;
							if (colType == "Int")
							{
								colType = "INTEGER";
								insertSql += value + ",";
							}
							else if (colType == "DataString")
							{
								colType = "TEXT";
								insertSql += "'" + value + "',";
							}
							else
							{
								insertSql += value + ",";
							}
							if (i == 0)
							{
								sql = "ALTER TABLE  " + recordsetName + " ADD COLUMN " + key + " " + colType + ";";
								command.CommandText = sql;
								command.ExecuteNonQuery();
							}
						}
						insertSql = insertSql.Remove(insertSql.Length - 1);
						command.CommandText = insertSql;
						command.ExecuteNonQuery();
						i++;
					} while (enumerator.MoveNext());
				}
			}
		}
		public void ApplyResultToEnvironment(string returnRecordsetName, List<DataRow> recordset)
		{
			var l = new List<AssignValue>();
			foreach (DataRow dr in recordset)
			{
				foreach (DataColumn dc in dr.Table.Columns)
				{
					l.Add(new AssignValue("[[" + returnRecordsetName + "()." + dc.ColumnName.ToString() + "]]", dr[dc].ToString()));
				}
			}
			Environment.AssignWithFrame(l, 0);
			Environment.CommitAssign();
		}
		public void Dispose()
		{
			dbManager.Dispose();
		}
	}
}
