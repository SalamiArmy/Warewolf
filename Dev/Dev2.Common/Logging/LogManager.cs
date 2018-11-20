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
using Dev2.Common.Container;
using Dev2.Common.Interfaces.Logging;
using Dev2.Common.Interfaces.Wrappers;
using System.Data.SQLite;
using System.IO;
using Dev2.Interfaces;

namespace Dev2.Common.Logging
{
    public class LogManager : ILogManager, IDisposable
    {
        private static LogManager _instance;

        readonly IDev2StateAuditLogger _logger;
        readonly IStateNotifierFactory _stateNotifierFactory;
        readonly IFile _fileWrapper;
        readonly IDirectory _directoryWrapper;

        public LogManager(IDev2StateAuditLogger logger, IStateNotifierFactory stateNotifierFactory, IFile fileWrapper, IDirectory directoryWrapper) {
            _logger = logger;
            _stateNotifierFactory = stateNotifierFactory;
            _fileWrapper = fileWrapper;
            _directoryWrapper = directoryWrapper;
        }

        public static void Initialize(IDev2StateAuditLogger logger, IStateNotifierFactory stateNotifierFactory, IFile fileWrapper, IDirectory directoryWrapper)
        {
            _instance = new LogManager(logger, stateNotifierFactory, fileWrapper, directoryWrapper);
        }

        public static IStateNotifier CreateStateNotifier(IDSFDataObject dsfDataObject)
        {
            return _instance.CreateStateNotifierImpl(dsfDataObject);
        }

        private IStateNotifier CreateStateNotifierImpl(IDSFDataObject dsfDataObject)
        {
            var stateNotifier = _stateNotifierFactory.NewInstance();

            if (dsfDataObject.Settings.EnableDetailedLogging)
            {
                stateNotifier.Subscribe(_logger.NewStateListener(dsfDataObject));
            }
            return stateNotifier;
        }

        public static void FlushLogs()
        {
            _instance.FlushLogsImpl();
        }
        private void FlushLogsImpl()
        {
            _logger.Flush();
        }

        public bool SaveLoggingPath(string auditsFilePath)
        {
            var sourceFilePath = Config.Server.AuditFilePath;

            if (sourceFilePath != auditsFilePath)
            {

                var source = Path.Combine(sourceFilePath, "auditDB.db");
                if (_fileWrapper.Exists(source))
                {
                    var destination = Path.Combine(auditsFilePath, "auditDB.db");
                    _directoryWrapper.CreateIfNotExists(auditsFilePath);


                    var connectionString = new SQLiteConnectionStringBuilder
                    {
                        DataSource = Path.Combine(Config.Server.AuditFilePath, "auditDB.db"),
                        ForeignKeys = true
                    }.ConnectionString;

                    try
                    {
                        PauseFlush();
                        //SQLiteConnection.ConnectionPool.ClearAllPools()
                        SQLiteConnection.ClearAllPools();
                        SQLiteConnection.Shutdown(false, false);
                        var sqliteConnection = new SQLiteConnection(connectionString);
                        sqliteConnection.Close();
                        sqliteConnection.Dispose();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                    finally
                    {
                        ResumeFlush();
                    }

                    _fileWrapper.Move(source, destination);
                    Config.Server.AuditFilePath = auditsFilePath;
                    return true;
                }
            }
            return false;
        }

        private void PauseFlush()
        {
            throw new NotImplementedException();
        }

        private void ResumeFlush()
        {
            throw new NotImplementedException();
        }

        private bool isDisposed = false; // To detect redundant calls
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    _logger.Dispose();
                }
                isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
