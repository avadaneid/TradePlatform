using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Account
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public long CNP { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public long PhoneNumber { get; set; }
        public string Address { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is Required.")]
        public string Password { get; set; }
        public DateTime CreatedOn { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Confirmation Password is Required")]
        //[Compare("Password", ErrorMessage = "Password Must Match")]
        public string ConfirmPassword { get; set; } 

        [Required]
        public AccountType AccountType { get; set; }

        public long CUI { get; set; }

        public virtual Individual Individual { get; set; }
      
        public virtual Company Company { get; set; }

    }

    public enum AccountType{
        SelectValue,
        Individual,
        Company
    }



}