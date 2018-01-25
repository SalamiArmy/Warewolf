using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Dev2.SignalR.Wrappers
{
    public interface IHubProxyWrapper
    {
        Task Invoke(string method, params object[] args);
        
        Task<T> Invoke<T>(string method, params object[] args);

        object Object();

        IDisposable On<T>(string eventName, Action<T> onData);
        
        ISubscriptionWrapper Subscribe(string sendmemo);
    }

    public interface ISubscriptionWrapper
    {
        event Action<IList<JToken>> Received;
    }
}
