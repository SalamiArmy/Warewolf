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
            ForEachType = forEachType;
            IIndexIterator localIndexIterator;
            errors = AddErrors(forEachType, from, to, csvNumbers, numberOfExecutes, recordsetName);
            if (errors.HasErrors())
            {
                return;
            }

            switch (forEachType)
            {
                case enForEachType.InRecordset:
                    var records = compiler.EvalRecordSetIndexes(recordsetName, update);
                    if (!compiler.HasRecordSet(recordsetName))
                    {
                        errors.AddError("When selecting a recordset only valid recordsets can be used");
                        return;
                    }

                    localIndexIterator = new IndexListIndexIterator(records);

                    IndexIterator = localIndexIterator;
                    return;

                case enForEachType.InRange:
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
                case enForEachType.InCSV:
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
                default:
                    var numOfExItr = ExecutionEnvironment.WarewolfEvalResultToString(compiler.Eval(numberOfExecutes, update));

                    if (!int.TryParse(numOfExItr, out int intExNum) || intExNum < 1)
                    {
                        errors.AddError(string.Format(ErrorResource.RangeFromOne, "Number of executes"));
                    }
                    IndexIterator = new IndexIterator(new HashSet<int>(), intExNum);
                    return;
            }
        }

        static ErrorResultTO AddErrors(enForEachType forEachType, string from, string to, string csvNumbers, string numberOfExecutes, string recordsetName)
        {
            var errors = new ErrorResultTO();
            if (forEachType == enForEachType.InRecordset && string.IsNullOrEmpty(recordsetName))
            {
                errors.AddError(string.Format(ErrorResource.IsRequired, "The Recordset Field"));
            }
            if (string.IsNullOrWhiteSpace(@from))
            {
                errors.AddError(string.Format(ErrorResource.IsRequired, "The FROM field"));
            }
            if (string.IsNullOrWhiteSpace(to))
            {
                errors.AddError(string.Format(ErrorResource.IsRequired, "The TO field"));
            }
            if (@from.Contains("(*)"))
            {
                errors.AddError(string.Format(ErrorResource.StarNotationNotAllowed, "From field"));
            }
            if (string.IsNullOrEmpty(csvNumbers))
            {
                errors.AddError(string.Format(ErrorResource.IsRequired, "The CSV Field"));
            }
            if (numberOfExecutes != null && numberOfExecutes.Contains("(*)"))
            {
                errors.AddError(string.Format(ErrorResource.StarNotationNotAllowed, "Numbers field."));
            }
            return errors;
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