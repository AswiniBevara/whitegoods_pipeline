using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VirtualSales.Models;

namespace VirtualSales.Core.SignalR
{
    public interface IPropertyChangedReceiver : IObserver<ChangedMessage>
    {
        void ProxyChangesToInstance<T>(T instance) where T : class;
    }
    public class PropertyChangedReceiver :  ObserverBase<ChangedMessage>, IPropertyChangedReceiver
    {
        
        private readonly Dictionary<Type, object> _types = new Dictionary<Type, object>();

        public void ProxyChangesToInstance<T>(T instance) where T : class
        {
            if (instance == null) throw new ArgumentNullException("instance");
  

            if(_types.ContainsKey(typeof(T)))
                throw new InvalidOperationException("Type " + typeof(T) + " is already registered");

            _types[typeof (T)] = instance;
        }


        protected override void OnNextCore(ChangedMessage value)
        {
            // Get the instance
            object instance;

            var type = Type.GetType(value.SourceObjectType, true);
            if (_types.TryGetValue(type, out instance))
            {
                var prop = instance.GetType().GetRuntimeProperty(value.PropertyName);

                // Convert value back to the orig type
                var obj = JsonConvert.DeserializeObject(value.Value, prop.PropertyType);

                if (prop.SetMethod != null)
                {
                    prop.SetMethod.Invoke(instance, new[] { obj });
                }
            }
        }

        protected override void OnErrorCore(Exception error)
        {
            throw error;
        }

        protected override void OnCompletedCore()
        {
            _types.Clear();
        }
    }
}
