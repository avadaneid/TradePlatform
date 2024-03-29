﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Windows.Forms;
using Models;
using System.Linq;
using System.Data.Entity.Core.Mapping;
using Microsoft.Ajax.Utilities;
using Validation;
using EXL = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Web.Mvc;
using System.Web;
using static Validation.TransactionValidation;

namespace EntityFramework
{
    public class Connect : DbContext
    {

        public Connect() : base("name=DatabaseConnection")
        {
           
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Individual> Individuals { get; set; }
        public DbSet<CompanyFinancialDetails> CompanyFinancialDetails { get; set; }
        public DbSet<CompanyFinancialIndicators> CompanyFinancialIndicators { get; set; }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<ASK> Ask { get; set; }
        public DbSet<BID> Bid { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<Portofolio> Portofolios { get; set; }
        public DbSet<TransactionReport> TransactionReports { get; set; }
           
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
               .HasKey(k => k.Id)
               .HasOptional(s => s.Company)
               .WithRequired(d => d.Account);

            modelBuilder.Entity<Account>()
             .HasOptional(s => s.Individual)
             .WithRequired(d => d.Account);

            modelBuilder.Entity<Company>()
               .HasKey(k => k.CUI)
               .HasOptional(s => s.CompanyFinancialDetails)
               .WithRequired(d => d.Company);
               

        }

    }

    public static class Context
    {
        public static void InsertLogIn(Account account)
        {
            try
            {
                using (Connect connect = new Connect())
                {


                    using (var transaction = connect.Database.BeginTransaction())
                    {
                        account.CreatedOn = DateTime.Now;
                        account.Id = Guid.NewGuid();

                        account.Password = Crypt.CryptPassword(account.Password);

                        connect.Accounts.Add(account);

                        connect.SaveChanges();

                        InsertEntity(account, connect);

                        connect.SaveChanges();

                        transaction.Commit();
                    }

                }

            }catch(DbEntityValidationException ex)
            {
                string s = "";
                foreach (var errors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in errors.ValidationErrors)
                    {
                        s += validationError.ErrorMessage + " ";
                        
                    }
                }
                MessageBox.Show(s);
            }
        }

        public static bool VerifyUserName(Account ac)
        {
            using (Connect c = new Connect())
            {
                Account account = c.Accounts.Where(k => k.UserName == ac.UserName).FirstOrDefault();
                if(account != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static void InsertEntity(Account account,Connect connect)
        {
            switch (account.AccountType)
            {
                case AccountType.Individual:

                    connect.Individuals.Add(new Individual

                            {
                                CNP = account.CNP,
                                Name = account.Name,
                                Surname = account.Surname,
                                PhoneNumber = account.PhoneNumber,
                                Address = account.Address,
                                UserName = account.UserName,
                                Account = account,
                                Debit = 60000
                            }

                     );


                break;

                case AccountType.Company:

                    connect.Companies.Add(new Company
                    {
                        UserName = account.UserName,
                        CUI = account.CUI,
                        CompanyName = account.CompanyName,
                        Account = account,
                        Simbol = SetSimbol(account.CompanyName)
                    });

                break;
            }
        }

        public static string SetSimbol(string cn)
        {
            string companyName = cn.Substring(0, 3).ToUpper();
            return companyName;
        }

        public static void InsertDetails(IEnumerable<CompanyFinancialDetails> c,string username)
        {
          
            using (Connect a = new Connect())
            {
                try
                {
                    Company co = Context.FindCompany(username);                  

                    foreach (CompanyFinancialDetails s in c)
                    {

                        SqlScripts.DocumentInsert(a, co, s);
                    }
              
                }
                catch (DbEntityValidationException ex)
                {
                    string s = "";
                    foreach (var errors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in errors.ValidationErrors)
                        {
                            s += validationError.ErrorMessage + " ";
                        }
                    }
                    MessageBox.Show(s);
                }
            }
        }
        
        public static Company FindCompany(string username)
        {
            Company co;
           
            using (Connect a = new Connect())
            {
                     co =  a.Companies
                     .Where(company => company.UserName == username)
                     .FirstOrDefault<Company>();                   
            }

            return co;
        }

        public static Company FindCompany(long cui)
        {
            Company co;

            using (Connect a = new Connect())
            {
                co = a.Companies
                .Where(company => company.CUI == cui)
                .FirstOrDefault<Company>();
            }

            return co;
        }

        public static CompanyFinancialIndicators FindCompanyFinancialDetails(long cui)
        {           
            CompanyFinancialIndicators q;

            using (Connect a = new Connect())
            {
                q = a.CompanyFinancialIndicators.Where(P => P.Cui == cui).FirstOrDefault();
            }

            return q;
        }

        public static CompanyFinancialIndicators FindCompanyFinancialIndicators(long cui)
        {
            CompanyFinancialIndicators cfi;
            using (Connect cn = new Connect())
            {
               cfi =  cn.CompanyFinancialIndicators.Where(c => c.Cui == cui).FirstOrDefault();
            }
            return cfi;
        }

        public static Company UpdateCompanyBeginTransactionDate(long cui,DateTime dt)
        {
            Company co;

            using (Connect a = new Connect())
            {
                co = a.Companies
                .Where(company => company.CUI == cui)
                .FirstOrDefault<Company>();
                co.DateBeginTransaction = dt;
                a.SaveChanges();
            }

            return co;
        }

        public static List<CompanyFinancialDetails> FindCompanyFinancialDetails(string username)
        {
            var cui = FindCompany(username).CUI;
            List<CompanyFinancialDetails> q;

            using (Connect a = new Connect())
            {
                q = a.CompanyFinancialDetails.Where(P => P.CUI == cui).ToList<CompanyFinancialDetails>();
            }

            return q;

        }

        public static CompanyFinancialIndicators InsertCompanyIndicators(long cui,CompanyFinancialDetails cfd,Listing lst,Company co)
        {
            CompanyFinancialIndicators cfi;

            using (Connect o = new Connect())
            {
                using (var transaction = o.Database.BeginTransaction())
                {
                    cfi = new CompanyFinancialIndicators();
                    cfi.Cui = cui;
                    cfi.EarningPerShare = cfd.NetProfit / lst.NumberOfShares;
                    cfi.PriceBookValue = co.MarketSharePrice / (cfd.TotalEquity/ lst.NumberOfShares);
                    cfi.Capitalisation = 0;
                    cfi.PriceEarningsRatio = co.MarketSharePrice / cfi.EarningPerShare;

                    o.CompanyFinancialIndicators.Add(cfi);
                    o.SaveChanges();
                    transaction.Commit();
                }
            }

            return cfi;
        }

        public static void UpdateCompanyForListing(string username, Listing lst)
        {
            using (Connect o = new Connect())
            {
                Company c = FindCompany(username);


                CompanyFinancialDetails cfd = FindCompanyFinancialDetails(username).FirstOrDefault(l => l.TotalEquity > 0);
                if (cfd != null)
                {
                    var cui = c.CUI;
                    var result = o.Companies.Where(i => i.CUI == cui).DistinctBy(x => x.CUI).FirstOrDefault();
                    result.NumberOfTotalShares = lst.NumberOfShares;
                    result.IsListed = true;
                    result.MarketSharePrice = (cfd.TotalEquity * lst.Percent) / lst.NumberOfShares;
                    result.NominalSharePrice = result.MarketSharePrice;
                    result.SharesOnInitialIPO = lst.NumberOfShares;

                    UpdateCompanyBeginTransactionDate(cui, DateTime.Now);
                    InsertCompanyIndicators(cui, cfd, lst, result);
                    InsertAskCompany(cui, result);

                    lst.Cui = cui;
                    o.Listings.Add(lst);
                    o.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Financial details file is not loaded !");
                }
            }
        }

        public static List<Company> FindListedCompanies()
        {
            List<Company> lst;
            using (Connect a = new Connect())
            {
                lst = a.Companies.Where(o => o.IsListed == true).DistinctBy(x => x.CUI).ToList();
            }          
            return lst;
        }
        public static void InsertAskCompany(long cui,Company c)
        {
            using(Connect cnt = new Connect())
            {
                ASK ask = new ASK();
                ask.CUI = c.CUI;
                ask.CreatedOn = DateTime.Now;
                ask.Price = c.NominalSharePrice;
                ask.Quantity = c.SharesOnInitialIPO;
                ask.CompanyName = c.CompanyName;
                ask.IsIPO = true;
                cnt.Ask.Add(ask);
                cnt.SaveChanges();
            }
        }

        public static List<ASK> FindASKforListing(long cui)
        {
            List<ASK> ask;
            using (Connect a = new Connect())
            {
                ask = a.Ask.Where(k => k.CUI == cui).ToList();
            }

            return ask;
        }

        public static List<BID> FindBIDforListing(long cui)
        {
            List<BID> bid;
            using (Connect a = new Connect())
            {
                bid = a.Bid.Where(k => k.CUI == cui).ToList();
            }

            return bid;
        }

        public static Individual FindIndividual(string username)
        {
            List<Individual> l;
            using (Connect c = new Connect())
            {
               l = c.Individuals.Where(ind => ind.UserName == username).ToList();
            }
            Individual i = l.DistinctBy(m => m.UserName == username).FirstOrDefault();
            return i;
        }

        public static Individual FindIndividual(long cnp)
        {
            List<Individual> l;
            using (Connect c = new Connect())
            {
                l = c.Individuals.Where(ind => ind.CNP == cnp).ToList();
            }
            Individual i = l.DistinctBy(m => m.CNP == cnp).FirstOrDefault();
            return i;
        }

        public static void InsertBID(BID b )
        {
            using(Connect a = new Connect())
            {
                a.Bid.Add(b);
                a.SaveChanges();
            }
        }

        public static void InsertASK(ASK ask)
        {
            using (Connect a = new Connect())
            {
                a.Ask.Add(ask);
                a.SaveChanges();
            }
        }

        public static List<BID> OrderOrderedBID(Transaction t)
        {
            List<BID> lst;
            using (Connect cnt = new Connect())
            {
                lst = cnt.Bid.Where(l => l.CUI == t.CUI).OrderByDescending(j => j.Price).ThenBy(m => m.CreatedOn).ToList();
            }
            return lst;
        }

        public static List<ASK> OrderOrderedASK(Transaction t)
        {
            List<ASK> lst;
            using (Connect cnt = new Connect())
            {
                lst = cnt.Ask.Where(l => l.CUI == t.CUI).OrderBy(j => j.Price).ThenBy(m => m.CreatedOn).ToList();
            }
            return lst;
        }

        public static void UpdateCompanyTransaction(BID bid_p,ASK ask_p)
        {

            using(Connect cnt = new Connect())
            {
                Company c = cnt.Companies
                .Where(company => company.CUI == bid_p.CUI)
                .FirstOrDefault<Company>();
                c.MarketSharePrice = ask_p.Price;

               
                ASK ask = cnt.Ask.Where(p => p.Id == ask_p.Id).FirstOrDefault();
                BID bid = cnt.Bid.Where(p => p.Id == bid_p.Id).FirstOrDefault();
                int quantityBID;

                if (ask_p.Quantity > bid_p.Quantity)
                {
                    var remain_ask = ask_p.Quantity - bid_p.Quantity;
                    ask.Quantity = remain_ask;

                    c.Debit += ask_p.Price * bid_p.Quantity;

                    c.SharesOnInitialIPO -= bid_p.Quantity;

                    cnt.Bid.Remove(bid);

                    quantityBID = bid_p.Quantity;

                    InsertTransactions(new Transactions
                    {
                        BuyFrom = ask_p.CUI,
                        SellTo = bid_p.CNP,
                        CreatedOn = DateTime.Now,
                        FromIPO = true,
                        Price = ask_p.Price,
                        Quantity = bid_p.Quantity,
                        CompanyIdentifier = ask_p.CUI,
                        CompanyName = ask_p.CompanyName

                    });
                }
                else if(ask_p.Quantity == bid_p.Quantity)
                {
                    c.Debit += ask_p.Price * bid_p.Quantity;
                    c.SharesOnInitialIPO -= bid_p.Quantity;
                    cnt.Ask.Remove(ask);
                    cnt.Bid.Remove(bid);

                    quantityBID = bid_p.Quantity;
                    InsertTransactions(new Transactions
                    {
                        BuyFrom = ask_p.CUI,
                        SellTo = bid_p.CNP,
                        CreatedOn = DateTime.Now,
                        FromIPO = false,
                        Price = ask_p.Price,
                        Quantity = bid_p.Quantity,
                        CompanyIdentifier = ask_p.CUI,
                        CompanyName = ask_p.CompanyName
                    });
                }
                else
                {
                    var remain_bid = bid_p.Quantity - ask_p.Quantity;
                    bid.Quantity = remain_bid;

                    c.Debit += ask_p.Price * ask_p.Quantity;

                    c.SharesOnInitialIPO = 0;
                    cnt.Ask.Remove(ask);

                    quantityBID = ask_p.Quantity;
                    InsertTransactions(new Transactions
                    {
                        BuyFrom = ask_p.CUI,
                        SellTo = bid_p.CNP,
                        CreatedOn = DateTime.Now,
                        FromIPO = false,
                        Price = ask_p.Price,
                        Quantity = ask_p.Quantity,
                        CompanyIdentifier = ask_p.CUI,
                        CompanyName = ask_p.CompanyName
                    });
                }

                c.MarketSharePrice = ask_p.Price;

                ask_p.Quantity = quantityBID;

                UpdatePortfolio(bid_p,ask_p);            

                cnt.SaveChanges();

                UpdateFinancialIndicators(ask_p.CUI);
                VerifyAndDeleteEmptyPortfolio();
            }

        }

        public static void UpdateIndividualPortfolio(BID bid_p,ASK ask_p)
        {
           
            using (Connect cnt = new Connect())
            {
                Individual i = cnt.Individuals.Where(p => p.CNP == ask_p.CNP).FirstOrDefault();            
                Portofolio portf = cnt.Portofolios.Where(q => q.CNP == ask_p.CNP && q.CUI == ask_p.CUI).FirstOrDefault();

                ASK ask = cnt.Ask.Where(p => p.Id == ask_p.Id).FirstOrDefault();
                BID bid = cnt.Bid.Where(p => p.Id == bid_p.Id).FirstOrDefault();

                int bd_q;

                if (ask_p.Quantity > bid_p.Quantity)
                {
                    var remain_ask = ask_p.Quantity - bid_p.Quantity;
                    ask.Quantity = remain_ask;

                    i.Debit += ask_p.Price * bid_p.Quantity;

                    portf.Quantity -= bid_p.Quantity;
                    cnt.Bid.Remove(bid);

                    bd_q = bid_p.Quantity;

                    InsertTransactions(new Transactions
                    {
                        BuyFrom = ask_p.CNP,
                        SellTo = bid_p.CNP,
                        CreatedOn = DateTime.Now,
                        FromIPO = false,
                        Price = ask_p.Price,
                        Quantity = bid_p.Quantity,
                        CompanyIdentifier = ask_p.CUI,
                        CompanyName = ask_p.CompanyName
                    });


                }
                else if (ask_p.Quantity == bid_p.Quantity)
                {
                    i.Debit += ask_p.Price * bid_p.Quantity;
                    portf.Quantity -= bid_p.Quantity;
                    cnt.Ask.Remove(ask);
                    cnt.Bid.Remove(bid);

                    bd_q = bid_p.Quantity;


                    InsertTransactions(new Transactions
                    {
                        BuyFrom = ask_p.CNP,
                        SellTo = bid_p.CNP,
                        CreatedOn = DateTime.Now,
                        FromIPO = false,
                        Price = ask_p.Price,
                        Quantity = bid_p.Quantity,
                        CompanyIdentifier = ask_p.CUI,
                        CompanyName = ask_p.CompanyName
                    });
                }
                else
                {
                    var remain_bid = bid_p.Quantity - ask_p.Quantity;
                    bid.Quantity = remain_bid;

                    i.Debit += ask_p.Price * ask_p.Quantity;

                    portf.Quantity -= ask_p.Quantity;
                    cnt.Ask.Remove(ask);

                    bd_q = ask_p.Quantity;

                   InsertTransactions(new Transactions
                    {
                        BuyFrom = ask_p.CNP,
                        SellTo = bid_p.CNP,
                        CreatedOn = DateTime.Now,
                        FromIPO = false,
                        Price = ask_p.Price,
                        Quantity = ask_p.Quantity,
                        CompanyIdentifier = ask_p.CUI,
                        CompanyName = ask_p.CompanyName
                   });
                }              

                Company comp = cnt.Companies.Where(c => c.CUI == bid_p.CUI).FirstOrDefault();               
                comp.MarketSharePrice = ask_p.Price;

                ask_p.Quantity = bd_q;

                UpdatePortfolio(bid_p,ask_p);               

                cnt.SaveChanges();

                UpdateFinancialIndicators(ask_p.CUI);
                VerifyAndDeleteEmptyPortfolio();
            }
        }

        public static void UpdatePortfolio(BID b,ASK k)
        {
            using (Connect cnt = new Connect())
            {
                Individual i = cnt.Individuals.Where(p => p.CNP == b.CNP).FirstOrDefault();

                i.Debit -= k.Price * k.Quantity;

                Portofolio portf = cnt.Portofolios.Where(q => q.CNP == b.CNP && q.CUI == b.CUI).FirstOrDefault();
                if (portf == null)
                {
                    Portofolio portofolio = new Portofolio();
                    portofolio.CNP = b.CNP;
                    portofolio.CompanyName = k.CompanyName;
                    portofolio.CreatedOn = DateTime.Now;
                    portofolio.CUI = k.CUI;
                    portofolio.Quantity = k.Quantity;
                    cnt.Portofolios.Add(portofolio);

                }
                else if (portf != null)
                {

                    portf.Quantity += k.Quantity;
                    portf.CompanyName = k.CompanyName;

                }

                cnt.SaveChanges();
            }

            VerifyAndDeleteEmptyPortfolio();
        }

        public static Portofolio FindPortofolio(Transaction t)
        {
            Portofolio port;

            using (Connect c = new Connect())
            {
                port = c.Portofolios.Where(p => p.CUI == t.CUI && p.CNP == t.ASK.CNP).FirstOrDefault();
            }

            return port;
        }
   
        public static void InsertTransactions (Transactions t)
        {
            using(Connect c = new Connect())
            {
                c.Transactions.Add(t);
                c.SaveChanges();
            }
        }

        public static bool CheckCountASKPortfolio(Transaction t)
        {
            List<ASK> countASK;
            List<Portofolio> countPortfolio;
            int cntASK = 0;
            int cntPort = 0;
            using (Connect a  = new Connect())
            {
                countASK = a.Ask.Where(l => l.CNP == t.ASK.CNP && t.ASK.CUI == l.CUI).ToList();

                countPortfolio = a.Portofolios.Where(l => l.CNP == t.ASK.CNP && t.ASK.CUI == l.CUI).ToList();

            }
            foreach(ASK a in countASK)
            {
                cntASK += a.Quantity;
            }
            foreach(Portofolio p in countPortfolio)
            {
                cntPort += p.Quantity;
            }

            int sum = t.ASK.Quantity + cntASK;

            if(sum > cntPort)
            {
                return false;
            }
            else
            {
                return true;
            }
           
        }

        public static bool CheckUserDebitOrder(Transaction t)
        {
            decimal debit = FindIndividual(t.BID.CNP).Debit;
            List<BID> lstBD;
            decimal sumBIDDebit = 0;

            using (Connect c = new Connect())
            {
                lstBD = c.Bid.Where(l => l.CNP == t.BID.CNP).ToList();
            }

            foreach(BID b in lstBD)
            {
                sumBIDDebit += (b.Price * b.Quantity); 
            }

            if((sumBIDDebit + (t.BID.Quantity * t.BID.Price)) > debit)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public static List<Portofolio> FindPortofolios(long cnp)
        {
            List<Portofolio> p;

            using (Connect a = new Connect())
            {
                p = a.Portofolios.Where(t => t.CNP == cnp).ToList();
            }

            return p;
        }

        public static List<BID> FindBIDsForIndividual(long cnp)
        {
            List<BID> lstBD;
            using (Connect c = new Connect())
            {
                lstBD = c.Bid.Where(b => b.CNP == cnp).ToList();
            }
            return lstBD;
        }

        public static List<ASK> FindASKsForIndividual(long cnp)
        {
            List<ASK> lstASK;
            using (Connect c = new Connect())
            {
                lstASK = c.Ask.Where(b => b.CNP == cnp).ToList();
            }
            return lstASK;
        }
     
        public static void UpdateFinancialIndicators(long cui)
        {
            CompanyFinancialIndicators cfi;
            Company comp;
            CompanyFinancialDetails cfd;

            using (Connect c = new Connect())
            {
                cfi = c.CompanyFinancialIndicators.Where(w => w.Cui == cui).FirstOrDefault();
                comp = c.Companies.Where(d => d.CUI == cui).FirstOrDefault();
                cfd = c.CompanyFinancialDetails.Where(l => l.CUI == cui).FirstOrDefault();

                cfi.Capitalisation = (comp.NumberOfTotalShares - comp.SharesOnInitialIPO) * comp.MarketSharePrice;
                cfi.PriceBookValue = comp.MarketSharePrice / (cfd.TotalEquity / comp.NumberOfTotalShares);
                cfi.PriceEarningsRatio = comp.MarketSharePrice / cfi.EarningPerShare;

                c.SaveChanges();
            }
        }

        public static void VerifyAndDeleteEmptyPortfolio()
        {
            List<Portofolio> lp;
            using (Connect c = new Connect())
            {
                lp = c.Portofolios.Where(p => p.Quantity == 0).ToList();

                if (lp != null)
                {
                    foreach(Portofolio p in lp)
                    {
                        c.Portofolios.Remove(p);
                    }
                }
                c.SaveChanges();
            }
        }

        public static List<TransactionReport> TransactionReport(long cui)
        {
            List<TransactionReport> lst;

            using (Connect c = new Connect())
            {
                lst = c.TransactionReports.Where(co => co.CompanyIdentifier == cui).
                    OrderBy(b => b.CompanyName).OrderBy(l => l.Date).ToList();
            }

            return lst;
        }

        public static string UpdateBID(Term term)
        {
            string result = "";
            using(Connect c = new Connect())
            {

               foreach(UpdateOrder uo in term.Terminal)
               {
                    BID b = c.Bid.Where(p => p.Id == uo.id).FirstOrDefault();

                    Transaction t = new Transaction
                    {
                       BID = new BID{CNP = b.CNP,Quantity = uo.quantity,Price = uo.price},
                    };
                 
                    if (uo.quantity == 0 || uo.price == 0)
                    {
                        c.Bid.Remove(b);
                        c.SaveChanges();
                        result = SuccesMessage.TransactionDeleteSucces;
                    }
                    else
                    {
                        if (CheckUserDebitOrder(t,uo))
                        {
                            b.Price = uo.price;
                            b.Quantity = uo.quantity;
                            c.SaveChanges();
                            result = SuccesMessage.TransactionSucces;
                        }
                        else
                        {
                            result = ErrorMessage.InsufficientFunds;
                        }
                                          
                    }
                    TransactionValidation.Order(new Transaction { CUI = b.CUI });
               }
            }
            return Alert(result);
        }

        public static bool CheckUserDebitOrder(Transaction t, UpdateOrder UpdateOrder)
        {
            decimal debit = FindIndividual(t.BID.CNP).Debit;
            List<BID> lstBD;
            decimal sumBIDDebit = 0;

            using (Connect c = new Connect())
            {
                lstBD = c.Bid.Where(l => l.CNP == t.BID.CNP && l.Id != UpdateOrder.id).ToList();
            }

            foreach (BID b in lstBD)
            {
                sumBIDDebit += (b.Price * b.Quantity);
            }

            if ((sumBIDDebit + (t.BID.Quantity * t.BID.Price)) > debit)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public static string UpdateASK(Term term)
        {
            string result = "";
            using (Connect c = new Connect())
            {

                foreach (UpdateOrder uo in term.Terminal)
                {
                    ASK ask = c.Ask.Where(p => p.Id == uo.id).FirstOrDefault();

                    Transaction t = new Transaction
                    {
                        ASK = new ASK { CNP = ask.CNP, Quantity = uo.quantity, Price = uo.price,CUI = ask.CUI },
                    };

                    if (uo.quantity == 0 || uo.price == 0)
                    {
                        c.Ask.Remove(ask);
                        c.SaveChanges();
                        result = SuccesMessage.TransactionDeleteSucces;
                    }
                    else
                    {
                        if (CheckCountASKPortfolio(uo,t))
                        {
                            ask.Price = uo.price;
                            ask.Quantity = uo.quantity;
                            c.SaveChanges();
                            result = SuccesMessage.TransactionSucces;
                        }
                        else
                        {
                            result = ErrorMessage.InsufficientShares;
                        }

                    }
                    TransactionValidation.Order(new Transaction { CUI = ask.CUI });
                }
            }

            return Alert(result);
        }

        public static bool CheckCountASKPortfolio(UpdateOrder term,Transaction t)
        {
            List<ASK> countASK;
            List<Portofolio> countPortfolio;
            int cntASK = 0;
            int cntPort = 0;
            using (Connect a = new Connect())
            {
                countASK = a.Ask.Where(l => l.CNP == t.ASK.CNP && t.ASK.CUI == l.CUI && l.Id != term.id).ToList();

                countPortfolio = a.Portofolios.Where(l => l.CNP == t.ASK.CNP && t.ASK.CUI == l.CUI).ToList();

            }
            foreach (ASK a in countASK)
            {
                cntASK += a.Quantity;
            }
            foreach (Portofolio p in countPortfolio)
            {
                cntPort += p.Quantity;
            }

            int sum = t.ASK.Quantity + cntASK;

            if (sum > cntPort)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public static decimal PortofolioValue(long cnp)
        {
            decimal value = 0;
            using (Connect c = new Connect())
            {
                List<Portofolio> p = c.Portofolios.Where(po => po.CNP == cnp).ToList();
                List<Company> comp = c.Companies.DistinctBy(l => l.CUI).ToList();

                foreach(Portofolio po in p)
                {
                    foreach(Company com in comp)
                    {
                        if (po.CUI == com.CUI)
                        {
                            value += po.Quantity * com.MarketSharePrice;
                        }
                    }
                }

            }
            return value;
        }

        public static List<Transactions> FindTransactionHistory(string username)
        {
            Individual ind = FindIndividual(username);
            List<Transactions> lst;

            using (Connect c = new Connect())
            {
                lst = c.Transactions.Where(t => t.BuyFrom == ind.CNP || t.SellTo == ind.CNP).OrderByDescending(cr => cr.CreatedOn).ToList();
            }
            return lst;
        }

        public static List<Transactions> FindTransactionHistoryForCompany(long cui)
        {
            List<Transactions> lst;

            using (Connect c = new Connect())
            {
                lst = c.Transactions.Where(t => t.CompanyIdentifier == cui).OrderByDescending(cr => cr.CreatedOn).ToList();
            }
            return lst;
        }      

    }
}