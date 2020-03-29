using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using Models;
using EntityFramework;

namespace FileProcessing
{
    public class PathL
    {
        public string MapPath { get; set; }

        public PathL()
        {
            this.MapPath = String.Format($"{HttpRuntime.AppDomainAppPath}App_Data\\uploads\\FinancialDetails.csv").Replace(@"\\", @"\");
        }

    }
    public static class FileProcessing
    {
        public static string[] reader = Array.Empty<string>();
        public static string[] item = Array.Empty<string>();
        public static List<CompanyFinancialDetails> lst;
        public static PathL _path;


        public static void Read(string username)
        {
            _path = new PathL();
            lst = new List<CompanyFinancialDetails>();
            reader = System.IO.File.ReadAllLines(_path.MapPath);

            for (var i = 0; i < reader.Length; i++)
            {
                item = reader[i].ToString().Split(',');

                try
                {
                    lst.Add(new CompanyFinancialDetails
                    {
                        #region Propreties
                        Year = int.Parse(item[0]),
                        Quarter = int.Parse(item[1]),
                        TotalTangibleAssets = decimal.Parse(item[2]),
                        Shares = decimal.Parse((item[3]) != null && item[3] != "" ? item[3] : "0"),
                        TotalCurrentAssets = decimal.Parse(item[4]),
                        Inventories = decimal.Parse(item[5]),
                        Receivables = decimal.Parse(item[6]),
                        Cash = decimal.Parse(item[7]),
                        ShortTermInvestments = decimal.Parse((item[8]) != null && item[8] != "" ? item[8] : "0"),
                        Prepayments = decimal.Parse(item[9]),
                        TotalOneYearDebts = decimal.Parse(item[10]),
                        FinancialOneYearDebts = decimal.Parse(item[11]),
                        CommercialOneYearDebts = decimal.Parse(item[12]),
                        ReceivablesCurrentDebts = decimal.Parse(item[13]),
                        NetAssets = decimal.Parse(item[14]),
                        LongTermDebts = decimal.Parse(item[15]),
                        FinancialLongTerDebts = decimal.Parse(item[16]),
                        RevenueInAdvance = decimal.Parse(item[17]),
                        SubscribedCapital = decimal.Parse(item[18]),
                        TotalEquity = decimal.Parse(item[19]),
                        TotalDebts = decimal.Parse((item[20]) == null && item[20] != "" ? item[20] : "0"),
                        NetTurnover = decimal.Parse(item[21]),
                        TotalOperatingIncome = decimal.Parse(item[22]),
                        ValueAdjustments = decimal.Parse(item[23]),
                        TotalOperatingExpenses = decimal.Parse(item[24]),
                        NetOperatingIncome = decimal.Parse(item[25]),
                        SharesIncome = decimal.Parse((item[26]) != null && item[26] != "" ? item[26] : "0"),
                        InterestIncome = decimal.Parse(item[27]),
                        TotalFinancialRevenues = decimal.Parse(item[28]),
                        InterestExpenses = decimal.Parse(item[29]),
                        TotalFinanciarExpenses = decimal.Parse(item[30]),
                        FinancialResult = decimal.Parse(item[31]),
                        TotalRevenues = decimal.Parse(item[32]),
                        TotalExpenses = decimal.Parse(item[33]),
                        GrossProfit = decimal.Parse(item[34]),
                        NetProfit = decimal.Parse(item[35]),
                        NumberOfEmployees = decimal.Parse(item[36])
                        #endregion                      
                    });

                }
                catch (Exception ex)
                {
                    MessageBox.Show("error " + ex);
                }

            }           

            Context.InsertDetails(lst,username);

        }


    }
}