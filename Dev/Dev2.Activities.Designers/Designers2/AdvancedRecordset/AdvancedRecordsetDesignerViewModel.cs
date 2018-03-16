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
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using Dev2.Activities.Designers2.Core;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.DB;
using Dev2.Common.Interfaces.Threading;
using Dev2.Common.Interfaces.ToolBase;
using Dev2.Studio.Interfaces;
using Dev2.Threading;
using Dev2.Activities.Designers2.Core.Extensions;
using Dev2.Providers.Errors;
using Dev2.Common.Interfaces.Infrastructure.Providers.Errors;
using System.Collections.ObjectModel;
using Dev2.Communication;
using System.Windows;
using System.Windows.Input;
using Dev2.Runtime.Configuration.ViewModels.Base;
using System.Linq;

namespace Dev2.Activities.Designers2.AdvancedRecordset
{
	public class AdvancedRecordsetDesignerViewModel : CustomToolWithRegionBase
	{
		const string DoneText = "Done";
		const string FixText = "Fix";
		private Guid GetUniqueId() => GetProperty<Guid>();
		readonly IAsyncWorker _worker;
		readonly IViewPropertyBuilder _propertyBuilder;
		IOutputsToolRegion _outputsRegion;
		IErrorInfo _worstDesignError;

		public AdvancedRecordsetDesignerViewModel(ModelItem modelItem) : this(modelItem, new AsyncWorker(), new ViewPropertyBuilder()) { }
		public AdvancedRecordsetDesignerViewModel(ModelItem modelItem, IAsyncWorker worker, IViewPropertyBuilder propertyBuilder)
		   : base(modelItem)
		{
			_worker = worker;
			_propertyBuilder = propertyBuilder;

			SetupCommonProperties();
			this.RunViewSetup();
			HelpText = Warewolf.Studio.Resources.Languages.HelpText.Tool_AdvancedRecordset;
		}
		void SetupCommonProperties()
		{
			AddTitleBarMappingToggle();
			InitialiseViewModel();
			NoError = new ErrorInfo
			{
				ErrorType = ErrorType.None,
				Message = "Service Working Normally"
			};
			
			UpdateWorstError();
		}
		void AddTitleBarMappingToggle()
		{
			HasLargeView = true;
		}
		void InitialiseViewModel()
		{
			BuildRegions();

			LabelWidth = 46;
			ButtonDisplayValue = DoneText;

			ShowLarge = true;
			ThumbVisibility = Visibility.Visible;
			ShowExampleWorkflowLink = Visibility.Collapsed;

			DesignValidationErrors = new ObservableCollection<IErrorInfo>();
			FixErrorsCommand = new DelegateCommand(o =>
			{
				IsWorstErrorReadOnly = true;
			});

			ExecuteSqlQueryCommand = new DelegateCommand(command =>
			{
				ExecuteSqlQuery(SqlQuery);
			});

			SetDisplayName("");
			OutputsRegion.OutputMappingEnabled = true;

			Properties = _propertyBuilder.BuildProperties(SqlQuery, Type);
			if (OutputsRegion != null && OutputsRegion.IsEnabled)
			{
				var recordsetItem = OutputsRegion.Outputs.FirstOrDefault(mapping => !string.IsNullOrEmpty(mapping.RecordSetName));
				if (recordsetItem != null)
				{
					OutputsRegion.IsEnabled = true;
				}
			}
		}

		private void ExecuteSqlQuery(string SqlQuery)
		{
			if (string.IsNullOrWhiteSpace(SqlQuery))
			{
				return;
			}
			var advancedRecordset = new AdvancedRecordsetActivity
			{
				ExecuteActionString = SqlQuery
			};
			
			// Execute the Sql Query
		}

		string SqlQuery => GetProperty<string>();
		string Type => GetProperty<string>();
		IErrorInfo NoError { get; set; }
		public ObservableCollection<IErrorInfo> DesignValidationErrors { get; set; }
		public int LabelWidth { get; set; }
		public string ButtonDisplayValue { get; set; }
		public List<string> Properties { get; private set; }
		public ICommand ExecuteSqlQueryCommand { get; set; }
		public DelegateCommand FixErrorsCommand { get; set; }

		public static readonly DependencyProperty WorstErrorProperty = DependencyProperty.Register("WorstError", typeof(ErrorType), 
																	typeof(AdvancedRecordsetDesignerViewModel), new PropertyMetadata(ErrorType.None));
		public static readonly DependencyProperty IsWorstErrorReadOnlyProperty = DependencyProperty.Register("IsWorstErrorReadOnly", typeof(bool), 
																	typeof(AdvancedRecordsetDesignerViewModel), new PropertyMetadata(false));
		public ErrorType WorstError
		{
			get { return (ErrorType)GetValue(WorstErrorProperty); }
			private set { SetValue(WorstErrorProperty, value); }
		}
		public ErrorRegion ErrorRegion { get; private set; }
		public IOutputsToolRegion OutputsRegion
		{
			get =>  _outputsRegion;
			set
			{
				_outputsRegion = value;
				OnPropertyChanged();
			}
		}
		public override IList<IToolRegion> BuildRegions()
		{
			IList<IToolRegion> regions = new List<IToolRegion>();

			OutputsRegion = new OutputsRegion(ModelItem);
			regions.Add(OutputsRegion);
			if (OutputsRegion.Outputs.Count > 0)
			{
				OutputsRegion.IsEnabled = true;
			}
			ErrorRegion = new ErrorRegion();
			regions.Add(ErrorRegion);

			Regions = regions;
			return regions;
		}
		public void SetDisplayName(string displayName)
		{
			var index = DisplayName.IndexOf(" -", StringComparison.Ordinal);

			if (index > 0)
			{
				DisplayName = DisplayName.Remove(index);
			}

			var displayName2 = DisplayName;

			if (!string.IsNullOrEmpty(displayName2))
			{
				DisplayName = displayName2;
			}
			if (!string.IsNullOrWhiteSpace(displayName))
			{
				DisplayName = displayName2 + displayName;
			}
		}
		public override void UpdateHelpDescriptor(string helpText)
		{
			var mainViewModel = CustomContainer.Get<IShellViewModel>();
			mainViewModel?.HelpViewModel.UpdateHelpText(helpText);
		}
		public override void Validate()
		{
			if (Errors == null)
			{
				Errors = new List<IActionableErrorInfo>();
			}
			Errors.Clear();

			Errors = Regions.SelectMany(a => a.Errors).Select(a => new ActionableErrorInfo(new ErrorInfo() { Message = a, ErrorType = ErrorType.Critical }, () => { }) as IActionableErrorInfo).ToList();
			if (Errors.Count <= 0)
			{
				ClearValidationMemoWithNoFoundError();
			}
			UpdateWorstError();
		}
		public void ErrorMessage(Exception exception, bool hasError)
		{
			Errors = new List<IActionableErrorInfo>();
			if (hasError)
			{
				Errors = new List<IActionableErrorInfo> { new ActionableErrorInfo(new ErrorInfo() { ErrorType = ErrorType.Critical, FixData = "", FixType = FixType.None, Message = exception.Message, StackTrace = exception.StackTrace }, () => { }) };
			}
		}
		void UpdateWorstError()
		{
			if (DesignValidationErrors.Count == 0)
			{
				DesignValidationErrors.Add(NoError);
			}

			IErrorInfo[] worstError = { DesignValidationErrors[0] };

			foreach (var error in DesignValidationErrors.Where(error => error.ErrorType > worstError[0].ErrorType))
			{
				worstError[0] = error;
				if (error.ErrorType == ErrorType.Critical)
				{
					break;
				}
			}
			SetWorstDesignError(worstError[0]);
		}
		void SetWorstDesignError(IErrorInfo value)
		{
			if (_worstDesignError != value)
			{
				_worstDesignError = value;
				IsWorstErrorReadOnly = value == null || value.ErrorType == ErrorType.None || value.FixType == FixType.None || value.FixType == FixType.Delete;
				WorstError = value?.ErrorType ?? ErrorType.None;
			}
		}
		public bool IsWorstErrorReadOnly
		{
			get { return (bool)GetValue(IsWorstErrorReadOnlyProperty); }
			private set
			{
				ButtonDisplayValue = value ? DoneText : FixText;
				SetValue(IsWorstErrorReadOnlyProperty, value);
			}
		}
		public void ClearValidationMemoWithNoFoundError()
		{
			var memo = new DesignValidationMemo
			{
				InstanceID = GetUniqueId(),
				IsValid = false,
			};
			memo.Errors.Add(new ErrorInfo
			{
				InstanceID = GetUniqueId(),
				ErrorType = ErrorType.None,
				FixType = FixType.None,
				Message = ""
			});
			UpdateDesignValidationErrors(memo.Errors);
		}
		void UpdateDesignValidationErrors(IEnumerable<IErrorInfo> errors)
		{
			DesignValidationErrors.Clear();
			foreach (var error in errors)
			{
				DesignValidationErrors.Add(error);
			}
			UpdateWorstError();
		}
	}
}
