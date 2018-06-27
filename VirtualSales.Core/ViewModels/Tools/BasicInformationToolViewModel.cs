using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualSales.Core.AppServices;
using VirtualSales.Core.Models;

namespace VirtualSales.Core.ViewModels.Tools
{
    public class BasicInformationToolViewModel : ToolViewModel
    {
        private readonly PersonModel _person;

        public BasicInformationToolViewModel(ISharedDataService dataService, IViewModelLocator locator)
            : base(new Guid("74EF5D9B-E6D1-4218-8755-C8349157FAAE"))
        {
            _person = dataService.Person;
            Name = "Basic Information";

            AddToolPages(
                new Page1(this, locator.Mode),
                new Page2(this, locator.Mode),
                new Page3(this, locator.Mode));
        }

        public class Page1 : ToolPage
        {
            private readonly BasicInformationToolViewModel _parent;
            public Page1(BasicInformationToolViewModel parent, AppMode mode)
            {
                Mode = mode;
                _parent = parent;
            }
            public AppMode Mode { get; private set; }
            public PersonModel Person
            {
                get { return _parent._person; }
            }
        }
        public class Page2 : ToolPage
        {
            private readonly BasicInformationToolViewModel _parent;
            public Page2(BasicInformationToolViewModel parent, AppMode mode)
            {
                Mode = mode;
                _parent = parent;
            }
            public AppMode Mode { get; private set; }
            public PersonModel Person
            {
                get { return _parent._person; }
            }
        }
        public class Page3 : ToolPage
        {
            private readonly BasicInformationToolViewModel _parent;
            public Page3(BasicInformationToolViewModel parent, AppMode mode)
            {
                Mode = mode;
                _parent = parent;
            }
            public AppMode Mode { get; private set; }
            public PersonModel Person
            {
                get { return _parent._person; }
            }
        }
    }
}
