using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Dev2.Common;
using Dev2.Data.Interfaces;
using Dev2.Studio.Core.Factories;
using Dev2.Studio.Interfaces;
using Dev2.Studio.Interfaces.DataList;
using Dev2.Studio.ViewModels.DataList;

namespace Dev2.Studio.Core.DataList
{
    class ScalarHandler : IScalarHandler
    {
        readonly DataListViewModel _vm;

        public ScalarHandler(DataListViewModel dataListViewModel)
        {
            _vm = dataListViewModel;
        }

        #region Implementation of IScalarHandler

        public void FindMissingForScalar(IDataListVerifyPart part, List<IDataListVerifyPart> missingDataParts)
        {
            if (!part.IsScalar)
            {
                return;
            }

            if (_vm. ScalarCollection.Count(c => c.DisplayName == part.Field) == 0)
            {
                missingDataParts.Add(part);
            }
        }

        public void SetScalarItemsAsUsed()
        {
            foreach (var dataListItemModel in _vm.ScalarCollection.Where(model => !model.IsUsed))
            {
                dataListItemModel.IsUsed = true;
            }
        }

        public void AddScalars(XmlNode xmlNode)
        {
            if (xmlNode.Attributes != null)
            {
                var scalar = DataListItemModelFactory.CreateScalarItemModel(xmlNode.Name, Common.ParseDescription(xmlNode.Attributes[Common.Description]), Common.ParseColumnIODirection(xmlNode.Attributes[GlobalConstants.DataListIoColDirection]));
                if (scalar != null)
                {
                    AddScalars(xmlNode, scalar);
                }
            }
            else
            {
                var scalar = DataListItemModelFactory.CreateScalarItemModel(xmlNode.Name, Common.ParseDescription(null), Common.ParseColumnIODirection(null));
                if (scalar != null)
                {
                    AddScalars(xmlNode, scalar);
                }
            }
        }

        void AddScalars(XmlNode xmlNode, IScalarItemModel scalar)
        {
            scalar.IsEditable = Common.ParseIsEditable(xmlNode.Attributes[Common.IsEditable]);
            if (string.IsNullOrEmpty(_vm.SearchText))
            {
                _vm.ScalarCollection.Add(scalar);
            }
            else
            {
                if (scalar.DisplayName.ToUpper().StartsWith(_vm.SearchText.ToUpper()))
                {
                    _vm.ScalarCollection.Add(scalar);
                }
            }
        }

        public void SortScalars(bool ascending)
        {
            var newScalarCollection = @ascending
                ? _vm.ScalarCollection.Where(c => !c.IsBlank).OrderBy(c => c.DisplayName)
                : _vm.ScalarCollection.Where(c => !c.IsBlank).OrderByDescending(c => c.DisplayName);

            var iter = newScalarCollection.GetEnumerator();
            int i = 0;
            while (iter.MoveNext())
            {
                _vm.ScalarCollection.Move(_vm.ScalarCollection.IndexOf(iter.Current), i);
                i++;
            }
        }

        public void FixNamingForScalar(IDataListItemModel scalar)
        {
            if (scalar.DisplayName.Contains("()"))
            {
                scalar.DisplayName = scalar.DisplayName.Replace("()", "");
            }
            FixCommonNamingProblems(scalar);
        }

        public void AddRowToScalars()
        {
            var blankList = _vm.ScalarCollection.Where(c => c.IsBlank);
            if (blankList.Any())
            {
                return;
            }

            var scalar = DataListItemModelFactory.CreateScalarItemModel(string.Empty);
            _vm.ScalarCollection.Add(scalar);
        }

        public void RemoveBlankScalar()
        {
            var blankList = _vm.ScalarCollection.Where(c => c.IsBlank);
            if (blankList.Count() <= 1)
            {
                return;
            }

            _vm.ScalarCollection.Remove(blankList.First());
        }

        public void RemoveUnusedScalars()
        {
            var unusedScalars = _vm.ScalarCollection.Where(c => !c.IsUsed);
            if (unusedScalars.Any())
            {
                foreach (var dataListItemModel in unusedScalars)
                {
                    _vm.ScalarCollection.Remove(dataListItemModel);
                }
            }
        }

        public void AddMissingScalarParts(IDataListVerifyPart part)
        {
            if (_vm.ScalarCollection.FirstOrDefault(c => c.DisplayName == part.Field) == null)
            {
                var scalar = DataListItemModelFactory.CreateScalarItemModel(part.Field, part.Description);
                if (_vm.ScalarCollection.Count > 0)
                {
                    _vm.ScalarCollection.Insert(_vm.ScalarCollection.Count - 1, scalar);
                }
                else
                {
                    _vm.ScalarCollection.Add(scalar);
                }
            }
        }

        #endregion

        static void FixCommonNamingProblems(IDataListItemModel recset)
        {
            if (recset.DisplayName.Contains("[") || recset.DisplayName.Contains("]"))
            {
                recset.DisplayName = recset.DisplayName.Replace("[", "").Replace("]", "");
            }
        }

    }
}