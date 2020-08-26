using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorClient.Models
{
    public class BaseResponse
    {
        public DateTime DateTimeRequest { get; set; } = DateTime.Now;
        public int StatusCode { get; set; } = 200;
        public List<string> Errors { get; set; } = new List<string>();
        public bool IsSucessfull { get; set; } 
        public string ErrorMessage { get; set; }
        public string JWT { get; set; }
        public string IdUser { get; set; }
    }
}
