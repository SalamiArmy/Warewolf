/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2018 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using Dev2.Common.DateAndTime;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Dev2.Tests.ConverterTests.DateTimeTests
{
    /// <summary>
    /// Summary description for DateTimeComparerTests
    /// </summary>
    [TestClass]
    public class DateTimeComparerTests
    {
        #region Fields

        const string Input1 = "2011/06/05 08:20:30:123 AM";
        string _input2 = "2012/06/05 08:20:30:123 AM";
        const string InputFormat = "yyyy/mm/dd 12h:min:ss:sp am/pm";
        string _outputType = "";

        #endregion Fields

        #region TestContext

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #endregion TestContext

        #region Comparer Tests

        #region Years Tests

        [TestMethod]
        public void TryCompare_Years_Negative_Years_Expected_NegativeOne_Years()
        {
            _input2 = "2010/06/05 08:20:30:124 AM";
            _outputType = "Years";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "-1");
        }
     
        [TestMethod]
        public void TryCompare_Years_Equal_Expected_One_Years()
        {
            _outputType = "Years";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "1");
        }
        
        [TestMethod]
        public void TryCompare_Years_One_Short_Expected_Zero_Years()
        {
            _input2 = "2012/06/05 08:20:30:122 AM";
            _outputType = "Years";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "0");
        }
        
        [TestMethod]
        public void TryCompare_Years_One_Over_Expected_One_Years()
        {
            _input2 = "2012/06/05 08:20:30:124 AM";
            _outputType = "Years";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "1");
        }

        #endregion Years Tests

        #region Months Tests

        [TestMethod]
        public void TryCompare_Months_Negative_Expected_NegativeOne_Months()
        {
            _input2 = "2011/05/05 08:20:30:123 AM";
            _outputType = "Months";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "-1");
        }
        
        [TestMethod]
        public void TryCompare_Months_Equal_Expected_One_Months()
        {
            _input2 = "2011/07/05 08:20:30:123 AM";
            _outputType = "Months";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "1");
        }
        
        [TestMethod]
        public void TryCompare_Months_One_Short_Expected_Zero_Months()
        {
            _input2 = "2011/07/05 08:20:30:122 AM";
            _outputType = "Months";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "0");
        }

        
        [TestMethod]
        public void TryCompare_Months_One_Over_Expected_One_Months()
        {
            _input2 = "2011/07/05 08:20:30:124 AM";
            _outputType = "Months";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "1");
        }

        #endregion Months Tests

        #region Days Tests

        [TestMethod]
        public void TryCompare_Days_Negative_Expected_NegativeOne_Days()
        {
            _input2 = "2011/06/04 08:20:30:123 AM";
            _outputType = "Days";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "-1");
        }

        [TestMethod]
        public void TryCompare_Days_Equal_Expected_One_Days()
        {
            _input2 = "2011/06/06 08:20:30:123 AM";
            _outputType = "Days";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "1");
        }

        [TestMethod]
        public void TryCompare_Days_One_Short_Expected_Zero_Days()
        {
            _input2 = "2011/06/06 08:20:30:122 AM";
            _outputType = "Days";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "0");
        }


        [TestMethod]
        public void TryCompare_Days_One_Over_Expected_One_Days()
        {
            _input2 = "2011/06/06 08:20:30:124 AM";
            _outputType = "Days";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "1");
        }

        #endregion Days Tests

        #region Weeks Tests

        [TestMethod]
        public void TryCompare_Weeks_Negative_Expected_NegativeOne_Weeks()
        {
            _input2 = "2011/05/28 08:20:30:123 AM";
            _outputType = "Weeks";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.AreEqual("-2",result);
        }

        [TestMethod]
        public void TryCompare_Weeks_Equal_Expected_One_Weeks()
        {
            _input2 = "2011/06/12 08:20:30:123 AM";
            _outputType = "Weeks";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "1");
        }

        [TestMethod]
        public void TryCompare_Weeks_One_Short_Expected_Zero_Weeks()
        {
            _input2 = "2011/06/12 08:20:30:122 AM";
            _outputType = "Weeks";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "0");
        }


        [TestMethod]
        public void TryCompare_Weeks_One_Over_Expected_One_Weeks()
        {
            _input2 = "2011/06/12 08:20:30:124 AM";
            _outputType = "Weeks";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "1");
        }

        #endregion Weeks Tests

        #region Hours Tests

        [TestMethod]
        public void TryCompare_Hours_Negative_Expected_NegativeOne_Hours()
        {
            _input2 = "2011/06/05 07:20:30:123 AM";
            _outputType = "Hours";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "-1");
        }

        [TestMethod]
        public void TryCompare_Hours_Equal_Expected_One_Hours()
        {
            _input2 = "2011/06/05 09:20:30:123 AM";
            _outputType = "Hours";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "1");
        }

        [TestMethod]
        public void TryCompare_Hours_One_Short_Expected_Zero_Hours()
        {
            _input2 = "2011/06/05 09:20:30:122 AM";
            _outputType = "Hours";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "0");
        }


        [TestMethod]
        public void TryCompare_Hours_One_Over_Expected_One_Hours()
        {
            _input2 = "2011/06/05 09:20:30:124 AM";
            _outputType = "Hours";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "1");
        }

        #endregion Hours Tests

        #region Minutes Tests

        [TestMethod]
        public void TryCompare_Minutes_Negative_Expected_NegativeOne_Minutes()
        {
            _input2 = "2011/06/05 08:19:30:123 AM";
            _outputType = "Minutes";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "-1");
        }

        [TestMethod]
        public void TryCompare_Minutes_Equal_Expected_One_Minutes()
        {
            _input2 = "2011/06/05 08:21:30:123 AM";
            _outputType = "Minutes";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "1");
        }

        [TestMethod]
        public void TryCompare_Minutes_One_Short_Expected_Zero_Minutes()
        {
            _input2 = "2011/06/05 08:21:30:122 AM";
            _outputType = "Minutes";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "0");
        }


        [TestMethod]
        public void TryCompare_Minutes_One_Over_Expected_One_Minutes()
        {
            _input2 = "2011/06/05 08:21:30:124 AM";
            _outputType = "Minutes";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "1");
        }

        #endregion Minutes Tests

        #region Seconds Tests

        [TestMethod]
        public void TryCompare_Seconds_Negative_Expected_NegativeOne_Seconds()
        {
            _input2 = "2011/06/05 08:20:29:123 AM";
            _outputType = "Seconds";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "-1");
        }

        [TestMethod]
        public void TryCompare_Seconds_Equal_Expected_One_Seconds()
        {
            _input2 = "2011/06/05 08:20:31:123 AM";
            _outputType = "Seconds";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "1");
        }

        [TestMethod]
        public void TryCompare_Seconds_One_Short_Expected_Zero_Seconds()
        {
            _input2 = "2011/06/05 08:20:31:122 AM";
            _outputType = "Seconds";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "0");
        }


        [TestMethod]
        public void TryCompare_Seconds_One_Over_Expected_One_Seconds()
        {
            _input2 = "2011/06/05 08:20:31:124 AM";
            _outputType = "Seconds";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "1");
        }

        #endregion Seconds Tests

        #region SplitSeconds Tests

        [TestMethod]
        public void TryCompare_SplitSeconds_Equal_Expected_Zero_SplitSeconds()
        {
            _input2 = "2011/06/05 08:20:30:123 AM";
            _outputType = "Milliseconds";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "0");
        }

        [TestMethod]
        public void TryCompare_SplitSeconds_One_Short_Expected_Zero_SplitSeconds()
        {
            _input2 = "2011/06/05 08:20:30:122 AM";
            _outputType = "Milliseconds";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.IsTrue(result == "-1");
        }


        [TestMethod]
        public void TryCompare_SplitSeconds_One_Over_Expected_One_SplitSeconds()
        {
            _input2 = "2011/06/05 08:20:30:124 AM";
            _outputType = "Milliseconds";
            var comparer = DateTimeConverterFactory.CreateComparer();
            var dateTimeResult = DateTimeConverterFactory.CreateDateTimeDiffTO(Input1, _input2, InputFormat, _outputType);
            comparer.TryCompare(dateTimeResult, out string result, out string error);
            Assert.AreEqual("1", result);
        }

        #endregion SplitSeconds Tests

        #endregion Comparer Tests
    }
}
