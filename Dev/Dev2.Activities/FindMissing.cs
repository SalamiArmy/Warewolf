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
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Dev2.Activities;
using Dev2.Activities.SelectAndApply;
using Dev2.Activities.Sharepoint;
using Dev2.Activities.WcfEndPoint;
using Dev2.Util;
using Dev2.Utilities;
using Unlimited.Applications.BusinessDesignStudio.Activities;

namespace Dev2.FindMissingStrategies
{
    public static class FindMissing
    {
        public static List<string> GetActivityFields(object activity) => new List<string>();

        public static List<string> GetActivityFields(DsfWcfEndPointActivity activity)
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

        public static List<string> GetActivityFields(DsfComDllActivity activity)
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

        public static List<string> GetActivityFields(DsfEnhancedDotNetDllActivity activity)
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

        public static List<string> GetActivityFields(DsfDotNetDllActivity activity)
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

        public static List<string> GetActivityFields(DsfWebGetActivity activity)
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

        public static List<string> GetActivityFields(DsfWebPutActivity activity)
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

        public static List<string> GetActivityFields(DsfWebDeleteActivity activity)
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

        public static List<string> GetActivityFields(DsfWebPostActivity activity)
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

        public static List<string> GetActivityFields(DsfODBCDatabaseActivity activity)
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

        public static List<string> GetActivityFields(DsfOracleDatabaseActivity activity)
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

        public static List<string> GetActivityFields(DsfPostgreSqlActivity activity)
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

        public static List<string> GetActivityFields(DsfMySqlDatabaseActivity activity)
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

        public static List<string> GetActivityFields(DsfSqlServerDatabaseActivity activity)
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

        public static List<string> GetActivityFields(DsfDotNetGatherSystemInformationActivity activity) => (List<string>)InternalFindMissing(activity.SystemInformationCollection);
        public static List<string> GetActivityFields(DsfGatherSystemInformationActivity activity) => (List<string>)InternalFindMissing(activity.SystemInformationCollection);
        public static List<string> GetActivityFields(DsfDotNetMultiAssignObjectActivity activity) => (List<string>)InternalFindMissing(activity.FieldsCollection);
        public static List<string> GetActivityFields(DsfDotNetMultiAssignActivity activity) => (List<string>)InternalFindMissing(activity.FieldsCollection);
        public static List<string> GetActivityFields(DsfMultiAssignObjectActivity activity) => (List<string>)InternalFindMissing(activity.FieldsCollection);
        public static List<string> GetActivityFields(DsfMultiAssignActivity activity) => (List<string>)InternalFindMissing(activity.FieldsCollection);
        public static List<string> GetActivityFields(DsfCaseConvertActivity activity) => (List<string>)InternalFindMissing(activity.ConvertCollection);
        public static List<string> GetActivityFields(DsfBaseConvertActivity activity) => (List<string>)InternalFindMissing(activity.ConvertCollection);

        public static List<string> GetActivityFields(DsfActivity act)
        {
            var results = new List<string>();
            if (!string.IsNullOrEmpty(act.ServiceName))
            {
                results.Add(act.ServiceName);
            }

            if (!string.IsNullOrEmpty(act.InputMapping))
            {
                var inputMappingElement = XElement.Parse(act.InputMapping);
                const string InputElement = "Input";
                var inputs = inputMappingElement.DescendantsAndSelf().Where(c => c.Name.ToString().Equals(InputElement, StringComparison.InvariantCultureIgnoreCase));

                results.AddRange(inputs.Select(element => element.Attribute("Source").Value).Where(val => !string.IsNullOrEmpty(val)));
            }

            if (!string.IsNullOrEmpty(act.OutputMapping))
            {
                var outputMappingElement = XElement.Parse(act.OutputMapping);
                const string OutputElement = "Output";
                var inputs = outputMappingElement.DescendantsAndSelf().Where(c => c.Name.ToString().Equals(OutputElement, StringComparison.InvariantCultureIgnoreCase));

                results.AddRange(inputs.Select(element => element.Attribute("Value").Value).Where(val => !string.IsNullOrEmpty(val)));
            }

            if (!string.IsNullOrEmpty(act.OnErrorVariable))
            {
                results.Add(act.OnErrorVariable);
            }

            if (!string.IsNullOrEmpty(act.OnErrorWorkflow))
            {
                results.Add(act.OnErrorWorkflow);
            }
            return results;
        }

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

        static IList<string> InternalFindMissing<T>(IEnumerable<T> data)
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

        public static List<string> GetActivityFields(DsfForEachActivity forEachActivity)
        {
            var results = new List<string>();
            enFindMissingType findMissingType;
            var boolAct = forEachActivity.DataFunc.Handler as DsfNativeActivity<bool>;
            if (boolAct == null)
            {
                if (forEachActivity.DataFunc.Handler is DsfNativeActivity<string> stringAct)
                {
                    findMissingType = stringAct.GetFindMissingType();
                    results.AddRange(GetActivityFields(stringAct));
                }
            }
            else
            {
                findMissingType = boolAct.GetFindMissingType();
                results.AddRange(GetActivityFields(boolAct));
            }
            return results;
        }

        public static List<string> GetActivityFields(DsfSelectAndApplyActivity selectAndApply)
        {
            var results = new List<string>();
            enFindMissingType findMissingType;
            var boolAct = selectAndApply.ApplyActivityFunc.Handler as DsfNativeActivity<bool>;
            if (boolAct == null)
            {
                if (selectAndApply.ApplyActivityFunc.Handler is DsfNativeActivity<string> stringAct)
                {
                    findMissingType = stringAct.GetFindMissingType();
                    results.AddRange(GetActivityFields(stringAct));
                }
            }
            else
            {
                findMissingType = boolAct.GetFindMissingType();
                results.AddRange(GetActivityFields(boolAct));
            }
            return results;
        }

        public static List<string> GetActivityFields(DsfSequenceActivity sequenceActivity)
        {
            var results = new List<string>();
            foreach (var innerActivity in sequenceActivity.Activities)
            {
                if (innerActivity is IDev2Activity dsfActivityAbstractString)
                {
                    results.AddRange(GetActivityFields(dsfActivityAbstractString));
                }
            }
            return results;
        }

        public static List<string> GetActivityFields(DsfNativeActivity<string> act)
        {
            var results = new List<string>();
            if (!string.IsNullOrEmpty(act.OnErrorVariable))
            {
                results.Add(act.OnErrorVariable);
            }

            if (!string.IsNullOrEmpty(act.OnErrorWorkflow))
            {
                results.Add(act.OnErrorWorkflow);
            }
            return results;
        }

        public static List<string> GetActivityFields(DsfFindRecordsMultipleCriteriaActivity frmAct)
        {
            var results = new List<string>();
            results.AddRange(InternalFindMissing(frmAct.ResultsCollection));
            if (!string.IsNullOrEmpty(frmAct.FieldsToSearch))
            {
                results.Add(frmAct.FieldsToSearch);
            }
            if (!string.IsNullOrEmpty(frmAct.Result))
            {
                results.Add(frmAct.Result);
            }
            return results;
        }

        public static List<string> GetActivityFields(DsfSqlBulkInsertActivity sbiAct)
        {
            var results = new List<string>();
            results.AddRange(InternalFindMissing(sbiAct.InputMappings));
            if (!string.IsNullOrEmpty(sbiAct.Result))
            {
                results.Add(sbiAct.Result);
            }
            return results;
        }

        public static List<string> GetActivityFields(DsfXPathActivity xpAct)
        {
            var results = new List<string>();
            results.AddRange(InternalFindMissing(xpAct.ResultsCollection));
            if (!string.IsNullOrEmpty(xpAct.SourceString))
            {
                results.Add(xpAct.SourceString);
            }
            return results;
        }

        public static List<string> GetActivityFields(DsfDataMergeActivity dmAct)
        {
            var results = new List<string>();
            results.AddRange(InternalFindMissing(dmAct.MergeCollection));
            if (!string.IsNullOrEmpty(dmAct.Result))
            {
                results.Add(dmAct.Result);
            }
            return results;
        }

        public static List<string> GetActivityFields(SharepointUpdateListItemActivity dsAct)
        {
            var results = new List<string>();
            results.AddRange(InternalFindMissing(dsAct.ReadListItems));
            results.AddRange(InternalFindMissing(dsAct.FilterCriteria));
            results.Add(dsAct.Result);
            return results;
        }

        public static List<string> GetActivityFields(SharepointDeleteListItemActivity sharepointDeleteListItemActivity)
        {
            var results = new List<string>();
            results.AddRange(InternalFindMissing(sharepointDeleteListItemActivity.FilterCriteria));
            results.Add(sharepointDeleteListItemActivity.DeleteCount);
            return results;
        }

        public static List<string> GetActivityFields(SharepointCreateListItemActivity sharepointCreateListItemActivity)
        {
            var results = new List<string>();
            results.AddRange(InternalFindMissing(sharepointCreateListItemActivity.ReadListItems));
            results.Add(sharepointCreateListItemActivity.Result);
            return results;
        }

        public static List<string> GetActivityFields(SharepointReadListActivity sharepointReadListActivity)
        {
            var results = new List<string>();
            results.AddRange(InternalFindMissing(sharepointReadListActivity.ReadListItems));
            if (sharepointReadListActivity.FilterCriteria != null)
            {
                results.AddRange(InternalFindMissing(sharepointReadListActivity.FilterCriteria));
            }
            return results;
        }

        public static List<string> GetActivityFields(DsfCreateJsonActivity createJsonActivity)
        {
            var results = new List<string>();
            results.AddRange(InternalFindMissing(createJsonActivity.JsonMappings));
            if (!string.IsNullOrEmpty(createJsonActivity.JsonString))
            {
                results.Add(createJsonActivity.JsonString);
            }
            return results;
        }

        public static List<string> GetActivityFields(DsfDataSplitActivity dataSplitActivity)
        {
            var results = new List<string>();
            results.AddRange(InternalFindMissing(dataSplitActivity.ResultsCollection));
            if (!string.IsNullOrEmpty(dataSplitActivity.SourceString))
            {
                results.Add(dataSplitActivity.SourceString);
            }
            return results;
        }
    }
}
