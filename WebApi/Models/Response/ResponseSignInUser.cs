using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Shared;

namespace WebApi.Models
{
    public class ResponseSignInUser: BaseResponse
    {    

        public string JWT { get; set; }
        public string IdUser { get; set; } 
      
    }
}
