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
using System.Reflection;
using Dev2.Activities;
using Dev2.Activities.WcfEndPoint;
using Dev2.Interfaces;
using Dev2.Util;
using Dev2.Utilities;
using Unlimited.Applications.BusinessDesignStudio.Activities;

namespace Dev2.FindMissingStrategies
{
    public class DataGridActivityFindMissingStrategy : IFindMissingStrategy
    {
        public Enum HandlesType() => enFindMissingType.DataGridActivity;
        
        public List<string> GetActivityFields(object activity)
        {
            switch (activity)
            {
                case DsfBaseConvertActivity dsfBaseConvertActivity:
                    return GetActivityFields(dsfBaseConvertActivity);
                case DsfCaseConvertActivity dsfCaseConvertActivity:
                    return GetActivityFields(dsfCaseConvertActivity);
                case DsfMultiAssignActivity dsfMultiAssignActivity:
                    return GetActivityFields(dsfMultiAssignActivity);
                case DsfMultiAssignObjectActivity dsfMultiAssignObjectActivity:
                    return GetActivityFields(dsfMultiAssignObjectActivity);
                case DsfDotNetMultiAssignActivity dsfDotNetMultiAssignActivity:
                    return GetActivityFields(dsfDotNetMultiAssignActivity);
                case DsfDotNetMultiAssignObjectActivity dsfDotNetMultiAssignObjectActivity:
                    return GetActivityFields(dsfDotNetMultiAssignObjectActivity);
                case DsfGatherSystemInformationActivity dsfGatherSystemInformationActivity:
                    return GetActivityFields(dsfGatherSystemInformationActivity);
                case DsfDotNetGatherSystemInformationActivity dsfDotNetGatherSystemInformationActivity:
                    return GetActivityFields(dsfDotNetGatherSystemInformationActivity);
                case DsfSqlServerDatabaseActivity dsfSqlServerDatabaseActivity:
                    return GetActivityFields(dsfSqlServerDatabaseActivity);
                case DsfMySqlDatabaseActivity dsfMySqlDatabaseActivity:
                    return GetActivityFields(dsfMySqlDatabaseActivity);
                case DsfPostgreSqlActivity dsfPostgreSqlActivity:
                    return GetActivityFields(dsfPostgreSqlActivity);
                case DsfOracleDatabaseActivity dsfOracleDatabaseActivity:
                    return GetActivityFields(dsfOracleDatabaseActivity);
                case DsfODBCDatabaseActivity dsfODBCDatabaseActivity:
                    return GetActivityFields(dsfODBCDatabaseActivity);
                case DsfWebPostActivity dsfWebPostActivity:
                    return GetActivityFields(dsfWebPostActivity);
                case DsfWebDeleteActivity dsfWebDeleteActivity:
                    return GetActivityFields(dsfWebDeleteActivity);
                case DsfWebPutActivity dsfWebPutActivity:
                    return GetActivityFields(dsfWebPutActivity);
                case DsfWebGetActivity dsfWebGetActivity:
                    return GetActivityFields(dsfWebGetActivity);
                case DsfDotNetDllActivity dsfDotNetDllActivity:
                    return GetActivityFields(dsfDotNetDllActivity);
                case DsfEnhancedDotNetDllActivity dsfEnhancedDotNetDllActivity:
                    return GetActivityFields(dsfEnhancedDotNetDllActivity);
                case DsfComDllActivity dsfComDllActivity:
                    return GetActivityFields(dsfComDllActivity);
                case DsfWcfEndPointActivity dsfWcfEndPointActivity:
                    return GetActivityFields(dsfWcfEndPointActivity);
                default:
                    return new List<string>();
            }
        }

        List<string> GetActivityFields(DsfWcfEndPointActivity activity)
        {
            var results = new List<string>();
            if (activity.Inputs != null)
            {
                results.AddRange(InternalFindMissing(activity.Inputs));
            }
            if (!string.IsNullOrEmpty(activity.OnErrorVariable))
            {
                results.Add(activity.OnErrorVariable);
            }
            if (activity.IsObject)
            {
                if (!string.IsNullOrEmpty(activity.ObjectName))
                {
                    results.Add(activity.ObjectName);
                }
            }
            else
            {
                if (activity.Outputs != null)
                {
                    results.AddRange(InternalFindMissing(activity.Outputs));
                }
            }
            if (!string.IsNullOrEmpty(activity.OnErrorWorkflow))
            {
                results.Add(activity.OnErrorWorkflow);
            }
            return results;
        }

        List<string> GetActivityFields(DsfComDllActivity activity)
        {
            var results = new List<string>();
            if (activity.Inputs != null)
            {
                results.AddRange(InternalFindMissing(activity.Inputs));
            }
            if (!string.IsNullOrEmpty(activity.OnErrorVariable))
            {
                results.Add(activity.OnErrorVariable);
            }
            if (activity.IsObject)
            {
                if (!string.IsNullOrEmpty(activity.ObjectName))
                {
                    results.Add(activity.ObjectName);
                }
            }
            else
            {
                if (activity.Outputs != null)
                {
                    results.AddRange(InternalFindMissing(activity.Outputs));
                }
            }
            if (!string.IsNullOrEmpty(activity.OnErrorWorkflow))
            {
                results.Add(activity.OnErrorWorkflow);
            }
            return results;
        }

        List<string> GetActivityFields(DsfEnhancedDotNetDllActivity activity)
        {
            var results = new List<string>();
            if (activity.ConstructorInputs != null)
            {
                results.AddRange(InternalFindMissing(activity.ConstructorInputs));
            }
            if (!string.IsNullOrEmpty(activity.OnErrorVariable))
            {
                results.Add(activity.OnErrorVariable);
            }

            if (activity.MethodsToRun != null)
            {
                foreach (var pluginAction in activity.MethodsToRun)
                {
                    AddMethodsToRun(results, pluginAction);
                }
            }
            if (activity.IsObject)
            {
                if (!string.IsNullOrEmpty(activity.ObjectName))
                {
                    results.Add(activity.ObjectName);
                }
            }
            else
            {
                if (activity.Outputs != null)
                {
                    results.AddRange(InternalFindMissing(activity.Outputs));
                }
            }
            if (!string.IsNullOrEmpty(activity.OnErrorWorkflow))
            {
                results.Add(activity.OnErrorWorkflow);
            }
            return results;
        }

        void AddMethodsToRun(List<string> results, Common.Interfaces.IPluginAction pluginAction)
        {
            if (pluginAction?.Inputs != null)
            {
                results.AddRange(InternalFindMissing(pluginAction.Inputs));
            }
            if (!string.IsNullOrEmpty(pluginAction?.OutputVariable))
            {
                results.Add(pluginAction.OutputVariable);
            }
        }

        List<string> GetActivityFields(DsfDotNetDllActivity activity)
        {
            var results = new List<string>();
            if (activity.Inputs != null)
            {
                results.AddRange(InternalFindMissing(activity.Inputs));
            }
            if (!string.IsNullOrEmpty(activity.OnErrorVariable))
            {
                results.Add(activity.OnErrorVariable);
            }
            if (activity.IsObject)
            {
                if (!string.IsNullOrEmpty(activity.ObjectName))
                {
                    results.Add(activity.ObjectName);
                }
            }
            else
            {

                if (activity.Outputs != null)
                {
                    results.AddRange(InternalFindMissing(activity.Outputs));
                }
            }
            if (!string.IsNullOrEmpty(activity.OnErrorWorkflow))
            {
                results.Add(activity.OnErrorWorkflow);
            }
            return results;
        }

        List<string> GetActivityFields(DsfWebGetActivity activity)
        {
            var results = new List<string>();
            if (activity.Inputs != null)
            {
                results.AddRange(InternalFindMissing(activity.Inputs));
            }
            if (activity.QueryString != null)
            {
                results.Add(activity.QueryString);
            }
            if (activity.Headers != null)
            {
                foreach (var nameValue in activity.Headers)
                {
                    results.Add(nameValue.Name);
                    results.Add(nameValue.Value);
                }
            }
            if (!string.IsNullOrEmpty(activity.ObjectName))
            {
                results.Add(activity.ObjectName);
            }
            if (!string.IsNullOrEmpty(activity.OnErrorVariable))
            {
                results.Add(activity.OnErrorVariable);
            }
            if (activity.IsObject)
            {
                if (!string.IsNullOrEmpty(activity.ObjectName))
                {
                    results.Add(activity.ObjectName);
                }
            }
            else
            {
                if (activity.Outputs != null)
                {
                    results.AddRange(InternalFindMissing(activity.Outputs));
                }
            }
            if (!string.IsNullOrEmpty(activity.OnErrorWorkflow))
            {
                results.Add(activity.OnErrorWorkflow);
            }
            return results;
        }

        List<string> GetActivityFields(DsfWebPutActivity activity)
        {
            var results = new List<string>();
            if (activity.Inputs != null)
            {
                results.AddRange(InternalFindMissing(activity.Inputs));
            }
            if (activity.QueryString != null)
            {
                results.Add(activity.QueryString);
            }
            if (activity.PutData != null)
            {
                results.Add(activity.PutData);
            }
            if (activity.Headers != null)
            {
                foreach (var nameValue in activity.Headers)
                {
                    results.Add(nameValue.Name);
                    results.Add(nameValue.Value);
                }
            }
            if (!string.IsNullOrEmpty(activity.OnErrorVariable))
            {
                results.Add(activity.OnErrorVariable);
            }
            if (activity.IsObject)
            {
                if (!string.IsNullOrEmpty(activity.ObjectName))
                {
                    results.Add(activity.ObjectName);
                }
            }
            else
            {
                if (activity.Outputs != null)
                {
                    results.AddRange(InternalFindMissing(activity.Outputs));
                }
            }
            if (!string.IsNullOrEmpty(activity.OnErrorWorkflow))
            {
                results.Add(activity.OnErrorWorkflow);
            }
            return results;
        }

        List<string> GetActivityFields(DsfWebDeleteActivity activity)
        {
            var results = new List<string>();
            if (activity.Inputs != null)
            {
                results.AddRange(InternalFindMissing(activity.Inputs));
            }
            if (activity.QueryString != null)
            {
                results.Add(activity.QueryString);
            }
            if (activity.Headers != null)
            {
                foreach (var nameValue in activity.Headers)
                {
                    results.Add(nameValue.Name);
                    results.Add(nameValue.Value);
                }
            }
            if (!string.IsNullOrEmpty(activity.OnErrorVariable))
            {
                results.Add(activity.OnErrorVariable);
            }
            if (activity.IsObject)
            {
                if (!string.IsNullOrEmpty(activity.ObjectName))
                {
                    results.Add(activity.ObjectName);
                }
            }
            else
            {
                if (activity.Outputs != null)
                {
                    results.AddRange(InternalFindMissing(activity.Outputs));
                }
            }
            if (!string.IsNullOrEmpty(activity.OnErrorWorkflow))
            {
                results.Add(activity.OnErrorWorkflow);
            }
            return results;
        }

        List<string> GetActivityFields(DsfWebPostActivity activity)
        {
            var results = new List<string>();
            if (activity.Inputs != null)
            {
                results.AddRange(InternalFindMissing(activity.Inputs));
            }
            if (activity.QueryString != null)
            {
                results.Add(activity.QueryString);
            }
            if (activity.PostData != null)
            {
                results.Add(activity.PostData);
            }
            if (activity.Headers != null)
            {
                results.AddRange(AddAllHeaders(activity));
            }
            if (!string.IsNullOrEmpty(activity.OnErrorVariable))
            {
                results.Add(activity.OnErrorVariable);
            }
            if (!string.IsNullOrEmpty(activity.OnErrorWorkflow))
            {
                results.Add(activity.OnErrorWorkflow);
            }
            if (activity.IsObject)
            {
                if (!string.IsNullOrEmpty(activity.ObjectName))
                {
                    results.Add(activity.ObjectName);
                }
            }
            else
            {
                if (activity.Outputs != null)
                {
                    results.AddRange(InternalFindMissing(activity.Outputs));
                }
            }
            return results;
        }

        List<string> GetActivityFields(DsfODBCDatabaseActivity activity)
        {
            var results = new List<string>();
            if (activity.Inputs != null)
            {
                results.AddRange(InternalFindMissing(activity.Inputs));
            }
            if (activity.CommandText != null)
            {
                results.Add(activity.CommandText);
            }
            if (activity.Outputs != null)
            {
                results.AddRange(InternalFindMissing(activity.Outputs));
            }
            if (!string.IsNullOrEmpty(activity.OnErrorVariable))
            {
                results.Add(activity.OnErrorVariable);
            }
            if (!string.IsNullOrEmpty(activity.OnErrorWorkflow))
            {
                results.Add(activity.OnErrorWorkflow);
            }
            return results;
        }

        List<string> GetActivityFields(DsfOracleDatabaseActivity activity)
        {
            var results = new List<string>();
            if (activity.Inputs != null)
            {
                results.AddRange(InternalFindMissing(activity.Inputs));
            }
            if (activity.Outputs != null)
            {
                results.AddRange(InternalFindMissing(activity.Outputs));
            }
            if (!string.IsNullOrEmpty(activity.OnErrorVariable))
            {
                results.Add(activity.OnErrorVariable);
            }

            if (!string.IsNullOrEmpty(activity.OnErrorWorkflow))
            {
                results.Add(activity.OnErrorWorkflow);
            }
            return results;
        }

        List<string> GetActivityFields(DsfPostgreSqlActivity activity)
        {
            var results = new List<string>();
            if (activity.Inputs != null)
            {
                results.AddRange(InternalFindMissing(activity.Inputs));
            }
            if (activity.Outputs != null)
            {
                results.AddRange(InternalFindMissing(activity.Outputs));
            }
            if (!string.IsNullOrEmpty(activity.OnErrorVariable))
            {
                results.Add(activity.OnErrorVariable);
            }
            if (!string.IsNullOrEmpty(activity.OnErrorWorkflow))
            {
                results.Add(activity.OnErrorWorkflow);
            }
            return results;
        }

        List<string> GetActivityFields(DsfMySqlDatabaseActivity activity)
        {
            var results = new List<string>();
            if (activity.Inputs != null)
            {
                results.AddRange(InternalFindMissing(activity.Inputs));
            }
            if (activity.Outputs != null)
            {
                results.AddRange(InternalFindMissing(activity.Outputs));
            }
            if (!string.IsNullOrEmpty(activity.OnErrorVariable))
            {
                results.Add(activity.OnErrorVariable);
            }
            if (!string.IsNullOrEmpty(activity.OnErrorWorkflow))
            {
                results.Add(activity.OnErrorWorkflow);
            }
            return results;
        }

        List<string> GetActivityFields(DsfSqlServerDatabaseActivity activity)
        {
            var results = new List<string>();
            if (activity.Inputs != null)
            {
                results.AddRange(InternalFindMissing(activity.Inputs));
            }
            if (activity.Outputs != null)
            {
                results.AddRange(InternalFindMissing(activity.Outputs));
            }
            if (!string.IsNullOrEmpty(activity.OnErrorVariable))
            {
                results.Add(activity.OnErrorVariable);
            }
            if (!string.IsNullOrEmpty(activity.OnErrorWorkflow))
            {
                results.Add(activity.OnErrorWorkflow);
            }
            return results;
        }

        List<string> GetActivityFields(DsfDotNetGatherSystemInformationActivity activity) => (List<string>)InternalFindMissing(activity.SystemInformationCollection);
        List<string> GetActivityFields(DsfGatherSystemInformationActivity activity) => (List<string>)InternalFindMissing(activity.SystemInformationCollection);
        List<string> GetActivityFields(DsfDotNetMultiAssignObjectActivity activity) => (List<string>)InternalFindMissing(activity.FieldsCollection);
        List<string> GetActivityFields(DsfDotNetMultiAssignActivity activity) => (List<string>)InternalFindMissing(activity.FieldsCollection);
        List<string> GetActivityFields(DsfMultiAssignObjectActivity activity) => (List<string>)InternalFindMissing(activity.FieldsCollection);
        List<string> GetActivityFields(DsfMultiAssignActivity activity) => (List<string>)InternalFindMissing(activity.FieldsCollection);
        List<string> GetActivityFields(DsfBaseConvertActivity activity) => (List<string>)InternalFindMissing(activity.ConvertCollection);
        List<string> GetActivityFields(DsfCaseConvertActivity activity) => (List<string>)InternalFindMissing(activity.ConvertCollection);

        static List<string> AddAllHeaders(DsfWebPostActivity maAct)
        {
            var results = new List<string>();
            foreach (var nameValue in maAct.Headers)
            {
                results.Add(nameValue.Name);
                results.Add(nameValue.Value);
            }
            return results;
        }

        IList<string> InternalFindMissing<T>(IEnumerable<T> data)
        {
            IList<string> results = new List<string>();
            foreach (T row in data)
            {
                var properties = StringAttributeRefectionUtils.ExtractAdornedProperties<FindMissingAttribute>(row);
                foreach (PropertyInfo propertyInfo in properties)
                {
                    var property = propertyInfo.GetValue(row, null);
                    if (property != null)
                    {
                        results.Add(property.ToString());
                    }
                }
            }
            return results;
        }
    }
}
