using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityFramework;
using Models;

namespace Validation
{
    public class Outcome
    {
        public bool IsSucces { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class TransactionValidation
    {
        public static void Order(Transaction t)
        {
            List<BID> bdL =  Context.OrderOrderedBID(t);
            List<ASK> askL = Context.OrderOrderedASK(t);
            BID bd = bdL.First();
            ASK ask = askL.First();

            if (bd.Price >= ask.Price)
            {

                    if(ask.IsIPO == true)
                    {
                        Context.UpdateCompanyTransaction(bd,ask);
                    }
                    Context.UpdateIndividualPortfolio(bd, ask);

            }
        }

        public static void ValidateBUY(Transaction t)
        {
            Individual individual = Context.FindIndividual(t.BID.CNP);
            if(individual.Debit >= (t.BID.Price * t.BID.Quantity))
            {
                Context.InsertBID(t.BID);
                Order(t);
            }
        }
    }
}