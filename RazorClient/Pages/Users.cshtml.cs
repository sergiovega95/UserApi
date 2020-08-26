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
using RestSharp;
using Services.Interface;

namespace RazorClient.Pages
{
    public class UserModel : PageModel
    {
        private readonly ILogger<UserModel> _logger;
        private readonly IUserServices _user;
        public UserModel(ILogger<UserModel> logger , IUserServices user)
        {
            _logger = logger;
            _user = user;
        }

        public void OnGet()
        {

        }
        
        public IActionResult OnGetGetUsers(DataSourceLoadOptionsBase loadOptionsBase)
        {
            List<User> usuarios = new List<User>();

            try
            {
                var response = _user.GetUsers();

                if (response.StatusCode==HttpStatusCode.OK)
                {
                    usuarios = JsonConvert.DeserializeObject<User>(response.Content);
                    return new JsonResult(DataSourceLoader.Load(usuarios, loadOptionsBase));
                }
                else
                {
                    usuarios = JsonConvert.DeserializeObject<User>(response.Content);
                    return BadRequest(response);
                }                

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
                newuser.DocumentType = Core.Enums.EnumDocumentType.CC;
                IRestResponse response=  _user.AddUser(newuser);

                if (response.StatusCode!=HttpStatusCode.OK)
                {
                    var respuesta = JsonConvert.DeserializeObject<LoginResponse>(response.Content);
                    return BadRequest(respuesta.ErrorMessage);
                }
               
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
                _user.DeleteUser(key);
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
                _user.UpdateUser(key);
                return new JsonResult(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return BadRequest("Ocurrió un error inesperado al actualizar los datos del usuario");
            }

         
        }
    }
}
