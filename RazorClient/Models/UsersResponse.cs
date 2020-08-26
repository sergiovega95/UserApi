using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorClient.Models
{
    public class UsersResponse:BaseResponse
    {
        public List<User> Usuarios { get; set; } = new List<User>();
    }
}
