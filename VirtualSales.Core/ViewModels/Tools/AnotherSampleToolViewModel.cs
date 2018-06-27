using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualSales.Core.AppServices;
using VirtualSales.Core.Models;

namespace VirtualSales.Core.ViewModels.Tools
{
    public class AnotherSampleToolViewModel : ToolViewModel
    {
        private readonly PersonModel _person;
        public AnotherSampleToolViewModel(ISharedDataService dataService)
            : base(new Guid("FC8C62F6-05D9-42FA-8FDA-1CC16D62769D"))
        {
            _person = dataService.Person;

            Name = "Another Sample Tool";

            AddToolPages(
                new Page1(),
                new Page2(this));
        }


        public class Page1 : ToolPage
        {
            
        }

        public class Page2 : ToolPage
        {
            private readonly AnotherSampleToolViewModel _parent;

            public Page2(AnotherSampleToolViewModel parent)
            {
                _parent = parent;
            }

            public PersonModel Person
            {
                get { return _parent._person; }
            }
        }

    }
   
}
