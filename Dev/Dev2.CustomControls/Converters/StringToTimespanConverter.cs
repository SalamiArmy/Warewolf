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
    public class StringToTimespanConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = string.Empty;

            if (value is TimeSpan && TimeSpan.TryParse(value.ToString(), out TimeSpan time))
            {
                result = time.Minutes.ToString(CultureInfo.InvariantCulture);
            }


            return result;
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = new TimeSpan();

            if (int.TryParse(value.ToString(), out int inVal))
            {
                result = new TimeSpan(0, inVal, 0);
            }

            return result;
        }

        #endregion
    }
}