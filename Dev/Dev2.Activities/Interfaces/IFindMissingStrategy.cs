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
using Dev2.Activities;
using Dev2.Activities.WcfEndPoint;
using Dev2.Common.Interfaces.Patterns;
using Unlimited.Applications.BusinessDesignStudio.Activities;

namespace Dev2.Interfaces
{
    public interface IFindMissingStrategy : ISpookyLoadable<Enum>
    {
        List<string> GetActivityFields(object activity);
        List<string> GetActivityFields(DsfBaseConvertActivity activity);
        List<string> GetActivityFields(DsfCaseConvertActivity activity);
        List<string> GetActivityFields(DsfMultiAssignActivity activity);
        List<string> GetActivityFields(DsfDotNetMultiAssignActivity activity);
        List<string> GetActivityFields(DsfDotNetMultiAssignObjectActivity activity);
        List<string> GetActivityFields(DsfGatherSystemInformationActivity activity);
        List<string> GetActivityFields(DsfDotNetGatherSystemInformationActivity activity);
        List<string> GetActivityFields(DsfSqlServerDatabaseActivity activity);
        List<string> GetActivityFields(DsfMySqlDatabaseActivity activity);
        List<string> GetActivityFields(DsfPostgreSqlActivity activity);
        List<string> GetActivityFields(DsfOracleDatabaseActivity activity);
        List<string> GetActivityFields(DsfODBCDatabaseActivity activity);
        List<string> GetActivityFields(DsfWebPostActivity activity);
        List<string> GetActivityFields(DsfWebDeleteActivity activity);
        List<string> GetActivityFields(DsfWebPutActivity activity);
        List<string> GetActivityFields(DsfWebGetActivity activity);
        List<string> GetActivityFields(DsfDotNetDllActivity activity);
        List<string> GetActivityFields(DsfEnhancedDotNetDllActivity activity);
        List<string> GetActivityFields(DsfComDllActivity activity);
        List<string> GetActivityFields(DsfWcfEndPointActivity activity);
    }
}
