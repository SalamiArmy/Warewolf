using System;
using System.Data;

namespace Dev2.Activities.AdvancedRecordset
{
	public interface IAdvancedRecordset : IDisposable
	{
		DataSet ExecuteQuery(string sql);
		
		string ExecuteScalar(string sql);

		int ExecuteNonQuery(string sql);
		
	}
}
