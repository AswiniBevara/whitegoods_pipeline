using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using VirtualSales.Core.AppServices;
using VirtualSales.Core.Models;

namespace VirtualSales.Core.ViewModels.Tools
{
    public class LifetimeEconomicValueViewModel : ToolViewModel
    {
        private PersonModel _person;
        public LifetimeEconomicValueViewModel(ISharedDataService sharedData, IViewModelLocator locator)
            : base(new Guid("6D2F7E48-8A1E-475C-8297-811D83F46CBE"))
        {
            Name = "Lifetime Economic Value";
            _person = sharedData.Person;

            AddToolPages(
                new Page1(_person, locator.Mode),
                new Page2(this, locator.Mode));
        }

        public class Page1 : ToolPage
        {
            public Page1(PersonModel person, AppMode mode)
            {
                Person = person;
                Mode = mode;
            }

            public AppMode Mode { get; private set; }
            public PersonModel Person { get; private set; }
        }

        public class Page2 : ToolPage
        {
            private readonly LifetimeEconomicValueViewModel _parent;

            private readonly ObservableAsPropertyHelper<bool> _hasValidInputs;
            private readonly ObservableAsPropertyHelper<bool> _hasInvalidInputs;
            private readonly ReactiveList<GapDataPoint> _dataPoints = new ReactiveList<GapDataPoint>();
            private readonly ReactiveList<GapDataPoint> _gapDataPoints = new ReactiveList<GapDataPoint>();
            public AppMode Mode { get; private set; }

            public Page2(LifetimeEconomicValueViewModel parent, AppMode mode)
            {
                Mode = mode;
                _parent = parent;

                

                // Check for valid inputs
                Person.WhenAnyValue(
                    p => p.AnnualIncome,
                    p => p.RetirementAge,
                    p => p.Age,
                    (income, retAge, curAge) => // Check that we have values for the inputs and tha they make sense
                        income.HasValue &&
                        income.Value >= 1000 &&
                        curAge.HasValue &&
                        curAge.Value >= 18 &&
                        retAge.HasValue &&
                        retAge.Value >= curAge
                    )
                    .ToProperty(this, p => p.HasValidInputs, out _hasValidInputs);

                this.WhenAnyValue(p => p.HasValidInputs, p => !p).ToProperty(this, p => p.HasInvalidInputs, out _hasInvalidInputs);
            }

            public string InvalidInputsMessage
            {
                get { return "Invalid inputs: cannot calculate Lifetime Economic Value is of the following is true:\nIncome less than 1000\nCurrent age less than 18\nRetriement age less than current age"; }
            }

            public IReadOnlyReactiveList<GapDataPoint> DataPoints
            {
                get { return _dataPoints; }
            }

            public bool HasValidInputs
            {
                get { return _hasValidInputs.Value; }
            }

            public bool HasInvalidInputs
            {
                get { return _hasInvalidInputs.Value; }
            }

            public PersonModel Person
            {
                get { return _parent._person; }
            }

            protected override void OnNavigatedTo()
            {
                base.OnNavigatedTo();

                using (_dataPoints.SuppressChangeNotifications())
                {
                    _dataPoints.Clear();

                    if (HasValidInputs)
                    {
                        // get the current coverage, if any
                        var current = Person.ExistingCoverage ?? 0;
                        var pt1 = new GapDataPoint("Existing Coverage", current);
                        _dataPoints.Add(new GapDataPoint("", current));
                        _dataPoints.Add(new GapDataPoint("Existing Coverage", current));
                        _dataPoints.Add(new GapDataPoint("", current));

                        // Calc current
                        // (Annual income)(retirement age – current age) – 30% for taxes
                        var lifetime = Math.Round((double)((Person.AnnualIncome * (Person.RetirementAge - Person.Age)) * .7), 0, MidpointRounding.ToEven);


                        _dataPoints.Add(new GapDataPoint("", (int)lifetime));
                        _dataPoints.Add(new GapDataPoint("Lifetime Value", (int)lifetime));
                        _dataPoints.Add(new GapDataPoint("", (int)lifetime));
                    }
                }
            }

            public class GapDataPoint
            {
                public string Category { get; private set; }
                public int Value { get; private set; }

                public GapDataPoint(string category, int value)
                {
                    Category = category;
                    Value = value;
                }
            }
        }

    }


}
