using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VirtualSales.Core.SignalRx
{
    public static  class RxExtensions
    {   /// <summary>
        /// Registers a <see cref="IHubProxy"/> event has an <see cref="T:IObservable{T}"/>.
        /// </summary>
        /// <param name="proxy">The <see cref="IHubProxy"/></param>
        /// <param name="eventName">The name of the event.</param>
        /// <returns>An <see cref="T:IObservable{object[]}"/>.</returns>
        public static IObservable<IList<JToken>> Observe(this IHubProxy proxy, string eventName)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }

            if (String.IsNullOrEmpty(eventName))
            {
                throw new ArgumentNullException("eventName");
            }

            return new Hubservable(proxy, eventName);
        }

        public static IObservable<T> Observe<T>(this IHubProxy proxy, string eventName)
        {
            return proxy.Observe(eventName).Select(list => Convert<T>(list[0], proxy.JsonSerializer));
        }

        private static T Convert<T>(JToken obj, JsonSerializer serializer)
        {
            if (obj == null)
            {
                return default(T);
            }

            return obj.ToObject<T>(serializer);
        }

        public static IObservable<string> AsObservable(this Connection connection)
        {
            return connection.AsObservable(value => value);
        }

        public static IObservable<T> AsObservable<T>(this Connection connection)
        {
            return connection.AsObservable(value => connection.JsonDeserializeObject<T>(value));
        }

        public static IObservable<T> AsObservable<T>(this Connection connection, Func<string, T> selector)
        {
            return new ObservableConnection<T>(connection, selector);
        }
    }
}
