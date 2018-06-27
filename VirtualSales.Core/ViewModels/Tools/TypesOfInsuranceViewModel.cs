using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace VirtualSales.Core.ViewModels.Tools
{
    public class TypesOfInsuranceViewModel : ToolViewModel
    {
        public TypesOfInsuranceViewModel()
            : base(new Guid("A39AA814-9123-46C2-8C39-2F2D615C5192"))
        {
            Name = "Types of Insurance";

            AddToolPages(new Page1());
        }

        public class Page1 : ToolPage
        {

        }
    }
}
