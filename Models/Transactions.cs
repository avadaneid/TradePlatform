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
}