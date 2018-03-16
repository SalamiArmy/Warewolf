using System;
using System.Data;

namespace Dev2.Activities
{
	public interface IAdvancedRecordset : IDisposable
	{
		DataSet ExecuteQuery();
		
		string ExecuteScalar();

		int ExecuteNonQuery();
		
	}
}
