using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualSales.Core.ViewModels.Tools
{
    public class DividendInterestRatesViewModel : ToolViewModel
    {
        public DividendInterestRatesViewModel()
            : base(new Guid("D4363596-120E-4278-8240-7F742805FA22"))
        {
            Name = "Dividend Interest Rates";


            AddToolPages(new Page1());
        }


        public class Page1 : ToolPage
        {
            private readonly Dictionary<DateTime, double> _rates = new Dictionary<DateTime, double>
            {
                { new DateTime(1996, 1, 1), .084},
                { new DateTime(1997, 1, 1), .084},
                { new DateTime(1998, 1, 1), .084},
                { new DateTime(1999, 1, 1), .084},
                { new DateTime(2000, 1, 1), .082},
                { new DateTime(2001, 1, 1), .082},
                { new DateTime(2002, 1, 1), .0805},
                { new DateTime(2003, 1, 1), .079},
                { new DateTime(2004, 1, 1), .075},
                { new DateTime(2005, 1, 1), .07},
                { new DateTime(2006, 1, 1), .074},
                { new DateTime(2007, 1, 1), .075},
                { new DateTime(2008, 1, 1), .079},
                { new DateTime(2009, 1, 1), .076},
                { new DateTime(2010, 1, 1), .07},
                { new DateTime(2011, 1, 1), .0685},
                { new DateTime(2012, 1, 1), .07},
                { new DateTime(2013, 1, 1), .07},
                { new DateTime(2014, 1, 1), .071},
            };

            public IEnumerable<Tuple<DateTime, double>> Rates
            {
                get
                {
                    return _rates.OrderBy(kvp => kvp.Key).Select(kvp => Tuple.Create(kvp.Key, kvp.Value));
                }
            }
        }
    }
}
