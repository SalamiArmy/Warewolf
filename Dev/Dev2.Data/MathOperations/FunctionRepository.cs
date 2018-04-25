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
using System.Linq;
using System.Linq.Expressions;
using Dev2.Common;
using Dev2.Common.Interfaces;
using Dev2.Data.MathOperations;
using Infragistics.Calculations.CalcManager;
using Infragistics.Calculations.Engine;
using Warewolf.Resource.Errors;


namespace Dev2.MathOperations
{
    // PBI: 1214
    //This repository will contain a collection of all the functions available to the Function Manager 
    // to perform evaluations on
    public class FunctionRepository : IFrameworkRepository<IFunction>
    {
        readonly List<IFunction> _functions = new List<IFunction>();
        static readonly IDev2CalculationManager CalcManager = new Dev2CalculationManager();
        bool _isDisposed;

        /// <summary>
        /// Returns the entire collection of functions.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IFunction> All()
        {
            return _functions;
        }

        /// <summary>
        /// Finds a collection of functions that satisfy a condition specified by the expression passed in
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IEnumerable<IFunction> Find(Expression<Func<IFunction, bool>> expression)
        {
            if (expression != null)
            {
                return _functions.AsQueryable().Where(expression);
            }
            
            throw new ArgumentNullException(ErrorResource.ExpressionCannotBeNull);
            
        }

        /// <summary>
        /// Finds the first function in the collection that satisfies the expression passed in
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IFunction FindSingle(Expression<Func<IFunction, bool>> expression)
        {
            if (expression is null)
            {
                return null;
            }

            try
            {
                return _functions.AsQueryable().First(expression);
            }
            catch (InvalidOperationException ioex)
            {
                Dev2Logger.Error(ioex, GlobalConstants.WarewolfError);
                var func = MathOpsFactory.CreateFunction();
                return func;
            }
        }


        /// <summary>
        /// // Load the Repository of all the functions from the CalculationManager.
        /// </summary>
        public void Load()
        {
            var calcFunctions = CalcManager.GetAllFunctions();

            foreach (CalculationFunction calcFunction in calcFunctions)
            {
                _functions.Add(MathOpsFactory.CreateFunction(calcFunction.Name, calcFunction.ArgList, calcFunction.ArgDescriptors, calcFunction.Description));
            }

        }

        /// <summary>
        /// Removes a collection of functions from the function repository
        /// </summary>
        /// <param name="instanceObjs"></param>
        public void Remove(IEnumerable<IFunction> instanceObjs)
        {
            var instanceObjects = instanceObjs ?? throw new ArgumentNullException(ErrorResource.CannotRemoveNullListOfFunctions);
            
            foreach (var func in instanceObjects)
            {
                _functions.Remove(func);
            }
        }

        /// <summary>
        /// Removes a function from the function repository
        /// </summary>
        /// <param name="instanceObj"></param>
        public void Remove(IFunction instanceObj)
        {
            if (instanceObj is null)
            {
                throw new ArgumentNullException(ErrorResource.FunctionCannotBeNull);
            }
            _functions.Remove(instanceObj);
        }

        /// <summary>
        /// Save A collection of new functions to the function library
        /// </summary>
        /// <param name="instanceObjs"></param>
        public void Save(IEnumerable<IFunction> instanceObjs)
        {
            if (instanceObjs is null)
            {
                throw new ArgumentNullException(ErrorResource.CannotSaveNullListOfFunctions);
            }
            _functions.AddRange(instanceObjs);
        }
        /// <summary>
        /// Save a collection of new user-defined functions to the function library
        /// </summary>
        /// <param name="instanceObj"></param>
        public string Save(IFunction instanceObj)
        {
            if (instanceObj is null)
            {
                throw new ArgumentNullException(ErrorResource.FunctionCannotBeNull);
            }
            _functions.Add(instanceObj);

            return "Saved";
        }

        public event EventHandler ItemAdded;

    
        protected void OnItemAdded()
        {
            ItemAdded?.Invoke(this, new EventArgs());
        }

        #region Implementation of IDisposable

        ~FunctionRepository()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);

            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!_isDisposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.                    
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                _isDisposed = true;
            }
        }

        #endregion
    }
}
