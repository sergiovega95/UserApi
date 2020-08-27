using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models
{
    public class UserUpdate
    {
        public int IdUser { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
    }
}
