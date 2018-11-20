using Dev2.Common.Logging;
using Dev2.Common.Wrappers;
using Dev2.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Linq.Mapping;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev2.Common
{


    public interface IDev2StateAuditLogger : IDisposable
    {
        IStateListener NewStateListener(IDSFDataObject dataObject);
        void Flush();
    }

    public interface IAuditDatabaseContext : IDisposable
    {
        DbSet<AuditLog> Audits { get; set; }
        int SaveChanges();
    }
    public interface IDatabaseContextFactory
    {
        IAuditDatabaseContext Get();
    }
    public class DatabaseContextFactory : IDatabaseContextFactory
    {
        public IAuditDatabaseContext Get()
        {
            return new DatabaseContext();
        }
    }


    public class SQLiteConfiguration : DbConfiguration
    {
        public SQLiteConfiguration()
        {
            SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
            SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);
            SetProviderServices("System.Data.SQLite", (DbProviderServices)SQLiteProviderFactory.Instance.GetService(typeof(DbProviderServices)));
        }
    }

    [Database]
    public class DatabaseContext : DbContext, IAuditDatabaseContext
    {
        public DatabaseContext() : base(new SQLiteConnection
        {
            ConnectionString = new SQLiteConnectionStringBuilder
            {
                DataSource = Path.Combine(Config.Server.AuditFilePath, "auditDB.db"),
                ForeignKeys = true
            }.ConnectionString
        }, true)
        {
            var userPrinciple = Utilities.ServerUser;
            Utilities.PerformActionInsideImpersonatedContext(userPrinciple, () =>
            {
                var directoryWrapper = new DirectoryWrapper();
                directoryWrapper.CreateIfNotExists(Config.Server.AuditFilePath);
                DbConfiguration.SetConfiguration(new SQLiteConfiguration());
                this.Database.CreateIfNotExists();
                this.Database.Initialize(false);
                this.Database.ExecuteSqlCommand("CREATE TABLE IF NOT EXISTS \"AuditLog\" ( `Id` INTEGER PRIMARY KEY AUTOINCREMENT, `WorkflowID` TEXT, `WorkflowName` TEXT, `ExecutionID` TEXT, `AuditType` TEXT, `PreviousActivity` TEXT, `PreviousActivityType` TEXT, `PreviousActivityID` TEXT, `NextActivity` TEXT, `NextActivityType` TEXT, `NextActivityID` TEXT, `ServerID` TEXT, `ParentID` TEXT, `ClientID` TEXT, `ExecutingUser` TEXT, `ExecutionOrigin` INTEGER, `ExecutionOriginDescription` TEXT, `ExecutionToken` TEXT, `AdditionalDetail` TEXT, `IsSubExecution` INTEGER, `IsRemoteWorkflow` INTEGER, `Environment` TEXT, `AuditDate` TEXT )");
            });
        }

        public void TryStopConnection()
        {
            try
            {
                Database.Connection.Close();
                SQLiteConnection.ClearAllPools();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AuditLog> Audits { get; set; }
    }

}
