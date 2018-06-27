using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualSales.Models
{
    public class IllustrationRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public Gender? Gender { get; set; }
        public bool? IsSmoker { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        public int? Age { get; set; }
        public int? NumOfDependents { get; set; }

        public int? RetirementAge { get; set; }
        public int? ExistingCoverage { get; set; }
        public int? RequestedCoverage { get; set; }
        public int? AnnualIncome { get; set; }
        public int? AnnualIncomeGrowthPercentage { get; set; }

        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}
