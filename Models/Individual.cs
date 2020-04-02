using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Models;

namespace Models
{
    public class Individual
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long CNP { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public long PhoneNumber { get; set; }
        public string Address { get; set; }
        public decimal Debit { get; set; }
        public string UserName { get; set; }
        public virtual Account Account { get; set; }
    }
}