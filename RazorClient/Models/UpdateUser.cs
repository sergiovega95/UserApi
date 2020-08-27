using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RazorClient.Models
{
    public class UpdateUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }

        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
    }
}
