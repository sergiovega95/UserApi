using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Shared
{
    public class BaseResponse
    {
        public DateTime DateTimeRequest { get; set; } = DateTime.Now;

        public int StatusCode { get; set; } = 200;

        public List<string> Errors { get; set; } = new List<string>();

        public bool IsSucessfull { get; set; } = true;       

        public string ErrorMessage { get; set; }
    }
}
