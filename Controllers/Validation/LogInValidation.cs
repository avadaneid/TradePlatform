﻿using EntityFramework;
using Models;
using System.Linq;

namespace Validation
{
    public static class LogInValidation
    {
        public static bool ValidCredentials { get; set; }
        public static AccountType AccountType { get; set; }

        public static void Validate(string username, string password)
        {
            using (Connect db = new Connect())
            {
                Account account = db.Accounts.Where<Account>(model => model.UserName.Equals(username) && model.Password.Equals(password)).FirstOrDefault();
                Individual individual = db.Individuals.Where(i => i.UserName == username).FirstOrDefault();
                Company company = db.Companies.Where(c => c.UserName == username).FirstOrDefault();


                if (account != null && (individual != null || company != null) )
                {
                    ValidCredentials = true;
                    AccountType = account.AccountType;
                   
                }
                else
                {
                    ValidCredentials = false;

                }

            }
        }

    }
}