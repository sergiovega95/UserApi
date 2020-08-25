using Core.Entities.Shared;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities.Identity
{
    public class User: IdentityUser<Guid>
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string LastName { get; set; }

        [Required]
        public DocumentType DocumentType { get; set; }

        [Required]
        [MaxLength(100)]
        public string Document { get; set; }

       
    }
}
