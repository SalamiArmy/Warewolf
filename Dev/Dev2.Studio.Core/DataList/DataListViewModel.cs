/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2018 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using Caliburn.Micro;
using Dev2.Common;
using Dev2.Common.Common;
using Dev2.Common.Interfaces;
using Dev2.Common.Utils;
using Dev2.Data;
using Dev2.Data.Interfaces;
using Dev2.Data.Util;
using Dev2.DataList.Contract;
using Dev2.Runtime.Configuration.ViewModels.Base;
using Dev2.Services.Events;
using Dev2.Studio.Core;
using Dev2.Studio.Core.DataList;
using Dev2.Studio.Core.Factories;
using Dev2.Studio.Core.Models.DataList;
using Dev2.Studio.Core.ViewModels.Base;
using ServiceStack.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Xml;
using Dev2.Data.TO;
using Dev2.Studio.Core.Equality;
using Dev2.Studio.Interfaces;
using Dev2.Studio.Interfaces.DataList;
using Warewolf.Resource.Errors;
using Warewolf.Storage;


namespace Dev2.Studio.ViewModels.DataList
{
    public class DataListViewModel : BaseViewModel, IDataListViewModel, IUpdatesHelp
    {
        readonly IComplexObjectHandler _complexObjectHandler;
        readonly IScalarHandler _scalarHandler;
        readonly IRecordsetHandler _recordsetHandler;
        readonly IDataListViewModelHelper _helper;
        ObservableCollection<DataListHeaderItemModel> _baseCollection;
        RelayCommand _findUnusedAndMissingDataListItems;
        ObservableCollection<IRecordSetItemModel> _recsetCollection;
        ObservableCollection<IScalarItemModel> _scalarCollection;
        string _searchText;
        RelayCommand _sortCommand;
        RelayCommand _deleteCommand;
        RelayCommand _viewComplexObjectsCommand;
        bool _viewSortDelete;

        readonly IMissingDataList _missingDataList;
        readonly IPartIsUsed _partIsUsed;

        public bool CanSortItems => HasItems();

        public ObservableCollection<DataListHeaderItemModel> BaseCollection
        {
            get => _baseCollection;
            set
            {
                _baseCollection = value;
                NotifyOfPropertyChange(() => BaseCollection);
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (!string.Equals(_searchText, value, StringComparison.InvariantCultureIgnoreCase))
                {
                    _searchText = value;
                    NotifyOfPropertyChange(() => SearchText);
                    FilterCollection(_searchText);
                }
            }
        }

        void FilterCollection(string searchText)
        {
            if (_scalarCollection != null && _scalarCollection.Count > 1)
            {
                for (int index = _scalarCollection.Count - 1; index >= 0; index--)
                {
                    var scalarItemModel = _scalarCollection[index];
                    scalarItemModel.Filter(searchText);
                }
            }
            if (_recsetCollection != null && _recsetCollection.Count > 1)
            {
                for (int index = _recsetCollection.Count - 1; index >= 0; index--)
                {
                    var recordSetItemModel = _recsetCollection[index];
                    recordSetItemModel.Filter(searchText);
                }
            }
            if (_complexObjectCollection != null && _complexObjectCollection.Count > 0)
            {
                for (int index = _complexObjectCollection.Count - 1; index >= 0; index--)
                {
                    var complexObjectItemModel = _complexObjectCollection[index];
                    complexObjectItemModel.Filter(searchText);
                }
            }
            NotifyOfPropertyChange(() => ScalarCollection);
            NotifyOfPropertyChange(() => RecsetCollection);
            NotifyOfPropertyChange(() => ComplexObjectCollection);
        }

        public bool ViewSortDelete
        {
            get => _viewSortDelete;
            set
            {
                _viewSortDelete = value;
                NotifyOfPropertyChange(() => ViewSortDelete);
            }
        }

        public IResourceModel Resource { get; private set; }

        public ObservableCollection<IDataListItemModel> DataList => CreateFullDataList();

        public bool HasErrors
        {
            get
            {
                if (DataList == null || DataList.Count == 0)
                {
                    return false;
                }
                var recSetsHasErrors = RecsetCollection.Any(model => model.HasError || (model.Children != null && model.Children.Any(itemModel => itemModel.HasError)));
                var complexObjectHasErrors = ComplexObjectCollection.Any(model => model.HasError || (model.Children != null && model.Children.Any(itemModel => itemModel.HasError)));
                var scalasHasErrors = ScalarCollection.Any(model => model.HasError);
                var hasErrors = recSetsHasErrors || scalasHasErrors || complexObjectHasErrors;
                return hasErrors;
            }
        }

        public ObservableCollection<IScalarItemModel> ScalarCollection
        {
            get
            {
                if (_scalarCollection != null)
                {
                    if (string.IsNullOrEmpty(_searchText))
                    {
                        return _scalarCollection;
                    }
                    return _scalarCollection.Where(model => model.IsVisible).ToObservableCollection();
                }
                _scalarCollection = new ObservableCollection<IScalarItemModel>();
                _scalarCollection.CollectionChanged += OnScalarCollectionOnCollectionChanged;
                return _scalarCollection;
            }
        }

        void OnScalarCollectionOnCollectionChanged(object o, NotifyCollectionChangedEventArgs args)
        {
            RemoveItemPropertyChangeEvent(args);
            AddItemPropertyChangeEvent(args);
        }

        public ObservableCollection<IComplexObjectItemModel> ComplexObjectCollection
        {
            get
            {
                if (_complexObjectCollection != null)
                {
                    if (string.IsNullOrEmpty(_searchText))
                    {
                        return _complexObjectCollection;
                    }
                    return _complexObjectCollection.Where(model => model.IsVisible).ToObservableCollection();
                }
                _complexObjectCollection = new ObservableCollection<IComplexObjectItemModel>();

                _complexObjectCollection.CollectionChanged += (o, args) =>
                {
                    RemoveItemPropertyChangeEvent(args);
                    AddItemPropertyChangeEvent(args);

                };
                return _complexObjectCollection;
            }
        }

        void AddItemPropertyChangeEvent(NotifyCollectionChangedEventArgs args)
        {
            if (args.NewItems == null)
            {
                return;
            }

            foreach (INotifyPropertyChanged item in args.NewItems)
            {
                if (item != null)
                {
                    item.PropertyChanged += ItemPropertyChanged;
                }
            }
        }

        void RemoveItemPropertyChangeEvent(NotifyCollectionChangedEventArgs args)
        {
            if (args.OldItems == null)
            {
                return;
            }

            foreach (INotifyPropertyChanged item in args.OldItems)
            {
                if (item != null)
                {
                    item.PropertyChanged -= ItemPropertyChanged;
                }
            }
        }

        void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (FindUnusedAndMissingCommand != null)
            {
                FindUnusedAndMissingCommand.RaiseCanExecuteChanged();
                SortCommand.RaiseCanExecuteChanged();
            }

            ViewComplexObjectsCommand?.RaiseCanExecuteChanged();
            DeleteCommand?.RaiseCanExecuteChanged();
        }

        public ObservableCollection<IRecordSetItemModel> RecsetCollection
        {
            get
            {
                if (_recsetCollection == null)
                {
                    _recsetCollection = new ObservableCollection<IRecordSetItemModel>();
                    _recsetCollection.CollectionChanged += (o, args) =>
                    {
                        RemoveItemPropertyChangeEvent(args);
                        AddItemPropertyChangeEvent(args);
                    };
                }

                if (string.IsNullOrEmpty(_searchText))
                {
                    return _recsetCollection;
                }
                var recordSetItemModels = _recsetCollection.Where(model => model.IsVisible).ToObservableCollection();
                return recordSetItemModels;
            }
        }

        bool _toggleSortOrder = true;
        ObservableCollection<IComplexObjectItemModel> _complexObjectCollection;

        public DataListViewModel()
            : this(EventPublishers.Aggregator)
        {
        }

        public DataListViewModel(IEventAggregator eventPublisher)
            : base(eventPublisher)
        {
            ClearSearchTextCommand = new Microsoft.Practices.Prism.Commands.DelegateCommand(() => SearchText = "");
            ViewSortDelete = true;
            Provider = new Dev2TrieSugggestionProvider();
            _missingDataList = new MissingDataList(RecsetCollection, ScalarCollection);
            _partIsUsed = new PartIsUsed(RecsetCollection, ScalarCollection, ComplexObjectCollection);
            _complexObjectHandler = new ComplexObjectHandler(this);
            _scalarHandler = new ScalarHandler(this);
            _recordsetHandler = new RecordsetHandler(this);
            _helper = new DataListViewModelHelper(this);
        }

        public IJsonObjectsView JsonObjectsView => CustomContainer.GetInstancePerRequestType<IJsonObjectsView>();

        void ViewJsonObjects(IComplexObjectItemModel item)
        {
            if (item != null && JsonObjectsView != null)
            {
                var json = item.GetJson();
                JsonObjectsView.ShowJsonString(JSONUtils.Format(json));
            }

        }

        bool CanViewComplexObjects(Object itemx)
        {
            var item = itemx as IDataListItemModel;
            return item != null && !item.IsComplexObject;
        }

        bool CanDelete(Object itemx)
        {
            var item = itemx as IDataListItemModel;
            return item != null && !item.IsUsed;
        }

        public ICommand ClearSearchTextCommand { get; private set; }

        public RelayCommand SortCommand => _sortCommand ??
                       (_sortCommand = new RelayCommand(method => SortItems(), p => CanSortItems));

        public IRelayCommand FindUnusedAndMissingCommand => _findUnusedAndMissingDataListItems ??
                       (_findUnusedAndMissingDataListItems = new RelayCommand(method => RemoveUnusedDataListItems(), o => HasAnyUnusedItems()));

        public RelayCommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new RelayCommand(item =>
                                                           {
                                                               RemoveDataListItem(item as IDataListItemModel);
                                                               WriteToResourceModel();
                                                               FindUnusedAndMissingCommand.RaiseCanExecuteChanged();
                                                               DeleteCommand.RaiseCanExecuteChanged();
                                                               UpdateIntellisenseList();
                                                           }, CanDelete));

        public RelayCommand ViewComplexObjectsCommand => _viewComplexObjectsCommand ?? (_viewComplexObjectsCommand = new RelayCommand(item =>
                                                                       {
                                                                           ViewJsonObjects(item as IComplexObjectItemModel);
                                                                       }, CanViewComplexObjects));

        public void SetIsUsedDataListItems(IEnumerable<IDataListVerifyPart> parts, bool isUsed)
        {
            foreach (var part in parts)
            {
                if (part.IsScalar)
                {
                    _partIsUsed.SetScalarPartIsUsed(part, isUsed);
                }
                else
                {
                    _partIsUsed.SetRecordSetPartIsUsed(part, isUsed);
                }
                if (part.IsJson)
                {
                    _partIsUsed.SetComplexObjectSetPartIsUsed(part, isUsed);
                }
            }
        }

        public void RemoveUnusedDataListItems()
        {
            _scalarHandler.RemoveUnusedScalars();
            _recordsetHandler.RemoveUnusedRecordSets();
            _complexObjectHandler.RemoveUnusedComplexObjects();

            WriteToResourceModel();
            FindUnusedAndMissingCommand.RaiseCanExecuteChanged();
            ViewComplexObjectsCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
            UpdateIntellisenseList();
        }

        public void AddMissingDataListItems(IEnumerable<IDataListVerifyPart> parts)
        {
            var tmpRecsetList = new List<IRecordSetItemModel>();
            foreach (var part in parts)
            {
                if (part.IsJson)
                {
                    _complexObjectHandler.AddComplexObject(part);
                }
                else
                {
                    if (part.IsScalar)
                    {
                        _scalarHandler.AddMissingScalarParts(part);
                    }
                    else
                    {
                        AddMissingRecsetParts(tmpRecsetList, part);
                    }
                }
            }
            _recordsetHandler.AddMissingTempRecordSetList(tmpRecsetList);
            _scalarHandler.RemoveBlankScalar();
            _recordsetHandler.RemoveBlankRecordsets();
            _recordsetHandler.RemoveBlankFieldFromRecordsets();
            _complexObjectHandler.RemoveBlankComplexObjects();
            if (parts.Any())
            {
                AddBlankRow(null);
            }
            UpdateIntellisenseList();
            WriteToResourceModel();
        }

        private void AddMissingRecsetParts(List<IRecordSetItemModel> tmpRecsetList, IDataListVerifyPart part)
        {
            var recsetToAddTo = RecsetCollection.FirstOrDefault(c => c.DisplayName == part.Recordset);

            var tmpRecset = tmpRecsetList.FirstOrDefault(c => c.DisplayName == part.Recordset);

            if (recsetToAddTo != null)
            {
                _recordsetHandler.AddMissingRecordSetPart(recsetToAddTo, part);
            }
            else if (tmpRecset != null)
            {
                _recordsetHandler.AddMissingTempRecordSet(part, tmpRecset);
            }
            else
            {
                var recset = DataListItemModelFactory.CreateRecordSetItemModel(part.Recordset, part.Description);
                tmpRecsetList.Add(recset);
            }
        }

        void UpdateIntellisenseList()
        {
            if (_scalarCollection != null && _recsetCollection != null && _complexObjectCollection != null && _complexObjectHandler != null)
            {
                var items = RefreshTries(_scalarCollection, new List<string>()).Union(RefreshRecordSets(_recsetCollection, new List<string>())).Union(_complexObjectHandler.RefreshJsonObjects(_complexObjectCollection));
                if (Provider != null)
                {
                    Provider.VariableList = new ObservableCollection<string>(items);
                }
            }
        }

        public void InitializeDataListViewModel(IResourceModel resourceModel)
        {
            Resource = resourceModel;
            if (Resource == null)
            {
                return;
            }

            CreateListsOfIDataListItemModelToBindTo(out string errorString);
            if (!string.IsNullOrEmpty(errorString))
            {
                throw new Exception(errorString);
            }
            var items = RefreshTries(_scalarCollection, new List<string>()).Union(RefreshRecordSets(_recsetCollection, new List<string>())).Union(_complexObjectHandler.RefreshJsonObjects(_complexObjectCollection));
            Provider.VariableList = new ObservableCollection<string>(items);
        }

        IEnumerable<string> RefreshTries(IEnumerable<IScalarItemModel> toList, IList<string> accList)
        {
            foreach (var dataListItemModel in toList)
            {
                if (!string.IsNullOrEmpty(dataListItemModel.DisplayName))
                {
                    accList.Add("[[" + dataListItemModel.DisplayName + "]]");
                }
            }
            return accList;
        }

        IEnumerable<string> RefreshRecordSets(IEnumerable<IRecordSetItemModel> toList, IList<string> accList)
        {
            foreach (var dataListItemModel in toList)
            {
                RefreshRecordSets(dataListItemModel, accList);
            }
            return accList;
        }

        void RefreshRecordSets(IRecordSetItemModel dataListItemModel, IList<string> accList)
        {
            if (!string.IsNullOrEmpty(dataListItemModel.DisplayName))
            {
                var recsetAppend = DataListUtil.MakeValueIntoHighLevelRecordset(dataListItemModel.DisplayName);
                var recsetStar = DataListUtil.MakeValueIntoHighLevelRecordset(dataListItemModel.DisplayName, true);

                accList.Add(DataListUtil.AddBracketsToValueIfNotExist(recsetAppend));
                accList.Add(DataListUtil.AddBracketsToValueIfNotExist(recsetStar));
            }
            foreach (var listItemModel in dataListItemModel.Children)
            {
                if (!string.IsNullOrEmpty(listItemModel.Name))
                {
                    var rec = DataListUtil.AddBracketsToValueIfNotExist(DataListUtil.CreateRecordsetDisplayValue(dataListItemModel.DisplayName, listItemModel.DisplayName, ""));
                    if (ExecutionEnvironment.IsRecordsetIdentifier(rec))
                    {
                        accList.Add(DataListUtil.ReplaceRecordBlankWithStar(rec));
                        accList.Add(rec);
                    }
                }
            }
            foreach (var listItemModel in ScalarCollection)
            {
                if (!string.IsNullOrEmpty(listItemModel.DisplayName))
                {
                    var rec = "[[" + listItemModel.DisplayName + "]]";
                    if (ExecutionEnvironment.IsScalar(rec))
                    {
                        accList.Add(rec);
                    }
                }
            }
        }

        public void AddBlankRow(IDataListItemModel item)
        {
            if (item != null)
            {
                if (!(item is IRecordSetItemModel) && !(item is IScalarItemModel))
                {
                    _scalarHandler.AddRowToScalars();
                }
                else
                {
                    if (item is IRecordSetItemModel)
                    {
                        _recordsetHandler.AddRowToRecordsets();
                    }
                }
            }
            else
            {
                _scalarHandler.AddRowToScalars();
                _recordsetHandler.AddRowToRecordsets();
            }
        }

        public void RemoveBlankRows(IDataListItemModel item)
        {
            if (item == null)
            {
                return;
            }

            if (!(item is IRecordSetItemModel) && item is IScalarItemModel)
            {
                _scalarHandler.RemoveBlankScalar();
            }
            else if (item is IRecordSetItemModel)
            {
                _recordsetHandler.RemoveBlankRecordsets();
            }
            else
            {
                _recordsetHandler.RemoveBlankFieldFromRecordsets();
            }
        }

        public void RemoveDataListItem(IDataListItemModel itemToRemove)
        {
            if (itemToRemove == null)
            {
                return;
            }

            if (itemToRemove is IComplexObjectItemModel complexObj)
            {
                var complexObjectItemModels = complexObj.Children;
                var allChildren = complexObjectItemModels.Flatten(model => model.Children);
                var notUsedItems = allChildren.Where(x => !x.IsUsed);
                foreach (var complexObjectItemModel in notUsedItems)
                {
                    RemoveUnusedChildComplexObjects(complexObj, complexObjectItemModel);
                }
                if (complexObj.Children.Count == 0)
                {
                    ComplexObjectCollection.Remove(complexObj);
                }
                else
                {
                    complexObj.IsUsed = true;
                }
            }

            if (itemToRemove is IScalarItemModel)
            {
                var item = itemToRemove as IScalarItemModel;
                ScalarCollection.Remove(item);
                CheckDataListItemsForDuplicates(DataList);
            }
            else if (itemToRemove is IRecordSetItemModel)
            {
                var item = itemToRemove as IRecordSetItemModel;
                RecsetCollection.Remove(item);
                CheckDataListItemsForDuplicates(DataList);
            }
            else
            {
                foreach (var recset in RecsetCollection)
                {
                    var item = itemToRemove as IRecordSetFieldItemModel;
                    recset.Children.Remove(item);
                    CheckDataListItemsForDuplicates(recset.Children);
                }
            }
            FindUnusedAndMissingCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
        }

        void RemoveUnusedChildComplexObjects(IComplexObjectItemModel parentItemModel, IComplexObjectItemModel itemToRemove)
        {
            _complexObjectHandler.RemoveUnusedChildComplexObjects(parentItemModel, itemToRemove);
        }

        public string WriteToResourceModel()
        {
            ScalarCollection.ForEach(_scalarHandler.FixNamingForScalar);
            _recordsetHandler.AddRecordsetNamesIfMissing();
            var result = GetDataListString();
            if (Resource != null)
            {
                Resource.DataList = result;
            }

            return result;
        }

        public void AddRecordsetNamesIfMissing()
        {
            _recordsetHandler.AddRecordsetNamesIfMissing();
        }

        public void ValidateNames(IDataListItemModel item)
        {
            if (item == null)
            {
                return;
            }

            if (item is IRecordSetItemModel)
            {
                _recordsetHandler.ValidateRecordset();
            }
            else if (item is IRecordSetFieldItemModel)
            {
                var rs = (IRecordSetFieldItemModel)item;
                _recordsetHandler.ValidateRecordsetChildren(rs.Parent);
            }
            else
            {
                ValidateScalar();
            }
        }

        void ValidateScalar()
        {
            CheckDataListItemsForDuplicates(DataList);
        }

        void CheckDataListItemsForDuplicates(IEnumerable<IDataListItemModel> itemsToCheck)
        {
            var duplicates = itemsToCheck.ToLookup(x => x.DisplayName);
            foreach (var duplicate in duplicates)
            {
                if (duplicate.Count() > 1 && !String.IsNullOrEmpty(duplicate.Key))
                {
                    duplicate.ForEach(model => model.SetError(StringResources.ErrorMessageDuplicateValue));
                }
                else
                {
                    duplicate.ForEach(model =>
                    {
                        if (model.ErrorMessage != null && model.ErrorMessage.Contains(StringResources.ErrorMessageDuplicateValue))
                        {
                            model.RemoveError();
                        }
                    });
                }
            }
        }

        bool HasAnyUnusedItems()
        {
            if (!HasItems())
            {
                return false;
            }

            bool hasUnused;

            if (ScalarCollection != null)
            {
                hasUnused = ScalarCollection.Any(sc => !sc.IsUsed);
                if (hasUnused)
                {
                    DeleteCommand.RaiseCanExecuteChanged();
                    return true;
                }
            }

            if (RecsetCollection != null)
            {
                hasUnused = RecsetCollection.Any(sc => !sc.IsUsed);
                if (!hasUnused)
                {
                    hasUnused = RecsetCollection.SelectMany(sc => sc.Children).Any(sc => !sc.IsUsed);
                }
                if (hasUnused)
                {
                    DeleteCommand.RaiseCanExecuteChanged();
                    return true;
                }
            }

            if (ComplexObjectCollection != null)
            {
                hasUnused = ComplexObjectCollection.Any(sc => !sc.IsUsed);
                if (hasUnused)
                {
                    DeleteCommand.RaiseCanExecuteChanged();
                    return true;
                }
            }
            return false;
        }

        OptomizedObservableCollection<IDataListItemModel> CreateFullDataList() => _helper.CreateFullDataList();

        void SortItems()
        {
            try
            {
                IsSorting = true;
                if (_scalarCollection.Any(model => !model.IsBlank))
                {
                    _scalarHandler.SortScalars(_toggleSortOrder);
                }
                if (_recsetCollection.Any(model => !model.IsBlank))
                {
                    _recordsetHandler.SortRecset(_toggleSortOrder);
                }
                if (_complexObjectCollection.Any(model => !model.IsBlank))
                {
                    _complexObjectHandler.SortComplexObjects(_toggleSortOrder);
                }
                _toggleSortOrder = !_toggleSortOrder;
                WriteToResourceModel();
            }
            finally { IsSorting = false; }
        }

        public bool IsSorting { get; set; }
        
        public void CreateListsOfIDataListItemModelToBindTo(out string errorString)
        {
            errorString = string.Empty;
            if (!string.IsNullOrEmpty(Resource.DataList))
            {
                var errors = new ErrorResultTO();
                try
                {
                    if (!string.IsNullOrEmpty(Resource.DataList))
                    {
                        ConvertDataListStringToCollections(Resource.DataList);
                    }
                }
                catch (Exception)
                {
                    errors.AddError(ErrorResource.InvalidVariableList);
                }
            }
            else
            {
                RecsetCollection.Clear();
                ScalarCollection.Clear();
                ComplexObjectCollection.Clear();

                _recordsetHandler.AddRecordSet();
            }

            BaseCollection = new OptomizedObservableCollection<DataListHeaderItemModel>();

            var variableNode = DataListItemModelFactory.CreateDataListHeaderItem("Variable");
            if (ScalarCollection.Count == 0)
            {
                var dataListItemModel = DataListItemModelFactory.CreateScalarItemModel(string.Empty);
                dataListItemModel.IsComplexObject = false;
                dataListItemModel.AllowNotes = false;
                ScalarCollection.Add(dataListItemModel);
            }
            BaseCollection.Add(variableNode);

            var recordsetsNode = DataListItemModelFactory.CreateDataListHeaderItem("Recordset");
            if (RecsetCollection.Count == 0)
            {
                _recordsetHandler.AddRecordSet();
            }
            BaseCollection.Add(recordsetsNode);

            var complexObjectNode = DataListItemModelFactory.CreateDataListHeaderItem("Object");
            BaseCollection.Add(complexObjectNode);

            AddBlankRow(null);

            BaseCollection[0].Children = ScalarCollection;
            BaseCollection[1].Children = RecsetCollection;
            BaseCollection[2].Children = ComplexObjectCollection;

            WriteToResourceModel();
        }

        public void ClearCollections()
        {
            BaseCollection.Clear();
        }

        public void UpdateHelpDescriptor(string helpText)
        {
            var mainViewModel = CustomContainer.Get<IShellViewModel>();
            mainViewModel?.HelpViewModel.UpdateHelpText(helpText);
        }

        public void GenerateComplexObjectFromJson(string parentObjectName, string json)
        {
            _complexObjectHandler.GenerateComplexObjectFromJson(parentObjectName, json);
        }

        public void ConvertDataListStringToCollections(string dataList)
        {
            RecsetCollection.Clear();
            ScalarCollection.Clear();
            ComplexObjectCollection.Clear();
            try
            {
                var xDoc = new XmlDocument();
                xDoc.LoadXml(dataList);
                if (xDoc.DocumentElement == null)
                {
                    return;
                }

                ConvertDataListStringToCollections(xDoc);
            }
            catch (Exception e)
            {
                Dev2Logger.Error(e, "Warewolf Error");
            }
        }

        void ConvertDataListStringToCollections(XmlDocument xDoc)
        {
            var children = xDoc.DocumentElement.ChildNodes;
            foreach (XmlNode child in children)
            {
                if (DataListUtil.IsSystemTag(child.Name))
                {
                    continue;
                }

                if (IsJsonAttribute(child))
                {
                    _complexObjectHandler.AddComplexObjectFromXmlNode(child, null);
                }
                else
                {
                    if (child.HasChildNodes)
                    {
                        _recordsetHandler.AddRecordSets(child);
                    }
                    else
                    {
                        AddScalars(child);
                    }
                }
            }
        }

        bool IsJsonAttribute(XmlNode child) => _helper.IsJsonAttribute(child);

        void AddScalars(XmlNode c)
        {
            _scalarHandler.AddScalars(c);
        }

        const string RootTag = "DataList";
        const string Description = "Description";
        const string IsEditable = "IsEditable";

        string GetDataListString()
        {
            var result = new StringBuilder("<" + RootTag + ">");
            foreach (var recSet in RecsetCollection.Where(model => !string.IsNullOrEmpty(model.DisplayName)))
            {
                IEnumerable<IDataListItemModel> filledRecordSet = recSet.Children.Where(c => !c.IsBlank && !c.HasError);
                var cols = filledRecordSet.Select(child => DataListFactory.CreateDev2Column(child.DisplayName, child.Description, child.IsEditable, child.ColumnIODirection));

                AddItemToBuilder(result, recSet);
                result.Append(">");
                foreach (var col in cols)
                {
                    result.AppendFormat("<{0} {1}=\"{2}\" {3}=\"{4}\" {5}=\"{6}\" />", col.ColumnName
                        , Description
                        , col.ColumnDescription
                        , IsEditable
                        , col.IsEditable
                        , GlobalConstants.DataListIoColDirection
                        , col.ColumnIODirection);
                }
                result.Append("</");
                result.Append(recSet.DisplayName);
                result.Append(">");
            }

            var filledScalars = ScalarCollection?.Where(scalar => !scalar.IsBlank && !scalar.HasError);
            if (!(filledScalars is null)) {
                foreach (var scalar in filledScalars)
                {
                    AddItemToBuilder(result, scalar);
                    result.Append("/>");
                }
            }
            var complexObjectItemModels = ComplexObjectCollection.Where(model => !string.IsNullOrEmpty(model.DisplayName) && !model.HasError);
            foreach (var complexObjectItemModel in complexObjectItemModels)
            {
                _complexObjectHandler.AddComplexObjectsToBuilder(result, complexObjectItemModel);
            }

            result.Append("</" + RootTag + ">");
            return result.ToString();
        }

        void AddItemToBuilder(StringBuilder result, IDataListItemModel item)
        {
            _helper.AddItemToBuilder(result, item);
        }

        bool HasItems() {
            var hasScalars = (ScalarCollection != null && ScalarCollection.Count > 1);
            var hasRecsets = (RecsetCollection != null && RecsetCollection.Count > 1);
            var hasComplex = (ComplexObjectCollection != null && ComplexObjectCollection.Count >= 1);
            return hasScalars || hasRecsets || hasComplex;
        }

        protected override void OnDispose()
        {
            ClearCollections();
            Resource = null;
        }

        void ShowUnusedDataListVariables(IResourceModel resourceModel, IEnumerable<IDataListVerifyPart> listOfUnused, IEnumerable<IDataListVerifyPart> listOfUsed)
        {
            if (resourceModel != Resource)
            {
                return;
            }

            if (listOfUnused != null && listOfUnused.Any())
            {
                SetIsUsedDataListItems(listOfUnused, false);
            }
            else
            {
                UpdateDataListItemsAsUsed();
            }

            if (listOfUsed != null && listOfUsed.Any())
            {
                SetIsUsedDataListItems(listOfUsed, true);
            }
        }

        void UpdateDataListItemsAsUsed()
        {
            _scalarHandler.SetScalarItemsAsUsed();
            _recordsetHandler.SetRecordSetItemsAsUsed();
        }

        public IEnumerable<IDataListVerifyPart> MissingWorkflowItems(IEnumerable<IDataListVerifyPart> partsToVerify) => MissingWorkflowItems(partsToVerify, false);

        public IEnumerable<IDataListVerifyPart> MissingWorkflowItems(IEnumerable<IDataListVerifyPart> partsToVerify, bool excludeUnusedItems)
        {
            if (DataList != null)
            {                
                foreach (var item in _missingDataList.MissingScalars(partsToVerify, excludeUnusedItems))
                {
                    yield return item;
                }
                foreach (var item in _missingDataList.MissingRecordsets(partsToVerify, excludeUnusedItems))
                {
                    yield return item;
                }
            }
            _complexObjectHandler.DetectUnusedComplexObjects(partsToVerify);
            FindUnusedAndMissingCommand.RaiseCanExecuteChanged();
        }

        public IEnumerable<IDataListVerifyPart> MissingDataListParts(IEnumerable<IDataListVerifyPart> partsToVerify)
        {
            var missingDataParts = new List<IDataListVerifyPart>();
            foreach (var part in partsToVerify.Where(part => DataList != null))
            {
                _recordsetHandler.FindMissingPartsForRecordset(part, missingDataParts);
                _scalarHandler.FindMissingForScalar(part, missingDataParts);
            }
            return missingDataParts;
        }

        public IEnumerable<IDataListVerifyPart> UpdateDataListItems(IResourceModel contextualResourceModel, IEnumerable<IDataListVerifyPart> workflowFields)
        {
            var removeParts = MissingWorkflowItems(workflowFields);
            var filteredDataListParts = MissingDataListParts(workflowFields);
            ShowUnusedDataListVariables(contextualResourceModel, removeParts, workflowFields);
            ViewModelUtils.RaiseCanExecuteChanged(DeleteCommand);
            if (contextualResourceModel != Resource)
            {
                return new IDataListVerifyPart[] { };
            }
            AddMissingDataListItems(filteredDataListParts);
            return filteredDataListParts;
        }

        public string DataListErrorMessage
        {
            get
            {
                if (!HasErrors)
                {
                    return "";
                }

                var allErrorMessages = RecsetCollection.Select(model =>
                {
                    if (_recordsetHandler.BuildRecordSetErrorMessages(model, out string errorMessage))
                    {
                        return errorMessage;
                    }
                    if (model.HasError)
                    {
                        return BuildErrorMessage(model);
                    }
                    return null;
                });

                allErrorMessages = allErrorMessages.Union(ScalarCollection.Select(model => model.HasError ? BuildErrorMessage(model) : null));
                allErrorMessages = allErrorMessages.Union(ComplexObjectCollection.Flatten(model => model.Children).Select(model => model.HasError ? BuildErrorMessage(model) : null));
                var completeErrorMessage = Environment.NewLine + string.Join(Environment.NewLine, allErrorMessages.Where(s => !string.IsNullOrEmpty(s)));
                return completeErrorMessage;
            }
        }

        public ISuggestionProvider Provider { get; set; }

        static string BuildErrorMessage(IDataListItemModel model) => DataListUtil.AddBracketsToValueIfNotExist(model.DisplayName) + " : " + model.ErrorMessage;

        public bool Equals(IDataListViewModel other)
        {
            var recordSetsAreEqual = CommonEqualityOps.CollectionEquals(RecsetCollection, other.RecsetCollection, new DataListItemModelComparer());
            var scalasAreEqual = CommonEqualityOps.CollectionEquals(ScalarCollection, other.ScalarCollection, new DataListItemModelComparer());
            var objectsAreEqual = CommonEqualityOps.CollectionEquals(ComplexObjectCollection, other.ComplexObjectCollection, new DataListItemModelComparer());

            return recordSetsAreEqual && scalasAreEqual && objectsAreEqual;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((DataListViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (ScalarCollection != null ? ScalarCollection.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (RecsetCollection != null ? RecsetCollection.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ComplexObjectCollection != null ? ComplexObjectCollection.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}