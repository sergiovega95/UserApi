using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Core.Entities.Shared;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
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
                    usuarios = JsonConvert.DeserializeObject<UsersResponse>(response.Content).Usuarios;
                    return new JsonResult(DataSourceLoader.Load(usuarios, loadOptionsBase));
                }
                else
                {                   
                    return BadRequest(JsonConvert.DeserializeObject<BaseResponse>(response.Content).ErrorMessage);
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
                IRestResponse response=  _user.AddUser(newuser);

                if (response.StatusCode!=HttpStatusCode.OK)
                {
                    var respuesta = JsonConvert.DeserializeObject<BaseResponse>(response.Content);
                    return BadRequest(respuesta.ErrorMessage);
                }
               
                return new JsonResult(HttpStatusCode.OK);
                
            }
            catch (Exception e)
            {
                return BadRequest("Ocurrió un error inesperado al agregar el usuario");
            }            
        }
                
        public IActionResult OnDeleteDeleteUser(int key)
        {
            try
            {
                IRestResponse response = _user.DeleteUser(key);

                if (response.StatusCode!=HttpStatusCode.OK)
                {
                    var respuesta = JsonConvert.DeserializeObject<BaseResponse>(response.Content);
                    return BadRequest(respuesta.ErrorMessage);
                }
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

        public IActionResult OnGetTiposDocumentos(DataSourceLoadOptions loadOptionsBase)
        {
            List<DocumentType> documentos = new List<DocumentType>();

            try
            {
                var response = _user.GetDocumentTypes();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (loadOptionsBase.Filter!=null)
                    {
                        var filtro = loadOptionsBase.Filter[1];
                        documentos = JsonConvert.DeserializeObject<List<DocumentType>>(response.Content).Where(s=>s.Id== Convert.ToInt32(filtro)).ToList();
                    }

                    documentos = JsonConvert.DeserializeObject <List<DocumentType>>(response.Content);
                    
                }

                return new JsonResult(DataSourceLoader.Load(documentos, loadOptionsBase));
            }
            catch (Exception e)
            {
                return BadRequest("Ocurrió un error inesperado al listar los usuarios");
            }

        }
    }
}
