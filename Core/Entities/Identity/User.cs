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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUser { get; set; }

        [Required(ErrorMessage = "Ingrese el nombre")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Ingrese el apellido")]
        [MaxLength(200)]
        public string LastName { get; set; }

        [Required]
        public DocumentType DocumentType { get; set; }

        [Required(ErrorMessage ="Ingrese el número del documento de identidad")]
        [MaxLength(100)]
        public string Document { get; set; }

       
    }
}
