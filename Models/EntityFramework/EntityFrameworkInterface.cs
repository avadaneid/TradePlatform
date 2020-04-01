﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Windows.Forms;
using Models;
using EntityFramework;
using System.Linq;
using System.Data.Entity.Core.Mapping;


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

                        //connect.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Companies] ON;");

                        connect.SaveChanges();

                        //connect.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Companies] ON;");
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



        public static List<CompanyFinancialDetails> CompanyFinancialDetails(string username)
        {
            var cui = FindCompany(username).CUI;
            List<CompanyFinancialDetails> q;

            using (Connect a = new Connect())
            {
                q = a.CompanyFinancialDetails.Where(P => P.CUI == cui).ToList<CompanyFinancialDetails>();
            }

            return q;

        }

    }
}