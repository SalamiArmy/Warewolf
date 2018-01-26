/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2018 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unlimited.Framework.Converters.Graph.Poco;


namespace Dev2.Tests.ConverterTests.GraphTests.PocoTests
{
    [TestClass]
    public class PocoPathSegmentTests
    {
        /// <summary>
        /// To string on enumerable segment expected enumerable format returned.
        /// </summary>
        [TestMethod]
        public void ToStringOnEnumerableSegment_Expected_EnumerableFormat()
        {
            var path = new PocoPath();
            var segment = path.CreatePathSegment("Collection()");

            const string expected = "Collection()";
            var actual = segment.ToString();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// To string on scalar segment expected scalar format returned.
        /// </summary>
        [TestMethod]
        public void ToStringOnScalarSegment_Expected_ScalarFormat()
        {
            var path = new PocoPath();
            var segment = path.CreatePathSegment("Name");

            const string expected = "Name";
            var actual = segment.ToString();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// To string on enumerable segment where enumerables aren't considered expected scalar format returned.
        /// </summary>
        [TestMethod]
        public void ToStringOnEnumerableSegment_WhereEnumerablesArentConsidered_Expected_ScalarFormat()
        {
            var path = new PocoPath();
            var segment = path.CreatePathSegment("Collection()");

            const string expected = "Collection";
            var actual = segment.ToString(false);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// To string on enumerable segment where enumerables are considered expected scalar format returned.
        /// </summary>
        [TestMethod]
        public void ToStringOnEnumerableSegment_WhereEnumerablesAreConsidered_Expected_ScalarFormat()
        {
            var path = new PocoPath();
            var segment = path.CreatePathSegment("Collection()");

            const string expected = "Collection()";
            var actual = segment.ToString(true);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// To string on scalar segment where enumerables arent considered expected scalar format returned.
        /// </summary>
        [TestMethod]
        public void ToStringOnScalarSegment_WhereEnumerablesArentConsidered__Expected_ScalarFormat()
        {
            var path = new PocoPath();
            var segment = path.CreatePathSegment("Name");

            const string expected = "Name";
            var actual = segment.ToString(false);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// To string on scalar segment where enumerables are considered expected scalar format returned.
        /// </summary>
        [TestMethod]
        public void ToStringOnScalarSegmentt_WhereEnumerablesAreConsidered__Expected_ScalarFormat()
        {
            var path = new PocoPath();
            var segment = path.CreatePathSegment("Name");

            const string expected = "Name";
            var actual = segment.ToString(true);

            Assert.AreEqual(expected, actual);
        }
    }
}
