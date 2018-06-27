using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using TinyIoC;

namespace VirtualSales.Core.Infrastructure
{
    public class RxUiViewLocator : IViewLocator
    {
        private readonly TinyIoCContainer _container;

        public RxUiViewLocator(TinyIoCContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            _container = container;
        }

        public IViewFor ResolveView<T>(T viewModel, string contract = null) where T : class
        {
            return (IViewFor)(contract != null ? _container.Resolve<T>(contract) : _container.Resolve<T>());
        }
    }
}
