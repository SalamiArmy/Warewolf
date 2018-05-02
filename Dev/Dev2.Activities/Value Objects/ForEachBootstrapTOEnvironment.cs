using System.Collections.Generic;
using System.Dynamic;
using Dev2.Data;
using Dev2.Data.Binary_Objects;
using Dev2.Data.Interfaces.Enums;
using Dev2.Data.TO;
using Warewolf.Resource.Errors;
using Warewolf.Storage;
using Warewolf.Storage.Interfaces;


namespace Unlimited.Applications.BusinessDesignStudio.Activities.Value_Objects
{
    public class ForEachBootstrapTO : DynamicObject
    {
        public enForEachExecutionType ExeType { get; set; }
        public int MaxExecutions { get; set; }
        public int IterationCount { get; set; }
        public ForEachInnerActivityTO InnerActivity { get; set; }
        public IIndexIterator IndexIterator { get; set; }
        public enForEachType ForEachType { get; private set; }
        
        public ForEachBootstrapTO(enForEachType forEachType, string from, string to, string csvNumbers, string numberOfExecutes, string recordsetName, IExecutionEnvironment compiler, out ErrorResultTO errors, int update)
        {
            errors = new ErrorResultTO();
            ForEachType = forEachType;
            IIndexIterator localIndexIterator;
            if (MissingRecordsetField(forEachType, recordsetName, errors) || MissingRecordset(forEachType, recordsetName, compiler, errors))
            {
                return;
            }
            
            if (forEachType == enForEachType.InRecordset)
            {
                var records = compiler.EvalRecordSetIndexes(recordsetName, update);
                localIndexIterator = new IndexListIndexIterator(records);
                IndexIterator = localIndexIterator;
                return;
            }

            if (FromFieldMissing(forEachType, from, errors) || ToFieldIsMissing(forEachType, to, errors) || StarNotationInFromField(forEachType, from, errors))
            {
                return;
            }

            if (forEachType == enForEachType.InRange)
            {
                var evalledFrom = ExecutionEnvironment.WarewolfEvalResultToString(compiler.Eval(@from, update));
                int intFrom;
                if (!int.TryParse(evalledFrom, out intFrom) || intFrom < 1)
                {
                    errors.AddError(string.Format(ErrorResource.RangeFromOne, "FROM range"));
                    return;
                }
                if (to.Contains("(*)"))
                {
                    errors.AddError(string.Format(ErrorResource.StarNotationNotAllowed, "TO field."));
                    return;
                }
                var evalledTo = ExecutionEnvironment.WarewolfEvalResultToString(compiler.Eval(@to, update));
                int intTo;
                if (!int.TryParse(evalledTo, out intTo) || intTo < 1)
                {
                    errors.AddError(string.Format(ErrorResource.RangeFromOne, "TO range"));
                    return;
                }
                IndexList indexList;
                if (intFrom > intTo)
                {
                    indexList = new IndexList(new HashSet<int>(), 0) { MinValue = intFrom, MaxValue = intTo };
                    var revIdxItr = new ReverseIndexIterator(new HashSet<int>(), 0) { IndexList = indexList };
                    IndexIterator = revIdxItr;
                }
                else
                {
                    indexList = new IndexList(new HashSet<int>(), 0) { MinValue = intFrom, MaxValue = intTo };
                    localIndexIterator = new IndexIterator(new HashSet<int>(), 0) { IndexList = indexList };
                    IndexIterator = localIndexIterator;
                }

                return;
            }

            if (forEachType == enForEachType.InCSV)
            {
                if (string.IsNullOrEmpty(csvNumbers))
                {
                    errors.AddError(string.Format(ErrorResource.IsRequired, "The CSV Field"));
                    return;
                }
                var csvIndexedsItr = ExecutionEnvironment.WarewolfEvalResultToString(compiler.Eval(csvNumbers, update));

                ErrorResultTO allErrors;
                var listOfIndexes = SplitOutCsvIndexes(csvIndexedsItr, out allErrors);
                if (allErrors.HasErrors())
                {
                    errors.MergeErrors(allErrors);
                    return;
                }
                var listLocalIndexIterator = new ListIndexIterator(listOfIndexes);
                var listOfIndex = new ListOfIndex(listOfIndexes);
                listLocalIndexIterator.IndexList = listOfIndex;
                IndexIterator = listLocalIndexIterator;
                return;
            }

            if (forEachType != enForEachType.InRecordset && forEachType != enForEachType.InRange && forEachType != enForEachType.InCSV)
            {
                if (numberOfExecutes != null && numberOfExecutes.Contains("(*)"))
                {
                    errors.AddError(string.Format(ErrorResource.StarNotationNotAllowed, "Numbers field."));
                    return;
                }

                int intExNum;
                var numOfExItr = ExecutionEnvironment.WarewolfEvalResultToString(compiler.Eval(numberOfExecutes, update));

                if (!int.TryParse(numOfExItr, out intExNum) || intExNum < 1)
                {
                    errors.AddError(string.Format(ErrorResource.RangeFromOne, "Number of executes"));
                }
                IndexIterator = new IndexIterator(new HashSet<int>(), intExNum);
            }
        }

        static bool StarNotationInFromField(enForEachType forEachType, string from, ErrorResultTO errors)
        {
            bool StarNotationInFromField = forEachType == enForEachType.InRange && @from.Contains("(*)");
            if (StarNotationInFromField)
            {
                errors.AddError(string.Format(ErrorResource.StarNotationNotAllowed, "From field"));
            }

            return StarNotationInFromField;
        }

        static bool ToFieldIsMissing(enForEachType forEachType, string to, ErrorResultTO errors)
        {
            bool ToFieldIsMissing = forEachType == enForEachType.InRange && string.IsNullOrWhiteSpace(to);
            if (ToFieldIsMissing)
            {
                errors.AddError(string.Format(ErrorResource.IsRequired, "The TO field"));
            }

            return ToFieldIsMissing;
        }

        static bool FromFieldMissing(enForEachType forEachType, string from, ErrorResultTO errors)
        {
            bool FromFieldMissing = forEachType == enForEachType.InRange && string.IsNullOrWhiteSpace(@from);
            if (FromFieldMissing)
            {
                errors.AddError(string.Format(ErrorResource.IsRequired, "The FROM field"));
            }

            return FromFieldMissing;
        }

        static bool MissingRecordset(enForEachType forEachType, string recordsetName, IExecutionEnvironment compiler, ErrorResultTO errors)
        {
            bool MissingRecordset = forEachType == enForEachType.InRecordset && !compiler.HasRecordSet(recordsetName);
            if (MissingRecordset)
            {
                errors.AddError("When selecting a recordset only valid recordsets can be used");
            }

            return MissingRecordset;
        }

        static bool MissingRecordsetField(enForEachType forEachType, string recordsetName, ErrorResultTO errors)
        {
            bool MissingRecordsetField = forEachType == enForEachType.InRecordset && string.IsNullOrEmpty(recordsetName);
            if (MissingRecordsetField)
            {
                errors.AddError(string.Format(ErrorResource.IsRequired, "The Recordset Field"));
            }

            return MissingRecordsetField;
        }

        List<int> SplitOutCsvIndexes(string csvNumbers, out ErrorResultTO errors)
        {
            errors = new ErrorResultTO();
            var result = new List<int>();
            var splitStrings = csvNumbers.Split(',');
            foreach(var splitString in splitStrings)
            {
                if(!string.IsNullOrEmpty(splitString))
                {
                    if (int.TryParse(splitString, out int index))
                    {
                        result.Add(index);
                    }
                    else
                    {
                        errors.AddError(ErrorResource.CSVInvalidCharecters);
                        return result;
                    }
                }
            }

            return result;
        }

        public void IncIterationCount()
        {
            IterationCount++;
        }
    }
}