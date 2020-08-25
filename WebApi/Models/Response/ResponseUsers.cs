using Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Shared;

namespace WebApi.Models.Response
{
    public class ResponseUsers : BaseResponse
    {
        public List<User> Usuarios { get; set; } = new List<User>();
    }
}
