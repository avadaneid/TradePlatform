using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Windows.Forms;
using Models;
using EntityFramework;
using System.Linq;
using System.Data.Entity.Core.Mapping;
using Microsoft.Ajax.Utilities;

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
                                Account = account
                            }

                     );


                break;

                case AccountType.Company:

                    connect.Companies.Add(new Company
                    {   
                        UserName = account.UserName,
                        CUI = account.CUI,
                        CompanyName = account.CompanyName,
                        Account  = account
                    });

                break;
            }
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

        public static Company UpdateCompany(long cui,DateTime dt)
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
                    cfi.PriceBookValue = co.SharePrice / (cfd.TotalEquity/ lst.NumberOfShares);
                    cfi.Capitalisation = 0;
                    cfi.PriceEarningsRatio = co.SharePrice / cfi.EarningPerShare;

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
                    result.SharesCount = lst.NumberOfShares;
                    result.IsListed = true;
                    result.SharePrice = (cfd.TotalEquity * lst.Percent) / lst.NumberOfShares;
                    result.NominalSharePrice = result.SharePrice;
                    result.SharesOnTheMarket = lst.NumberOfShares;

                    UpdateCompany(cui, DateTime.Now);
                    InsertCompanyIndicators(cui, cfd, lst, result);
                    InsertAsk(cui, result);

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
                lst = a.Companies.Where(o => o.IsListed == true).ToList<Company>();


            }
            List<Company> p = lst.DistinctBy(x => x.CUI).ToList();
            return p;
        }
        public static void InsertAsk(long cui,Company c)
        {
            using(Connect cnt = new Connect())
            {
                ASK ask = new ASK();
                ask.CUI = c.CUI;
                ask.CreatedOn = DateTime.Now;
                ask.Price = c.NominalSharePrice;
                ask.Quantity = c.SharesOnTheMarket;
                ask.CompanyName = c.CompanyName;
                cnt.Ask.Add(ask);
                cnt.SaveChanges();
            }
        }

        public static List<ASK> FindASK(long cui)
        {
            List<ASK> ask;
            using (Connect a = new Connect())
            {
                ask = a.Ask.Where(k => k.CUI == cui).ToList();
            }

            return ask;
        }

        public static List<BID> FindBID(long cui)
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

    }
}