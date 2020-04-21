using EntityFramework;
using Models;
using System.Linq;
using CRIPT = BCrypt.Net;

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
                Account account = db.Accounts.Where<Account>(model => model.UserName.Equals(username)).FirstOrDefault();
                bool v = false;

                if (account != null)
                {
                     v = Crypt.VerifyPassword(password, account.Password);
                }
            
                Individual individual = db.Individuals.Where(i => i.UserName == username).FirstOrDefault();
                Company company = db.Companies.Where(c => c.UserName == username).FirstOrDefault();


                if (v == true && (individual != null || company != null))
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

    public class Crypt
    {
        public static string CryptPassword(string password)
        {
            string passwordHash = CRIPT.BCrypt.HashPassword(password);
            return passwordHash;
        }
        public static bool VerifyPassword(string userpassword, string databasepassword)
        {
            bool isValid = CRIPT.BCrypt.Verify(userpassword, databasepassword);
            return isValid;
        }

    }
}