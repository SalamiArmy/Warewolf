/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2018 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System.Collections.Generic;
using Dev2.Common.Interfaces.Data;
using Dev2.Studio.ViewModels.DataList;


namespace Dev2.Studio.Factory
{
    public static class InputOutputViewModelFactory
    {
        public static IEnumerable<IInputOutputViewModel> CreateListToDisplayOutputs(IEnumerable<IDev2Definition> outputList)
        {
            foreach(IDev2Definition otp in outputList)
            {
                yield return new InputOutputViewModel(otp.Name, otp.RawValue, otp.MapsTo, otp.DefaultValue, otp.IsRequired, otp.RecordSetName, false);
            }
        }

        public static IEnumerable<IInputOutputViewModel> CreateListToDisplayInputs(IEnumerable<IDev2Definition> inputList)
        {
            foreach(IDev2Definition itp in inputList)
            {
                yield return new InputOutputViewModel(itp.Name, itp.RawValue, itp.RawValue, itp.DefaultValue, itp.IsRequired, itp.RecordSetName, itp.EmptyToNull);
            }
        }
    }
}
