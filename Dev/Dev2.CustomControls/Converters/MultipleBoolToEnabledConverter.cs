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
using System.Globalization;
using System.Windows.Data;

namespace Dev2.CustomControls.Converters
{
    public class MultipleBoolToEnabledConverter : IMultiValueConverter
    {
        #region Implementation of IMultiValueConverter
        
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (object value in values)
            {
                var tmpval = value as bool?;
                if (tmpval != null && !tmpval.GetValueOrDefault())
                {
                    return false;
                }

            }
            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => new object[] { };

        #endregion
    }
}