using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RazorClient.Models;
using RestSharp;
using Services.Interface;

namespace RazorClient.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUserServices _user;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger , IUserServices user)
        {
            _logger = logger;
            _user = user;
        }

        public void OnGet()
        {

        }      

        public JsonResult OnPostIniciarSesion(InicioSesion form)
        {
            try
            {                
                IRestResponse response =_user.SignInUser(form.Identification,form.Password);

                if (response.StatusCode!=System.Net.HttpStatusCode.OK)
                {
                    var respuesta = JsonConvert.DeserializeObject<BaseResponse>(response.Content);
                    return new JsonResult(respuesta);
                }
                else
                {
                    return new JsonResult(null);
                }
                             
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to login user with identification {form.Identification}");
                return new JsonResult(false,null);
            }            
        }      
    }
}
