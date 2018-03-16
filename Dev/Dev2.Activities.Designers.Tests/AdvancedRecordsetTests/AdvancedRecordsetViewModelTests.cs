using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Dev2.Activities.Designers2.AdvancedRecordset;
using Dev2.Activities.Designers2.Core;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Core;
using Dev2.Common.Interfaces.Core.DynamicServices;
using Dev2.Common.Interfaces.DB;
using Dev2.Common.Interfaces.Help;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Dev2.Common.Interfaces.ToolBase.Database;
using Dev2.Runtime.ServiceModel.Data;
using Dev2.Studio.Core.Activities.Utils;
using Dev2.Studio.Interfaces;
using Dev2.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Warewolf.Core;


namespace Dev2.Activities.Designers.Tests.AdvancedRecordset
{
	[TestClass]
	public class AdvancedRecordsetViewModelTests
	{

		[TestMethod]
		[Owner("Candice Daniel")]
		[TestCategory("AdvancedRecordset_MethodName")]
		public void AdvancedRecordset_MethodName_ClearErrors()
		{
			//------------Setup for test--------------------------
			var id = Guid.NewGuid();
			var mod = new SqliteModel();
			var act = new AdvancedRecordsetActivity();

			var advancedRecordset = new AdvancedRecordsetDesignerViewModel(ModelItemUtils.CreateModelItem(act), new SynchronousAsyncWorker(), new ViewPropertyBuilder());
			//------------Execute Test---------------------------
			advancedRecordset.ClearValidationMemoWithNoFoundError();
			//------------Assert Results-------------------------
			Assert.IsNull(advancedRecordset.Errors);
			Assert.AreEqual(advancedRecordset.DesignValidationErrors.Count, 1);
		}

		[TestMethod]
		[Owner("Candice Daniel")]
		[TestCategory("AdvancedRecordset_MethodName")]
		public void AdvancedRecordset_Ctor_EmptyModelItem()
		{
			//------------Setup for test--------------------------
			var id = Guid.NewGuid();
			var mod = new SqliteModel();
			var act = new AdvancedRecordsetActivity();

			//------------Execute Test---------------------------
			var advancedRecordset = new AdvancedRecordsetDesignerViewModel(ModelItemUtils.CreateModelItem(act), new SynchronousAsyncWorker(), new ViewPropertyBuilder());

			//------------Assert Results-------------------------
			Assert.IsFalse(advancedRecordset.OutputsRegion.IsEnabled);
			Assert.IsTrue(advancedRecordset.ErrorRegion.IsEnabled);
		}

		[TestMethod]
		[Owner("Candice Daniel")]
		[TestCategory("AdvancedRecordset_MethodName")]
		public void AdvancedRecordset_TestActionSetSourceAndTestClickOkHasMappingsErrorFromServer()
		{
			//------------Setup for test--------------------------
			var id = Guid.NewGuid();
			var mod = new SqliteModel();
			mod.HasRecError = true;
			var act = new AdvancedRecordsetActivity();

			//------------Execute Test---------------------------
			var advancedRecordset = new AdvancedRecordsetDesignerViewModel(ModelItemUtils.CreateModelItem(act), new SynchronousAsyncWorker(), new ViewPropertyBuilder());
#pragma warning disable 4014
			advancedRecordset.ExecuteSqlQueryCommand.Execute(null);
#pragma warning restore 4014

			//------------Assert Results-------------------------
			Assert.IsTrue(advancedRecordset.ErrorRegion.IsEnabled);
		}

		[TestMethod]
		[Owner("Candice Daniel")]
		[TestCategory("AdvancedRecordset_Handle")]
		public void AdvancedRecordset_UpdateHelp_ShouldCallToHelpViewMode()
		{
			//------------Setup for test--------------------------      
			var mockMainViewModel = new Mock<IShellViewModel>();
			var mockHelpViewModel = new Mock<IHelpWindowViewModel>();
			mockHelpViewModel.Setup(model => model.UpdateHelpText(It.IsAny<string>())).Verifiable();
			mockMainViewModel.Setup(model => model.HelpViewModel).Returns(mockHelpViewModel.Object);
			CustomContainer.Register(mockMainViewModel.Object);

			var mod = new SqliteModel();
			mod.HasRecError = true;
			var act = new AdvancedRecordsetActivity();
			var viewModel = new AdvancedRecordsetDesignerViewModel(ModelItemUtils.CreateModelItem(act), new SynchronousAsyncWorker(), new ViewPropertyBuilder());
			//------------Execute Test---------------------------
			viewModel.UpdateHelpDescriptor("help");
			//------------Assert Results-------------------------
			mockHelpViewModel.Verify(model => model.UpdateHelpText(It.IsAny<string>()), Times.Once());
		}

		[TestMethod]
		[Owner("Candice Daniel")]
		[TestCategory("AdvancedRecordset_TestAction")]
		public void AdvancedRecordset_TestActionSetSourceHasRecSet()
		{
			//------------Setup for test--------------------------
			var id = Guid.NewGuid();
			var mod = new SqliteModel();
			var act = new AdvancedRecordsetActivity();
			string query = "select * from person p join address a on p.address_id=a.id";
			//------------Execute Test---------------------------
			var advancedRecordset = new AdvancedRecordsetDesignerViewModel(ModelItemUtils.CreateModelItem(act), new SynchronousAsyncWorker(), new ViewPropertyBuilder());
#pragma warning disable 4014

			advancedRecordset.ExecuteSqlQueryCommand.Execute(query);
#pragma warning restore 4014

			//------------Assert Results-------------------------
			Assert.IsTrue(advancedRecordset.OutputsRegion.IsEnabled);
			Assert.IsTrue(advancedRecordset.ErrorRegion.IsEnabled);
		}
	}
	public class SqliteModel : IDbServiceModel
	{
#pragma warning disable 649
		IStudioUpdateManager _updateRepository;
#pragma warning restore 649
#pragma warning disable 169
		IQueryManager _queryProxy;
#pragma warning restore 169

		public ObservableCollection<IDbSource> _sources = new ObservableCollection<IDbSource>
		{
			new DbSourceDefinition()
			{
				ServerName = "localServer",
				Type = enSourceType.SQLiteDatabase,
				UserName = "johnny",
				Password = "bravo",
				AuthenticationType = AuthenticationType.Public,
				DbName = "",
				Name = "j_bravo",
				Path = "",
				Id = Guid.NewGuid()
			}
		};

		public ObservableCollection<IDbAction> _actions = new ObservableCollection<IDbAction>
		{
			new DbAction()
			{
				Name = "mob",
				Inputs = new List<IServiceInput>() { new ServiceInput("[[a]]", "asa") }
			}
		};

		public ObservableCollection<IDbAction> _refreshActions = new ObservableCollection<IDbAction>
		{
			new DbAction()
			{
				Name = "mob",
				Inputs = new List<IServiceInput>() { new ServiceInput("[[a]]", "asa") }
			},
			new DbAction()
			{
				Name = "arefreshOne",
				Inputs = new List<IServiceInput>() { new ServiceInput("[[b]]", "bsb") }
			}
		};

		public ICollection<IDbAction> RefreshActions(IDbSource source)
		{
			return RefreshActionsList;
		}
		public ICollection<IDbAction> RefreshActionsList => _refreshActions;
		public bool HasRecError { get; set; }

		#region Implementation of IDbServiceModel

		public ObservableCollection<IDbSource> RetrieveSources()
		{
			return Sources;
		}

		public ObservableCollection<IDbSource> Sources => _sources;

		public ICollection<IDbAction> GetActions(IDbSource source)
		{
			return Actions;
		}

		public ICollection<IDbAction> Actions => _actions;

		public void CreateNewSource(enSourceType type)
		{
		}
		public void EditSource(IDbSource selectedSource, enSourceType type)
		{
		}

		public DataTable TestService(IDatabaseService inputValues)
		{
			if (ThrowsTestError)
			{
				throw new Exception("bob");
			}

			if (HasRecError)
			{
				return null;
			}
			var dt = new DataTable();
			dt.Columns.Add("a");
			dt.Columns.Add("b");
			dt.Columns.Add("c");
			dt.TableName = "bob";
			return dt;

		}

		public IStudioUpdateManager UpdateRepository => _updateRepository;
		public bool ThrowsTestError { get; set; }

		#endregion
	}

	public class SqliteModelWithOneColumnReturn : IDbServiceModel
	{
#pragma warning disable 649
		IStudioUpdateManager _updateRepository;
#pragma warning restore 649
#pragma warning disable 169
		IQueryManager _queryProxy;
#pragma warning restore 169

		public ObservableCollection<IDbSource> _sources = new ObservableCollection<IDbSource>
		{
			new DbSourceDefinition()
			{
				ServerName = "localServer",
				Type = enSourceType.SQLiteDatabase,
				UserName = "johnny",
				Password = "bravo",
				AuthenticationType = AuthenticationType.Public,
				DbName = "",
				Name = "j_bravo",
				Path = "",
				Id = Guid.NewGuid()
			}
		};

		public ObservableCollection<IDbAction> _actions = new ObservableCollection<IDbAction>
		{
			new DbAction()
			{
				Name = "mob",
				Inputs = new List<IServiceInput>() { new ServiceInput("[[a]]", "asa") }
			}
		};

		public ObservableCollection<IDbAction> _refreshActions = new ObservableCollection<IDbAction>
		{
			new DbAction()
			{
				Name = "mob",
				Inputs = new List<IServiceInput>() { new ServiceInput("[[a]]", "asa") }
			},
			new DbAction()
			{
				Name = "arefreshOne",
				Inputs = new List<IServiceInput>() { new ServiceInput("[[b]]", "bsb") }
			}
		};

		public ICollection<IDbAction> RefreshActions(IDbSource source)
		{
			return RefreshActionsList;
		}
		public ICollection<IDbAction> RefreshActionsList => _refreshActions;
		public bool HasRecError { get; set; }

		#region Implementation of IDbServiceModel

		public ObservableCollection<IDbSource> RetrieveSources()
		{
			return Sources;
		}

		public ObservableCollection<IDbSource> Sources => _sources;

		public ICollection<IDbAction> GetActions(IDbSource source)
		{
			return Actions;
		}

		public ICollection<IDbAction> Actions => _actions;

		public void CreateNewSource(enSourceType type)
		{
		}
		public void EditSource(IDbSource selectedSource, enSourceType type)
		{
		}

		public DataTable TestService(IDatabaseService inputValues)
		{
			if (ThrowsTestError)
			{
				throw new Exception("bob");
			}

			if (HasRecError)
			{
				return null;
			}
			var dt = new DataTable();
			dt.Columns.Add("a");
			dt.TableName = "bob";
			return dt;

		}

		public IStudioUpdateManager UpdateRepository => _updateRepository;
		public bool ThrowsTestError { get; set; }

		#endregion
	}
	class InputViewForTest : ManageDatabaseServiceInputViewModel
	{
		public InputViewForTest(IDatabaseServiceViewModel model, IDbServiceModel serviceModel)
			: base(model, serviceModel)
		{
		}
	}
}
