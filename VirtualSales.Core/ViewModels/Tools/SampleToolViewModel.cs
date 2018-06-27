using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualSales.Core.AppServices;
using VirtualSales.Core.Models;

namespace VirtualSales.Core.ViewModels.Tools
{
    public class SampleToolViewModel : ToolViewModel
    {
        private readonly PersonModel _person;
        public SampleToolViewModel(ISharedDataService dataService)
            : base(new Guid("46A3FF71-ED2C-431F-86EF-A33B1D490E6D"))
        {
            _person = dataService.Person;

            Name = "Sample Tool";

            AddToolPages(
                new Page1(),
                new Page2(this),
                new Page3());
        }


        public class Page1 : ToolPage
        {
            
        }

        public class Page2 : ToolPage
        {
            private readonly SampleToolViewModel _parent;

            public Page2(SampleToolViewModel parent)
            {
                _parent = parent;
            }

            public PersonModel Person
            {
                get { return _parent._person; }
            }
        }

        public class Page3 : ToolPage
        {
            
        }
    }
   
}
