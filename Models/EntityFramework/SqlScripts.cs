using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EntityFramework
{
    public static class SqlScripts
    {
        public static void DocumentInsert(Connect a,Company co,CompanyFinancialDetails cfd)
        {
            a.Database.ExecuteSqlCommand(@"INSERT INTO [dbo].[CompanyFinancialDetails]
            ([Company_CUI]
           ,[Quarter]
           ,[TotalTangibleAssets]
           ,[Shares]
           ,[TotalCurrentAssets]
           ,[Inventories]
           ,[Receivables]
           ,[Cash]
           ,[ShortTermInvestments]
           ,[Prepayments]
           ,[TotalOneYearDebts]
           ,[FinancialOneYearDebts]
           ,[CommercialOneYearDebts]
           ,[ReceivablesCurrentDebts]
           ,[NetAssets]
           ,[LongTermDebts]
           ,[FinancialLongTerDebts]
           ,[RevenueInAdvance]
           ,[SubscribedCapital]
           ,[TotalEquity]
           ,[TotalDebts]
           ,[NetTurnover]
           ,[TotalOperatingIncome]
           ,[ValueAdjustments]
           ,[TotalOperatingExpenses]
           ,[NetOperatingIncome]
           ,[SharesIncome]
           ,[InterestIncome]
           ,[TotalFinancialRevenues]
           ,[InterestExpenses]
           ,[TotalFinanciarExpenses]
           ,[FinancialResult]
           ,[TotalRevenues]
           ,[TotalExpenses]
           ,[GrossProfit]
           ,[NetProfit]
           ,[NumberOfEmployees]
           ,[Year])

            values
           
           ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},
		   {31},{32},{33},{34},{35},{36},{37})", co.CUI, cfd.Quarter, cfd.TotalTangibleAssets, cfd.Shares, cfd.TotalCurrentAssets, cfd.Inventories, cfd.Receivables,

           cfd.Cash, cfd.ShortTermInvestments, cfd.Prepayments, cfd.TotalOneYearDebts, cfd.FinancialOneYearDebts, cfd.CommercialOneYearDebts,

           cfd.ReceivablesCurrentDebts, cfd.NetAssets, cfd.LongTermDebts, cfd.FinancialLongTerDebts, cfd.RevenueInAdvance, cfd.SubscribedCapital,
           cfd.TotalEquity, cfd.TotalDebts, cfd.NetTurnover, cfd.TotalOperatingIncome, cfd.ValueAdjustments, cfd.TotalOperatingExpenses, cfd.NetOperatingIncome,
           cfd.SharesIncome, cfd.InterestIncome, cfd.TotalFinancialRevenues, cfd.InterestExpenses, cfd.TotalFinanciarExpenses, cfd.FinancialResult,
           cfd.TotalRevenues, cfd.TotalExpenses, cfd.GrossProfit, cfd.NetProfit, cfd.NumberOfEmployees, cfd.Year
           );

        }
    }
}