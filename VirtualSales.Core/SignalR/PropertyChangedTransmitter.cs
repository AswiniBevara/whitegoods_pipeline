using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReactiveUI;
using VirtualSales.Models;

namespace VirtualSales.Core.SignalR
{
    public interface IPropertyChangedTransmitter : IDisposable
    {
        void RegisterForChanges(IReactiveNotifyPropertyChanged obj);

        void UnRegisterForChanges(IReactiveNotifyPropertyChanged obj);

        IObservable<ChangedMessage> OutgoingChanges { get; }
    }
    public class PropertyChangedTransmitter : IPropertyChangedTransmitter
    {
        private static readonly JsonSerializerSettings _SerializerSettings = new JsonSerializerSettings();

        private readonly ReactiveList<IReactiveNotifyPropertyChanged> _registeredObjects = new ReactiveList<IReactiveNotifyPropertyChanged>();

        private IDisposable _connectionDisposable;
        private bool _disposed;

        public PropertyChangedTransmitter()
        {
            // Any time the collection changes, get a new merged series of observables and merge them together. 
            // Then Switch flattens and takes only the latest IObservable and disposes the previous ones.
            var changesOnAnyObj = _registeredObjects.Changed
                .Select(_ => _registeredObjects
                    .Select(o => {
                        var type = o.GetType();
                        
                        return o.Changed.Select(changed => new ChangedMessage {
                            PropertyName = changed.PropertyName,
                            Value = JsonConvert.SerializeObject(changed.GetValue(), _SerializerSettings),
                            SourceObjectType = type.FullName
                        });
                    })
                    .Merge())
                .Switch()
                .Publish();
             _connectionDisposable = changesOnAnyObj.Connect();

             OutgoingChanges = changesOnAnyObj;
        }

        public void RegisterForChanges(IReactiveNotifyPropertyChanged obj)
        {
            _registeredObjects.Add(obj);
        }

        public void UnRegisterForChanges(IReactiveNotifyPropertyChanged obj)
        {
            _registeredObjects.Remove(obj);
        }

        public IObservable<ChangedMessage> OutgoingChanges { get; private set; }
        public void Dispose()
        {
            if (! _disposed)
            {
                _connectionDisposable.Dispose();
                _disposed = true;
                _connectionDisposable = null;
            }
        }
    }

}
