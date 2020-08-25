using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Entities.Shared
{
    public class DocumentType : BaseIdentity
    {
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(4)]
        public int Enum  { get; set; }
    }
}
