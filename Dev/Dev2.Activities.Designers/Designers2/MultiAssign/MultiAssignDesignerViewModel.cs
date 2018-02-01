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
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using Dev2.Activities.Designers2.Core;
using Dev2.Common.Interfaces.Infrastructure.Providers.Errors;
using Dev2.Studio.Core;
using Dev2.Studio.Core.Activities.Utils;
using Dev2.Studio.Interfaces;
using Unlimited.Applications.BusinessDesignStudio.Activities;

namespace Dev2.Activities.Designers2.MultiAssign
{
    public class MultiAssignDesignerViewModel : ActivityCollectionDesignerViewModel<ActivityDTO>
    {
        readonly Func<string> GetDatalistString = () => DataListSingleton.ActiveDataList.Resource.DataList;

        public MultiAssignDesignerViewModel(ModelItem modelItem)
            : base(modelItem)
        {
            AddTitleBarLargeToggle();
            AddTitleBarQuickVariableInputToggle();

            dynamic mi = ModelItem;
            InitializeItems(mi.FieldsCollection);
            HelpText = Warewolf.Studio.Resources.Languages.HelpText.Tool_Data_Assign;
        }

        public override string CollectionName => "FieldsCollection";

        protected override IEnumerable<IActionableErrorInfo> ValidateThis()
        {
            yield break;
        }

        protected override IEnumerable<IActionableErrorInfo> ValidateCollectionItem(ModelItem mi)
        {
            var dto = mi.GetCurrentValue() as ActivityDTO;
            if (dto == null)
            {
                yield break;
            }

            foreach (var error in dto.GetRuleSet("FieldName", GetDatalistString()).ValidateRules("'Variable'", () => mi.SetProperty("IsFieldNameFocused", true)))
            {
                yield return error;
            }
            foreach (var error in dto.GetRuleSet("FieldValueAndCalculate", GetDatalistString()).ValidateRules("'New Value'", () => mi.SetProperty("IsFieldValueFocused", true)))
            {
                yield return error;
            }
        }

        public override void UpdateHelpDescriptor(string helpText)
        {
            var mainViewModel = CustomContainer.Get<IShellViewModel>();
            mainViewModel?.HelpViewModel.UpdateHelpText(helpText);
        }
    }
}
