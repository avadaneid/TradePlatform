using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Models
{
    public class BID
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        public long CUI { get; set; }
        public long CNP { get; set; }
        public string CompanyName { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

    }

    public class ASK
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        public long CUI { get; set; }
        public long CNP { get; set; }
        public string CompanyName { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal Price { get; set; }
        public bool IsIPO { get; set; }
        public int Quantity { get; set; }
    }

    public class Transaction
    {
        public ASK ASK { get; set; }
        public BID BID { get; set; }
        public long CUI { get; set; }
        public long CNP { get; set; }

    }

    public class Portofolio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public long CNP { get; set; }
        public long CUI { get; set; }
        public string CompanyName { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedOn { get; set; }

    }

    public class Transactions
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public long SellTo { get; set; }
        public long BuyFrom { get; set; }
        public bool FromIPO { get; set; }
        public long CompanyIdentifier { get; set; }
        public string CompanyName { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    [Table("TransactionReport")]
    public class TransactionReport
    {
        [Key]
        public Guid Id { get; set; }
        public int CompanyIdentifier { get; set; }
        public string CompanyName { get; set; }
        public DateTime Date { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosingPrice { get; set; }
        public int Volume { get; set; }
        public decimal Value { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }

    }

    public class UpdateOrder
    {
        public int id { get; set; }
        public string companyName { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
    }

    public class Term
    {
        public UpdateOrder[] Terminal { get; set; }
    }
}