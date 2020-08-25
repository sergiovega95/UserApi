using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RazorClient.Models;

namespace RazorClient.Pages
{
    public class UserModel : PageModel
    {
        private readonly ILogger<UserModel> _logger;

        public UserModel(ILogger<UserModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
        
        public IActionResult OnGetGetUsers(DataSourceLoadOptionsBase loadOptionsBase)
        {
            List<User> usuarios = new List<User>();

            try
            {
                return new JsonResult(DataSourceLoader.Load(usuarios, loadOptionsBase));

            }
            catch (Exception e)
            {
                return BadRequest("Ocurrió un error inesperado al listar los usuarios");
            }          

        }
                
        public IActionResult OnPostInsertUser(string values)
        {
            try
            {   
                NewUser newuser = new NewUser();
                JsonConvert.PopulateObject(values, newuser);
                return new JsonResult(HttpStatusCode.OK);

            }
            catch (Exception e)
            {
                return BadRequest("Ocurrió un error inesperado al agregar el usuario");
            }            
        }
                
        public IActionResult OnPostDeleteUser(int key)
        {
            try
            {
                return new JsonResult(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return BadRequest("Ocurrió un error inesperado al eliminar el usuario");
            }            
        }
        
        public IActionResult OnPutUpdateUser(int key, string values)
        {
            try
            {
                return new JsonResult(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return BadRequest("Ocurrió un error inesperado al actualizar los datos del usuario");
            }

         
        }
    }
}
