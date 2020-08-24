using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Request
{
    public class RequestIdentityRegister
    {
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",ErrorMessage ="Invalid email format")]       
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
