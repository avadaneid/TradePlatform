using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class LogInModel
    {
        [Required(ErrorMessage = "UserName is Required !")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is Required !")]
        public string Password { get; set; }
    }
}