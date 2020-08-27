using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Core.Entities.Shared;
using Core.Models;
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
        
        /// <summary>
        /// Get all users
        /// </summary>
        /// <param name="loadOptionsBase"></param>
        /// <returns></returns>
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
                _logger.LogError(e, $"Failed to get user");
                return BadRequest("Ocurrió un error inesperado al listar los usuarios");
            }          

        }
          
        /// <summary>
        /// Insert New Users
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
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
                _logger.LogError(e, $"Failed to add new user");
                return BadRequest("Ocurrió un error inesperado al agregar el usuario");
            }            
        }
        
        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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
                _logger.LogError(e, $"Failed to delete user with id {key}");
                return BadRequest("Ocurrió un error inesperado al eliminar el usuario");
            }            
        }
        
        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public IActionResult OnPutUpdateUser(int key, string values)
        {
            try
            {
                UserUpdate user = new UserUpdate();
                JsonConvert.PopulateObject(values, user);
                user.IdUser = key;
                IRestResponse response =  _user.UpdateUser(user);

                if (response.StatusCode==HttpStatusCode.OK)
                {
                    return new JsonResult(HttpStatusCode.OK);
                }
                else
                {
                    var res = JsonConvert.DeserializeObject<BaseResponse>(response.Content)
                    return BadRequest(res.ErrorMessage);
                }
               
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to update user with id {key}");
                return BadRequest("Ocurrió un error inesperado al actualizar los datos del usuario");
            }

         
        }

        /// <summary>
        /// Get Document types
        /// </summary>
        /// <param name="loadOptionsBase"></param>
        /// <returns></returns>
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
                _logger.LogError(e, $"Failed to get document types");
                return BadRequest("Ocurrió un error inesperado al listar los usuarios");
            }

        }
    }
}
