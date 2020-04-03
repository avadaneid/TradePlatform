using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Models
{
    public class BID
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public int Quantity { get; set; }
    }

    public class Transaction
    {
        public ASK ASK { get; set; }
        public BID BID { get; set; }
        public long CUI { get; set; }
        public long CNP { get; set; }

    }

}