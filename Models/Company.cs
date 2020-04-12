using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Models
{
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long CUI { get; set; }

        public string UserName { get; set; }

        public string CompanyName { get; set; }

        public string Simbol { get; set; }

        public int NumberOfTotalShares { get; set; }
        
        public decimal MarketSharePrice { get; set; }

        public decimal NominalSharePrice { get; set; }

        public bool IsListed { get; set; }

        public int SharesOnInitialIPO { get; set; }

        public decimal Debit { get; set; }

        public DateTime? DateBeginTransaction { get; set; }

        public virtual Account Account { get; set; }

        public virtual CompanyFinancialDetails CompanyFinancialDetails { get; set; }

    }

    public class CompanyFinancialDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public decimal? CUI { get; set; }
        public decimal? Year { get; set; }
        public decimal? Quarter { get; set; }
        public decimal? TotalTangibleAssets { get; set; } 
        public decimal? Shares { get; set; }
        public decimal? TotalCurrentAssets { get; set; }
        public decimal? Inventories { get; set; }
        public decimal? Receivables { get; set; }
        public decimal? Cash { get; set; }
        public decimal? ShortTermInvestments { get; set; }
        public decimal? Prepayments { get; set; }
        public decimal? TotalOneYearDebts { get; set; }
        public decimal? FinancialOneYearDebts { get; set; }
        public decimal? CommercialOneYearDebts { get; set; }
        public decimal? ReceivablesCurrentDebts { get; set; }
        public decimal? NetAssets { get; set; }
        public decimal? LongTermDebts { get; set; }
        public decimal? FinancialLongTerDebts { get; set; }
        public decimal? RevenueInAdvance { get; set; }
        public decimal? SubscribedCapital { get; set; }
        public decimal TotalEquity { get; set; }
        public decimal? TotalDebts { get; set; }
        public decimal? NetTurnover { get; set; }
        public decimal? TotalOperatingIncome { get; set; }
        public decimal? ValueAdjustments { get; set; }
        public decimal? TotalOperatingExpenses { get; set; }
        public decimal? NetOperatingIncome { get; set; }
        public decimal? SharesIncome { get; set; }
        public decimal? InterestIncome { get; set; }
        public decimal? TotalFinancialRevenues { get; set; }
        public decimal? InterestExpenses { get; set; }
        public decimal? TotalFinanciarExpenses { get; set; }
        public decimal? FinancialResult { get; set; }
        public decimal? TotalRevenues { get; set; }
        public decimal? TotalExpenses { get; set; }
        public decimal? GrossProfit { get; set; }
        public decimal NetProfit { get; set; }
        public decimal? NumberOfEmployees { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual Company Company { get; set; }

    }

    public class Listing
    {
        [Key]
        public decimal Cui { get; set; }
        public decimal Percent { get; set; }
        public int NumberOfShares { get; set; }

    }

    public class CompanyFinancialIndicators
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Cui { get; set; }
        public decimal PriceEarningsRatio { get; set; }
        public decimal Capitalisation { get; set; }
        public decimal PriceBookValue { get; set; }
        public decimal EarningPerShare { get; set; }
        public decimal? DividendYield { get; set; }
        public decimal? Dividend { get; set; }
    } 
    
}