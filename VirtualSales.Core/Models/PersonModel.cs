using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using System.Reactive.Linq;
using VirtualSales.Models;

namespace VirtualSales.Core.Models
{
    public class PersonModel : ReactiveObject
    {
        private string _firstName;
        private string _lastName;
        private Gender? _gender;
        private DateTime? _dateOfBirth;
        private int? _numOfDependents;
        private bool _isSmoker;
        private int? _coverageAmountRequesting;
        private int? _existingCoverage;
        private int? _retirementAge;
        private int? _annualIncome;
        private int? _annualIncomeGrowthPercent;
        private string _addr1, _addr2, _city, _state, _zip;
        private readonly ObservableAsPropertyHelper<int?> _ageObservable; 

        public PersonModel()
        {
            _isSmoker = false;
            this.WhenAnyValue(p => p.DateOfBirth).Select(p =>
                                                                 {
                                                                     if (!p.HasValue) return null;
                                                                     var dob = p.Value;
                                                                     var today = DateTime.Today;
                                                                     int? age = today.Year - dob.Year;
                                                                     if (dob > today.AddYears(-age.Value)) age--;
                                                                     return age;
                                                                 }).ToProperty(this, p => p.Age, out _ageObservable);
        }

        public string FirstName
        {
            get { return _firstName; }
            set { this.RaiseAndSetIfChanged(ref _firstName, value); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { this.RaiseAndSetIfChanged(ref _lastName, value); }
        }

        public Gender? Gender
        {
            get { return _gender; }
            set { this.RaiseAndSetIfChanged(ref _gender, value); }
        }

        public DateTime? DateOfBirth
        {
            get { return _dateOfBirth; }
            set { this.RaiseAndSetIfChanged(ref _dateOfBirth, value); }
        }

        public int? NumOfDependents
        {
            get { return _numOfDependents; }
            set { this.RaiseAndSetIfChanged(ref _numOfDependents, value); }            
        }

        public int? AnnualIncome
        {
            get { return _annualIncome; }
            set { this.RaiseAndSetIfChanged(ref _annualIncome, value); }
        }

        public int? AnnualIncomeGrowthPercent
        {
            get { return _annualIncomeGrowthPercent; }
            set { this.RaiseAndSetIfChanged(ref _annualIncomeGrowthPercent, value); }
        }

        public int? RetirementAge
        {
            get { return _retirementAge; }
            set { this.RaiseAndSetIfChanged(ref _retirementAge, value); }
        }

        public int? ExistingCoverage
        {
            get { return _existingCoverage; }
            set { this.RaiseAndSetIfChanged(ref _existingCoverage, value); }
        }

        public int? CoverageAmountRequesting
        {
            get { return _coverageAmountRequesting; }
            set { this.RaiseAndSetIfChanged(ref _coverageAmountRequesting, value); }
        }

        public bool IsSmoker
        {
            get { return _isSmoker; }
            set { this.RaiseAndSetIfChanged(ref _isSmoker, value); }
        }

        public int? Age
        {
            get { return _ageObservable.Value; }
        }

        public string Addr1
        {
            get { return _addr1; }
            set { this.RaiseAndSetIfChanged(ref _addr1, value); }
        }

        public string Addr2
        {
            get { return _addr2; }
            set { this.RaiseAndSetIfChanged(ref _addr2, value); }
        }

        public string City
        {
            get { return _city  ; }
            set { this.RaiseAndSetIfChanged(ref _city, value); }
        }

        public string State
        {
            get { return _state; }
            set { this.RaiseAndSetIfChanged(ref _state, value); }
        }

        public string Zip
        {
            get { return _zip; }
            set { this.RaiseAndSetIfChanged(ref _zip, value); }
        }
    }
}