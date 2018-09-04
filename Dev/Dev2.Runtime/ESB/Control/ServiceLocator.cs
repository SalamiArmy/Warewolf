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
using System.IO;
using System.Linq;
using Dev2.Common.Interfaces.Monitoring;
using Dev2.DynamicServices;
using Dev2.Runtime.Hosting;
using Dev2.Runtime.Interfaces;
using Warewolf.Resource.Errors;

namespace Dev2.Runtime.ESB.Control
{
    public class ServiceLocator : IServiceLocator
    {
        readonly IResourceCatalog _resourceCatalog = ResourceCatalog.Instance;
        #region New Mgt Methods

        public DynamicService FindService(string serviceName, Guid workspaceID) => FindService(serviceName, workspaceID, null);
        public DynamicService FindService(string serviceName, Guid workspaceID, IWarewolfPerformanceCounterLocater _perfCounter)
        {
            if (_perfCounter == null)
            {
                _perfCounter = CustomContainer.Get<IWarewolfPerformanceCounterLocater>();
            }
            var perfCounter = _perfCounter.GetCounter("Count of requests for workflows which don't exist");
            if (string.IsNullOrEmpty(serviceName))
            {
                throw new InvalidDataException(ErrorResource.ServiceIsNull);
            }

            var res = _resourceCatalog.GetResource(workspaceID, serviceName);
            DynamicService ret = null;
            if (res != null)
            {
                ret = ServiceActionRepo.Instance.ReadCache(res.ResourceID);
            }
            if (ret == null)
            {
                ret = _resourceCatalog.GetDynamicObjects<DynamicService>(workspaceID, serviceName).FirstOrDefault();                
                if (ret == null)
                {
                    perfCounter.Increment();
                }
            }
            return ret;
        }

        public DynamicService FindService(Guid serviceID, Guid workspaceID) => FindService(serviceID, workspaceID, null);
        public DynamicService FindService(Guid serviceID, Guid workspaceID, IWarewolfPerformanceCounterLocater _perfCounter)
        {
            if (_perfCounter == null)
            {
                _perfCounter = CustomContainer.Get<IWarewolfPerformanceCounterLocater>();
            }
            var perfCounter = _perfCounter.GetCounter("Count of requests for workflows which don't exist");
            if (serviceID == Guid.Empty)
            {
                throw new InvalidDataException(ErrorResource.ServiceIsNull);
            }

            var firstOrDefault = ServiceActionRepo.Instance.ReadCache(serviceID);
                        
            if (firstOrDefault == null)
            {
                firstOrDefault = _resourceCatalog.GetDynamicObjects<DynamicService>(workspaceID, serviceID).FirstOrDefault();
                if (firstOrDefault != null)
                {
                    firstOrDefault.ServiceId = serviceID;
                    firstOrDefault.Actions.ForEach(action =>
                    {
                        action.ServiceID = serviceID;
                    });
                }
                if (firstOrDefault == null)
                {
                    perfCounter.Increment();
                }
            }

            return firstOrDefault;
        }
        
        public Source FindSourceByName(string sourceName, Guid workspaceID)
        {
            if (string.IsNullOrEmpty(sourceName))
            {
                throw new InvalidDataException(ErrorResource.ServiceIsNull);
            }

            return _resourceCatalog.GetDynamicObjects<Source>(workspaceID, sourceName).FirstOrDefault();
        }

        #endregion

    }


}
