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

        public List<string> GetActivityFields(object activity) => new List<string>();

        public List<string> GetActivityFields(DsfWcfEndPointActivity activity)
        {
            var results = new List<string>();
            if (activity is DsfWcfEndPointActivity maAct)
            {
                if (maAct.Inputs != null)
                {
                    results.AddRange(InternalFindMissing(maAct.Inputs));
                }

                if (!string.IsNullOrEmpty(maAct.OnErrorVariable))
                {
                    results.Add(maAct.OnErrorVariable);
                }
                if (maAct.IsObject)
                {
                    if (!string.IsNullOrEmpty(maAct.ObjectName))
                    {
                        results.Add(maAct.ObjectName);
                    }
                }
                else
                {

                    if (maAct.Outputs != null)
                    {
                        results.AddRange(InternalFindMissing(maAct.Outputs));
                    }
                }
                if (!string.IsNullOrEmpty(maAct.OnErrorWorkflow))
                {
                    results.Add(maAct.OnErrorWorkflow);
                }
            }
            return results;
        }

        public List<string> GetActivityFields(DsfComDllActivity activity)
        {
            var results = new List<string>();
            if (activity is DsfComDllActivity maAct)
            {
                if (maAct.Inputs != null)
                {
                    results.AddRange(InternalFindMissing(maAct.Inputs));
                }
                if (!string.IsNullOrEmpty(maAct.OnErrorVariable))
                {
                    results.Add(maAct.OnErrorVariable);
                }
                if (maAct.IsObject)
                {
                    if (!string.IsNullOrEmpty(maAct.ObjectName))
                    {
                        results.Add(maAct.ObjectName);
                    }
                }
                else
                {
                    if (maAct.Outputs != null)
                    {
                        results.AddRange(InternalFindMissing(maAct.Outputs));
                    }
                }
                if (!string.IsNullOrEmpty(maAct.OnErrorWorkflow))
                {
                    results.Add(maAct.OnErrorWorkflow);
                }
            }
            return results;
        }

        public List<string> GetActivityFields(DsfEnhancedDotNetDllActivity activity)
        {
            var results = new List<string>();
            if (activity is DsfEnhancedDotNetDllActivity maAct)
            {
                if (maAct.ConstructorInputs != null)
                {
                    results.AddRange(InternalFindMissing(maAct.ConstructorInputs));
                }
                if (!string.IsNullOrEmpty(maAct.OnErrorVariable))
                {
                    results.Add(maAct.OnErrorVariable);
                }

                if (maAct.MethodsToRun != null)
                {
                    foreach (var pluginAction in maAct.MethodsToRun)
                    {
                        AddMethodsToRun(results, pluginAction);
                    }
                }
                if (maAct.IsObject)
                {
                    if (!string.IsNullOrEmpty(maAct.ObjectName))
                    {
                        results.Add(maAct.ObjectName);
                    }
                }
                else
                {

                    if (maAct.Outputs != null)
                    {
                        results.AddRange(InternalFindMissing(maAct.Outputs));
                    }
                }
                if (!string.IsNullOrEmpty(maAct.OnErrorWorkflow))
                {
                    results.Add(maAct.OnErrorWorkflow);
                }
            }
            return results;
        }

        static void AddMethodsToRun(List<string> results, Common.Interfaces.IPluginAction pluginAction)
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

        public List<string> GetActivityFields(DsfDotNetDllActivity activity)
        {
            var results = new List<string>();
            if (activity is DsfDotNetDllActivity maAct)
            {
                if (maAct.Inputs != null)
                {
                    results.AddRange(InternalFindMissing(maAct.Inputs));
                }
                if (!string.IsNullOrEmpty(maAct.OnErrorVariable))
                {
                    results.Add(maAct.OnErrorVariable);
                }
                if (maAct.IsObject)
                {
                    if (!string.IsNullOrEmpty(maAct.ObjectName))
                    {
                        results.Add(maAct.ObjectName);
                    }
                }
                else
                {

                    if (maAct.Outputs != null)
                    {
                        results.AddRange(InternalFindMissing(maAct.Outputs));
                    }
                }
                if (!string.IsNullOrEmpty(maAct.OnErrorWorkflow))
                {
                    results.Add(maAct.OnErrorWorkflow);
                }
            }
            return results;
        }

        public List<string> GetActivityFields(DsfWebGetActivity activity)
        {
            var results = new List<string>();
            if (activity is DsfWebGetActivity maAct)
            {
                if (maAct.Inputs != null)
                {
                    results.AddRange(InternalFindMissing(maAct.Inputs));
                }
                if (maAct.QueryString != null)
                {
                    results.Add(maAct.QueryString);
                }
                if (maAct.Headers != null)
                {
                    foreach (var nameValue in maAct.Headers)
                    {
                        results.Add(nameValue.Name);
                        results.Add(nameValue.Value);
                    }
                }
                if (!string.IsNullOrEmpty(maAct.ObjectName))
                {
                    results.Add(maAct.ObjectName);
                }
                if (!string.IsNullOrEmpty(maAct.OnErrorVariable))
                {
                    results.Add(maAct.OnErrorVariable);
                }
                if (maAct.IsObject)
                {
                    if (!string.IsNullOrEmpty(maAct.ObjectName))
                    {
                        results.Add(maAct.ObjectName);
                    }
                }
                else
                {

                    if (maAct.Outputs != null)
                    {
                        results.AddRange(InternalFindMissing(maAct.Outputs));
                    }
                }
                if (!string.IsNullOrEmpty(maAct.OnErrorWorkflow))
                {
                    results.Add(maAct.OnErrorWorkflow);
                }
            }
            return results;
        }

        public List<string> GetActivityFields(DsfWebPutActivity activity)
        {
            var results = new List<string>();
            if (activity is DsfWebPutActivity maAct)
            {
                if (maAct.Inputs != null)
                {
                    results.AddRange(InternalFindMissing(maAct.Inputs));
                }
                if (maAct.QueryString != null)
                {
                    results.Add(maAct.QueryString);
                }
                if (maAct.PutData != null)
                {
                    results.Add(maAct.PutData);
                }
                if (maAct.Headers != null)
                {
                    foreach (var nameValue in maAct.Headers)
                    {
                        results.Add(nameValue.Name);
                        results.Add(nameValue.Value);
                    }
                }
                if (!string.IsNullOrEmpty(maAct.OnErrorVariable))
                {
                    results.Add(maAct.OnErrorVariable);
                }
                if (maAct.IsObject)
                {
                    if (!string.IsNullOrEmpty(maAct.ObjectName))
                    {
                        results.Add(maAct.ObjectName);
                    }
                }
                else
                {

                    if (maAct.Outputs != null)
                    {
                        results.AddRange(InternalFindMissing(maAct.Outputs));
                    }
                }
                if (!string.IsNullOrEmpty(maAct.OnErrorWorkflow))
                {
                    results.Add(maAct.OnErrorWorkflow);
                }
            }
            return results;
        }

        public List<string> GetActivityFields(DsfWebDeleteActivity activity)
        {
            var results = new List<string>();
            if (activity is DsfWebDeleteActivity maAct)
            {
                if (maAct.Inputs != null)
                {
                    results.AddRange(InternalFindMissing(maAct.Inputs));
                }
                if (maAct.QueryString != null)
                {
                    results.Add(maAct.QueryString);
                }
                if (maAct.Headers != null)
                {
                    foreach (var nameValue in maAct.Headers)
                    {
                        results.Add(nameValue.Name);
                        results.Add(nameValue.Value);
                    }
                }
                if (!string.IsNullOrEmpty(maAct.OnErrorVariable))
                {
                    results.Add(maAct.OnErrorVariable);
                }
                if (maAct.IsObject)
                {
                    if (!string.IsNullOrEmpty(maAct.ObjectName))
                    {
                        results.Add(maAct.ObjectName);
                    }
                }
                else
                {

                    if (maAct.Outputs != null)
                    {
                        results.AddRange(InternalFindMissing(maAct.Outputs));
                    }
                }
                if (!string.IsNullOrEmpty(maAct.OnErrorWorkflow))
                {
                    results.Add(maAct.OnErrorWorkflow);
                }
            }
            return results;
        }

        public List<string> GetActivityFields(DsfWebPostActivity activity)
        {
            var results = new List<string>();
            if (activity is DsfWebPostActivity maAct)
            {
                if (maAct.Inputs != null)
                {
                    results.AddRange(InternalFindMissing(maAct.Inputs));
                }

                if (maAct.QueryString != null)
                {
                    results.Add(maAct.QueryString);
                }
                if (maAct.PostData != null)
                {
                    results.Add(maAct.PostData);
                }
                if (maAct.Headers != null)
                {
                    results.AddRange(AddAllHeaders(maAct));
                }
                if (!string.IsNullOrEmpty(maAct.OnErrorVariable))
                {
                    results.Add(maAct.OnErrorVariable);
                }
                if (!string.IsNullOrEmpty(maAct.OnErrorWorkflow))
                {
                    results.Add(maAct.OnErrorWorkflow);
                }
                if (maAct.IsObject)
                {
                    if (!string.IsNullOrEmpty(maAct.ObjectName))
                    {
                        results.Add(maAct.ObjectName);
                    }
                }
                else
                {
                    if (maAct.Outputs != null)
                    {
                        results.AddRange(InternalFindMissing(maAct.Outputs));
                    }
                }
            }
            return results;
        }

        public List<string> GetActivityFields(DsfODBCDatabaseActivity activity)
        {
            var results = new List<string>();
            if (activity is DsfODBCDatabaseActivity maAct)
            {
                if (maAct.Inputs != null)
                {
                    results.AddRange(InternalFindMissing(maAct.Inputs));
                }
                if (maAct.CommandText != null)
                {
                    results.Add(maAct.CommandText);
                }
                if (maAct.Outputs != null)
                {
                    results.AddRange(InternalFindMissing(maAct.Outputs));
                }
                if (!string.IsNullOrEmpty(maAct.OnErrorVariable))
                {
                    results.Add(maAct.OnErrorVariable);
                }

                if (!string.IsNullOrEmpty(maAct.OnErrorWorkflow))
                {
                    results.Add(maAct.OnErrorWorkflow);
                }
            }
            return results;
        }

        public List<string> GetActivityFields(DsfOracleDatabaseActivity activity)
        {
            var results = new List<string>();
            if (activity is DsfOracleDatabaseActivity maAct)
            {
                if (maAct.Inputs != null)
                {
                    results.AddRange(InternalFindMissing(maAct.Inputs));
                }
                if (maAct.Outputs != null)
                {
                    results.AddRange(InternalFindMissing(maAct.Outputs));
                }
                if (!string.IsNullOrEmpty(maAct.OnErrorVariable))
                {
                    results.Add(maAct.OnErrorVariable);
                }

                if (!string.IsNullOrEmpty(maAct.OnErrorWorkflow))
                {
                    results.Add(maAct.OnErrorWorkflow);
                }
            }
            return results;
        }

        public List<string> GetActivityFields(DsfPostgreSqlActivity activity)
        {
            var results = new List<string>();
            if (activity is DsfPostgreSqlActivity maAct)
            {
                if (maAct.Inputs != null)
                {
                    results.AddRange(InternalFindMissing(maAct.Inputs));
                }
                if (maAct.Outputs != null)
                {
                    results.AddRange(InternalFindMissing(maAct.Outputs));
                }
                if (!string.IsNullOrEmpty(maAct.OnErrorVariable))
                {
                    results.Add(maAct.OnErrorVariable);
                }

                if (!string.IsNullOrEmpty(maAct.OnErrorWorkflow))
                {
                    results.Add(maAct.OnErrorWorkflow);
                }
            }
            return results;
        }

        public List<string> GetActivityFields(DsfMySqlDatabaseActivity activity)
        {
            var results = new List<string>();
            if (activity is DsfMySqlDatabaseActivity maAct)
            {
                if (maAct.Inputs != null)
                {
                    results.AddRange(InternalFindMissing(maAct.Inputs));
                }
                if (maAct.Outputs != null)
                {
                    results.AddRange(InternalFindMissing(maAct.Outputs));
                }
                if (!string.IsNullOrEmpty(maAct.OnErrorVariable))
                {
                    results.Add(maAct.OnErrorVariable);
                }

                if (!string.IsNullOrEmpty(maAct.OnErrorWorkflow))
                {
                    results.Add(maAct.OnErrorWorkflow);
                }
            }
            return results;
        }

        public List<string> GetActivityFields(DsfSqlServerDatabaseActivity activity)
        {
            var results = new List<string>();
            if (activity is DsfSqlServerDatabaseActivity maAct)
            {
                if (maAct.Inputs != null)
                {
                    results.AddRange(InternalFindMissing(maAct.Inputs));
                }
                if (maAct.Outputs != null)
                {
                    results.AddRange(InternalFindMissing(maAct.Outputs));
                }
                if (!string.IsNullOrEmpty(maAct.OnErrorVariable))
                {
                    results.Add(maAct.OnErrorVariable);
                }

                if (!string.IsNullOrEmpty(maAct.OnErrorWorkflow))
                {
                    results.Add(maAct.OnErrorWorkflow);
                }
            }
            return results;
        }

        public List<string> GetActivityFields(DsfDotNetGatherSystemInformationActivity activity) => (List<string>)InternalFindMissing(activity.SystemInformationCollection);
        public List<string> GetActivityFields(DsfGatherSystemInformationActivity activity) => (List<string>)InternalFindMissing(activity.SystemInformationCollection);
        public List<string> GetActivityFields(DsfDotNetMultiAssignObjectActivity activity) => (List<string>)InternalFindMissing(activity.FieldsCollection);
        public List<string> GetActivityFields(DsfDotNetMultiAssignActivity activity) => (List<string>)InternalFindMissing(activity.FieldsCollection);
        public List<string> GetActivityFields(DsfMultiAssignObjectActivity activity) => (List<string>)InternalFindMissing(activity.FieldsCollection);
        public List<string> GetActivityFields(DsfMultiAssignActivity activity) => (List<string>)InternalFindMissing(activity.FieldsCollection);
        public List<string> GetActivityFields(DsfCaseConvertActivity activity) => (List<string>)InternalFindMissing(activity.ConvertCollection);
        public List<string> GetActivityFields(DsfBaseConvertActivity activity) => (List<string>)InternalFindMissing(activity.ConvertCollection);

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
