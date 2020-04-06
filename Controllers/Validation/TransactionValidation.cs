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
            BID bd;
            ASK ask;

            
            if (bdL.Count > 0 && askL.Count > 0)
            {
                bd = bdL.First(); 
                ask = new ASK { }; 
                var x = false;

                foreach (ASK _ask in askL)
                {
                    if (_ask.CNP != bd.CNP)
                    {
                        ask = _ask;
                        x = true;
                        break;
                    }
                   
                }
               

                if (x == true && bd.Price >= ask.Price)
                {

                    if (ask.IsIPO == true)
                    {
                        Context.UpdateCompanyTransaction(bd, ask);
                    }
                    else
                    {

                        Context.UpdateIndividualPortfolio(bd, ask);
                    }

                    Order(t);

                }
            }
        }

        public static void ValidateBUY(Transaction t)
        {
            if (Context.CheckUserDebitOrder(t))
            {
                Context.InsertBID(t.BID);
                Order(t);
            }
            
        }

        public static void ValidateSELL(Transaction t)
        {
            Portofolio portofolio = Context.FindPortofolio(t);
            if(portofolio != null && portofolio.Quantity > 0)
            {
                if(t.ASK.Quantity > portofolio.Quantity)
                {
                    t.ASK.Quantity = portofolio.Quantity;
                }
                if (Context.CheckCountASKPortfolio(t))
                {
                    Context.InsertASK(t.ASK);
                    Order(t);
                }
            }
           
        }

    }
}