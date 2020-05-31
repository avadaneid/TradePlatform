using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityFramework;
using Models;
using Newtonsoft.Json;
using TradingPlatform.Controllers;

namespace Validation
{
  
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

        public static string ValidateBUY(Transaction t)
        {
            if (Context.CheckUserDebitOrder(t))
            {
                Context.InsertBID(t.BID);
                Order(t);
                return Alert(SuccesMessage.TransactionSucces);
            }
            else
            {
                return Alert(ErrorMessage.InsufficientFunds);
            }
            
        }

        public static string ValidateSELL(Transaction t)
        {
            Portofolio portofolio = Context.FindPortofolio(t);
            if(portofolio != null && portofolio.Quantity > 0)
            {
                if (Context.CheckCountASKPortfolio(t))
                {
                    Context.InsertASK(t.ASK);
                    Order(t);
                    return Alert(SuccesMessage.TransactionSucces);
                }
                else
                {
                    return Alert(ErrorMessage.InsufficientShares);
                }

            }
            else
            {
                return Alert(ErrorMessage.InvalidPortfolio);
            }
           
        }
        
        public static string Alert(string message)
        {
            return JsonConvert.SerializeObject(new Response(message));
        }
        
        class Response
        {
            public Response(string msg)
            {
                Message = msg;
            }
            public string Message { get; set; }
        }

        public class ErrorMessage
        {
            public static string InsufficientFunds { get; set; } = "Fonduri insuficiente pentru a realiza tranzactia ! ";
            public static string InsufficientShares { get; set; } = "Actiuni insuficiente in portofoliu pentru a realiza tranzactia ! ";
            public static string InvalidPortfolio { get; set; } = "Portofoliul nu exista ! ";
        }

        public class SuccesMessage
        {
            public static string TransactionSucces { get; set; } = "Tranzactia a fost realizata cu succes ! ";
            public static string TransactionUpdateSucces { get; set; } = "Tranzactia a fost actualizata cu succes ! ";
            public static string TransactionDeleteSucces { get; set; } = "Tranzactia a fost stearsa din sistem cu succes ! ";
        }

    }
  
}