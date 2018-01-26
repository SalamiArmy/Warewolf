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
using System.Collections.Generic;
using Dev2.PathOperations;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Dev2.Data.Tests.PathOperations
{
    [TestClass]
    public class Dev2FileSystemProviderTests
    {
        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("Dev2FileSystemProvider_CRUDOperationTests")]
        public void Dev2FileSystemProvider_GetOperation_NonExistingPath_FriendlyError()
        {
            var pass = false;
            var testProvider = new Dev2FileSystemProvider();
            var path = ActivityIOFactory.CreatePathFromString("C:/dadsdascasxxxacvaawqf", false);
            try
            {
                using(testProvider.Get(path, new List<string>()))
                {
                    // foo ;)
                }
            }
            catch(Exception ex)
            {
                Assert.AreEqual("File C:/dadsdascasxxxacvaawqf not found ", ex.Message);
                pass = true;
            }
            if(!pass)
            {
                Assert.Fail("The correct error wasn't returned.");
            }
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("Dev2FileSystemProvider_CRUDOperationTests")]
        public void Dev2FileSystemProvider_GetDirectoryOperation_NonExistingPath_FriendlyError()
        {
            var pass = false;
            var testProvider = new Dev2FileSystemProvider();
            var path = ActivityIOFactory.CreatePathFromString("C:/dadsdascasxxxacvaawqf", false);
            try
            {
                testProvider.ListDirectory(path);
            }
            catch(Exception ex)
            {
                Assert.AreEqual("Directory not found C:/dadsdascasxxxacvaawqf", ex.Message);
                pass = true;
            }
            if(!pass)
            {
                Assert.Fail("The corrrect error wasnt returned");
            }
        }
    }
}
