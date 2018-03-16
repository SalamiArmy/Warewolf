using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using Dev2.Activities.Debug;
using Dev2.Common;
using Dev2.Common.Interfaces.DB;
using Dev2.Common.Interfaces.Diagnostics.Debug;
using Dev2.Common.Interfaces.Toolbox;
using Dev2.Data.TO;
using Dev2.Data.Util;
using Dev2.Diagnostics;
using Dev2.Interfaces;
using Dev2.Services.Execution;
using Unlimited.Applications.BusinessDesignStudio.Activities;
using Warewolf.Core;
using Warewolf.Resource.Errors;
using Warewolf.Storage;
using Warewolf.Storage.Interfaces;

namespace Dev2.Activities
{
	[ToolDescriptorInfo("Advanced Recordset", "Advanced Recordset", ToolType.Native, "8999E59B-38A3-43BB-A98F-6090C5C9EA1E", "Dev2.Acitivities", "1.0.0.0", "Legacy", "Database", "/Warewolf.Studio.Themes.Luna;component/Images.xaml", "Tool_AdvancedRecordset")]
	public class AdvancedRecordsetActivity : DsfActivity, IEquatable<AdvancedRecordsetActivity>
	{
		public IExecutionEnvironment ExecutionEnvironment { get; protected set; }
		public AdvancedRecordsetActivity()
		{
			Type = "Advanced Recordset";
			DisplayName = "Advanced Recordset";
		}
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SqlQuery { get; set; }

		public string ExecuteActionString { get; set; }
		protected override void OnExecute(NativeActivityContext context)
		{
			var dataObject = context.GetExtension<IDSFDataObject>();
			ExecuteTool(dataObject, 0);
		}
		protected override void ExecuteTool(IDSFDataObject dataObject, int update)
		{
			var allErrors = new ErrorResultTO();
			InitializeDebug(dataObject);
			try
			{
				AddValidationErrors(allErrors);
				if (!allErrors.HasErrors())
				{
					var advancedrecordset = new AdvancedRecordset(dataObject.Environment)
					{
						SqlQuery = SqlQuery
					};
					var results = advancedrecordset.ExecuteQuery();

					// apply sql results to environment
					advancedrecordset.ApplyResultToEnvironment("newrecordsetname", results.Tables[0].Rows.Cast<DataRow>().ToList());

					if (dataObject.IsDebugMode() && !allErrors.HasErrors())
					{
						var resDebug = new DebugEvalResult(SqlQuery, "", dataObject.Environment, update);
						AddDebugOutputItem(resDebug);
					}
				}
			}
			catch (Exception e)
			{
				Dev2Logger.Error("AdvancedRecordset", e, GlobalConstants.WarewolfError);
				allErrors.AddError(e.Message);
			}
			finally
			{
				// Handle Errors
				var hasErrors = allErrors.HasErrors();
				if (hasErrors)
				{
					DisplayAndWriteError("AdvancedRecordset", allErrors);
					var errorString = allErrors.MakeDisplayReady();
					dataObject.Environment.AddError(errorString);
				}
				if (dataObject.IsDebugMode())
				{
					if (hasErrors)
					{
						AddDebugOutputItem(new DebugItemStaticDataParams("", SqlQuery, ""));
					}
					DispatchDebugState(dataObject, StateType.Before, update);
					DispatchDebugState(dataObject, StateType.After, update);
				}
			}
		}
		void AddValidationErrors(ErrorResultTO allErrors)
		{
			if (DataListUtil.HasNegativeIndex(SqlQuery))
			{
				allErrors.AddError(string.Format("Negative Recordset Index for SqlQuery: {0}", SqlQuery));
			}
		}
	
		protected override void ExecutionImpl(IEsbChannel esbChannel, IDSFDataObject dataObject, string inputs, string outputs, out ErrorResultTO tmpErrors, int update)
		{
			var execErrors = new ErrorResultTO();

			tmpErrors = new ErrorResultTO();
			tmpErrors.MergeErrors(execErrors);

			var allErrors = new ErrorResultTO();
			InitializeDebug(dataObject);
			try
			{
				var advancedRecordset = new AdvancedRecordset(dataObject.Environment);
				var personRecordsetName = "person";
				var addressRecordsetName = "address";
				advancedRecordset.LoadRecordsetAsTable(personRecordsetName);
				advancedRecordset.LoadRecordsetAsTable(addressRecordsetName);
				advancedRecordset.SqlQuery = dataObject.QueryString;
				var results = advancedRecordset.ExecuteQuery();

				// apply sql results to environment
				advancedRecordset.ApplyResultToEnvironment("", results.Tables[0].Rows.Cast<DataRow>().ToList());

				// fetch newly inserted data from environment
				var internalResult = advancedRecordset.Environment.EvalAsList("[[person(*).name]]", 0);


				var fetchErrors = allErrors.FetchErrors();
				foreach (var error in fetchErrors)
				{
					dataObject.Environment.Errors.Add(error);
				}
			}
			catch (Exception e)
			{
				allErrors.AddError(e.Message);
			}

			tmpErrors.MergeErrors(execErrors);

		}

		public override List<DebugItem> GetDebugInputs(IExecutionEnvironment env, int update)
		{
			if (env == null)
			{
				return new List<DebugItem>();
			}
			base.GetDebugInputs(env, update);

			if (Inputs != null)
			{
				foreach (var serviceInput in Inputs)
				{
					var debugItem = new DebugItem();
					AddDebugItem(new DebugEvalResult(serviceInput.Value, serviceInput.Name, env, update), debugItem);
					_debugInputs.Add(debugItem);
				}
			}
			return _debugInputs;
		}

		public override enFindMissingType GetFindMissingType() => enFindMissingType.DataGridActivity;

		public bool Equals(AdvancedRecordsetActivity other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return base.Equals(other) && string.Equals(SqlQuery, other.SqlQuery);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (obj.GetType() != this.GetType())
			{
				return false;
			}

			return Equals((AdvancedRecordsetActivity)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = base.GetHashCode();
				hashCode = (hashCode * 397) ^ (SourceId.GetHashCode());
				if (ExecuteActionString != null)
				{
					hashCode = (hashCode * 397) ^ (ExecuteActionString.GetHashCode());
				}
				return hashCode;
			}
		}
	}
}
