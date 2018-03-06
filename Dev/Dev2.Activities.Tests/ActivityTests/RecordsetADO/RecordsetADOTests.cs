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
using System.Collections;
using System.Collections.Generic;
using Dev2.Activities;
using Dev2.Common.Interfaces.Diagnostics.Debug;
using Dev2.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Warewolf.Storage;
using Warewolf.Storage.Interfaces;
using WarewolfParserInterop;

namespace Dev2.Tests.Activities.ActivityTests
{
	[TestClass]
	public class RecordsetADOTests : BaseActivityTests
	{
		IExecutionEnvironment env;
		public AdvancedRecordsetWorker CreatePersonAddressWorkers()
		{
			var personRecordsetName = "person";
			var addressRecordsetName = "address";
			var env = new ExecutionEnvironment();
			/*
			Person
			| Name | Age | address_id |
			Address
			| id | Addr | Postcode |

			| bob | 21 | 1 |
			| sue | 22 | 2 | # unique address
			| jef | 24 | 1 | # matching address
			| zak | 19 | 9 | # fail finding address

			| 1 | 11 test lane | 3421 |
			| 2 | 16 test lane | 3422 |
			 * */

			var l = new List<AssignValue>();
			l.Add(new AssignValue("[[person().name]]", "bob"));
			l.Add(new AssignValue("[[person().age]]", "21"));
			l.Add(new AssignValue("[[person().address_id]]", "1"));

			l.Add(new AssignValue("[[person().name]]", "sue"));
			l.Add(new AssignValue("[[person().age]]", "22"));
			l.Add(new AssignValue("[[person().address_id]]", "2"));

			l.Add(new AssignValue("[[person().name]]", "jef"));
			l.Add(new AssignValue("[[person().age]]", "24"));
			l.Add(new AssignValue("[[person().address_id]]", "1"));

			l.Add(new AssignValue("[[person().name]]", "zak"));
			l.Add(new AssignValue("[[person().age]]", "19"));
			l.Add(new AssignValue("[[person().address_id]]", "9"));



			l.Add(new AssignValue("[[address().id]]", "1"));
			l.Add(new AssignValue("[[address().addr]]", "11 test lane"));
			l.Add(new AssignValue("[[address().postcode]]", "3421"));

			l.Add(new AssignValue("[[address().id]]", "2"));
			l.Add(new AssignValue("[[address().addr]]", "16 test lane"));
			l.Add(new AssignValue("[[address().postcode]]", "3422"));



			env.AssignWithFrame(l, 0);
			env.CommitAssign();

			var Worker = new AdvancedRecordsetWorker(env);
			Worker.LoadRecordset(personRecordsetName);
			Worker.LoadRecordset(addressRecordsetName);
			return Worker;
		}

		[TestMethod]
		[Owner("Candice Daniel")]
		[TestCategory("AdvancedRecordset_Operations")]
		public void AdvancedRecordset_Converter_ConvertDataTableToRecordset_ExpectDataInIEnvironment()
		{
			string returnRecordsetName = "[[AdvancedRecordsetTestResults()]]";
			string query = "select * from person";
			var worker = CreatePersonAddressWorkers();
			var results = worker.ExecuteQuery(query);

			// apply sql results to environment
			worker.ApplyResultToEnvironment(returnRecordsetName);

			// fetch newly inserted data from environment
			var internalResult = env.EvalAsList("[[AdvancedRecordsetTestResults().Name]]", 0);

			// assert that data fetched is what we expect from sql
			var e = internalResult.GetEnumerator();
			if (e.MoveNext())
			{
				Assert.Equals(e.Current, "bob");
			}
			else
			{
				Assert.Fail();
			}
		}

		[TestMethod]
		[Owner("Candice Daniel")]
		[TestCategory("AdvancedRecordset_Converter")]
		public void AdvancedRecordset_Converter_FromRecordset()
		{
			var Worker = CreatePersonAddressWorkers();
			Assert.IsNotNull(Worker);
		}

		[TestMethod]
		[Owner("Candice Daniel")]
		[TestCategory("AdvancedRecordset_Operations")]
		public void AdvancedRecordset_Converter_CanRunSimpleQuery()
		{
			string query = "select * from person";
			var Worker = CreatePersonAddressWorkers();
			var Results = Worker.ExecuteQuery(query);

			Assert.Equals(Results.Count, 4);
		}

		[TestMethod]
		[Owner("Candice Daniel")]
		[TestCategory("AdvancedRecordset_Operations")]
		public void AdvancedRecordset_Converter_CanRunJoinQuery_ExpectAllResults()
		{
			var worker = CreatePersonAddressWorkers();
			string query = "select * from person p join address a on p.address_id=a.id";
			var results = worker.ExecuteQuery(query);

			

			Assert.IsInstanceOfType(results, typeof(IEnumerable<DataStorage.WarewolfAtom>));
			Assert.Equals(results.Count, 3);
			
		}

		[TestMethod]
		[Owner("Candice Daniel")]
		[TestCategory("AdvancedRecordset_Operations")]
		public void AdvancedRecordset_Converter_CanRunWhereQuery_ExpectFilteredResults()
		{
			string query = "select * from person p join address a on p.address_id=a.id where a.addr=\"11 test lane\" order by Name";
			var Worker = CreatePersonAddressWorkers();
			var results = Worker.ExecuteQuery(query);

			Assert.Equals(results[0]["Name"], "bob");
			Assert.Equals(results[0]["Age"], "21");
			Assert.Equals(results[0]["address_id"], "1");
			Assert.Equals(results[0]["Addr"], "11 test lane");
			Assert.Equals(results[0]["Postcode"], "3421");

			Assert.Equals(results[1]["Name"], "jef");
			Assert.Equals(results[1]["Age"], "24");
			Assert.IsTrue(results[0]["Addr"] == results[1]["addr"]); // Case insensitive should work
			Assert.IsTrue(results[1]["Postcode"] == results[0]["Postcode"]);
		}

		[TestMethod]
		[Owner("Candice Daniel")]
		[TestCategory("AdvancedRecordset_Operations")]
		public void AdvancedRecordset_Converter_CanRunWhereQuery_ExpectNoResults()
		{
			string query = "select * from person p join address a on p.address_id=a.id where p.Name=\"zak\"";
			var Worker = CreatePersonAddressWorkers();
			var results = Worker.ExecuteQuery(query);
			Assert.Equals(results.Count, 0);
		}

		[TestMethod]
		[Owner("Candice Daniel")]
		[TestCategory("AdvancedRecordset_Operations")]
		public void AdvancedRecordset_Converter_ExpectCanRunMultipleQueries()
		{
			string query = "select CURRENT_TIMESTAMP;" +
				"select * from address;update person set Age=20 where Name=\"zak\";" +
				"select * from person p join address a on p.address_id=a.id where a.addr=\"11 test lane\" order by Name";
			var Worker = CreatePersonAddressWorkers();
			var results = Worker.ExecuteQuery(query);

			Assert.Equals(results[0]["Name"], "bob");
			Assert.Equals(results[0]["Age"], "21");
			Assert.Equals(results[0]["address_id"], "1");
			Assert.Equals(results[0]["Addr"], "11 test lane");
			Assert.Equals(results[0]["Postcode"], "3421");

			Assert.Equals(results[1]["Name"], "jef");
			Assert.Equals(results[1]["Age"], "24");
			Assert.IsTrue(results[0]["Addr"] == results[1]["addr"]); // Case insensitive should work
			Assert.IsTrue(results[1]["Postcode"] == results[0]["Postcode"]);
		}


		[TestMethod]
		[Owner("Candice Daniel")]
		[TestCategory("AdvancedRecordset_Operations")]
		public void AdvancedRecordset_Converter_ExpectUpdateAffectedRows()
		{
			string query = "update person set Age=65 where Name=\"zak\";";
			var Worker = CreatePersonAddressWorkers();
			var results = Worker.ExecuteQuery(query);

			Assert.Equals(results[0]["AffectedRows"], 1);

			query = "select * from person where Name=\"zak\";";
			results = Worker.ExecuteQuery(query);

			Assert.Equals(results[0]["Name"], "zak");
			Assert.Equals(results[0]["Age"], "65");
		}

		[TestMethod]
		[Owner("Candice Daniel")]
		[TestCategory("AdvancedRecordset_Operations")]
		public void AdvancedRecordset_Converter_ExpectBadSQLToError()
		{
			string query = "select from person";
			var Worker = CreatePersonAddressWorkers();
			Assert.ThrowsException<Exception>(() => Worker.ExecuteQuery(query));
		}

		[TestMethod]
		[Owner("Candice Daniel")]
		[TestCategory("AdvancedRecordset_Activity")]
		public void AdvancedRecordset_Activity_Constructor()
		{
			var ar = new AdvancedRecordsetActivity();
		}
		[TestMethod]
		[Owner("Candice Daniel")]
		[TestCategory("AdvancedRecordset_Activity")]
		public void AdvancedRecordset_Activity_ExecuteTool()
		{
			var act = new AdvancedRecordsetActivity { };

			const string dataList = "<ADL><recset1><field1/><field2/><field3/></recset1><recset2><id/><value/></recset2><OutVar1/></ADL>";
			const string dataListWithData = "<ADL>" +
											"<recset1>" +
											"<field1>1</field1><field2>a</field2><field3>Test1</field3>" +
											"</recset1>" +
											"<recset1>" +
											"<field1>2</field1><field2>b</field2><field3>Test2</field3>" +
											"</recset1>" +
											"<recset1>" +
											"<field1>3</field1><field2>a</field2><field3>Test3</field3>" +
											"</recset1>" +
											"<recset1>" +
											"<field1>4</field1><field2>a</field2><field3>Test4</field3>" +
											"</recset1>" +
											"<recset1>" +
											"<field1>5</field1><field2>c</field2><field3>Test5</field3>" +
											"</recset1>" +
											"<OutVar1/></ADL>";

			var result = CheckActivityDebugInputOutput(act, dataList, dataListWithData, out List<DebugItem> inRes, out List<DebugItem> outRes);
			Assert.AreEqual(0, inRes.Count);
			Assert.AreEqual(1, outRes.Count);

			var debugOutput = outRes[0].FetchResultsList();
			Assert.AreEqual(1, debugOutput.Count);
			Assert.AreEqual("SomeText", debugOutput[0].Value);
			Assert.AreEqual(DebugItemResultType.Value, debugOutput[0].Type);
		}

		public class AdvancedRecordsetWorker
		{
			public Dictionary<string,int> Person;
			public IExecutionEnvironment Environment { get; set; }
			public AdvancedRecordsetWorker(IExecutionEnvironment env)
			{
				Environment = env;
			}
			public Rows ExecuteQuery(string q)
			{
				return new Rows();
			}

			public void LoadRecordset(string recordsetName)
			{
				var table = LoadRecordsetFromEnvironment(recordsetName);
				LoadIntoSQL(table);
			}

			void LoadIntoSQL(List<Dictionary<string, DataStorage.WarewolfAtom>> tableData)
			{

			}

			private List<Dictionary<string,DataStorage.WarewolfAtom>> LoadRecordsetFromEnvironment(string recordsetName) {
				
				var table = new List<Dictionary<string, DataStorage.WarewolfAtom>>();
				var e = Environment.EvalAsTable("[[" + recordsetName + "(*)]]",0);
				foreach(var item in e.Item.Data)
				{
					if (item.Key == "WarewolfPositionColumn") { continue; }
					var FieldName = item.Key;

					var enumerator = item.Value.GetEnumerator();
					int rowIndex = 0;
					while (enumerator.MoveNext())
					{
						if (table.Count <= rowIndex)
						{
							table.Add(new Dictionary<string, DataStorage.WarewolfAtom>());
						}
						table[rowIndex][FieldName] = enumerator.Current;

						rowIndex++;
					}
				}
				return table;
			}

			public void ApplyResultToEnvironment(string returnRecordsetName)
			{
				throw new NotImplementedException();
			}
		}

		public class Rows
		{
			public Row this[int idx] { get => new Row(); }
			public int Count;
		}
		public class Row
		{
			public string this[string fieldName] { get => ""; }
		}

	}
}
