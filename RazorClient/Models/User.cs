using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RazorClient.Models
{
    public class User: Core.Entities.Identity.User
    {
        [NotMapped]
        public string Password { get; set; }       
    }
}
