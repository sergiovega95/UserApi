using Core.Entities.Shared;
using Core.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApi.Models.Request
{
    public class RequestIdentityRegister
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DocumentType DocumentType { get; set; }

        [Required]
        public string Document { get; set; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",ErrorMessage ="Invalid email format")]       
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
   
    }
}
