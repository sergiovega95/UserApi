﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Core.Entities.Shared;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Models;
using WebApi.Models.Request;
using WebApi.Models.Response;
using WebApi.Models.Shared;
using WebApi.Shared;


namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        
        private readonly UserManager<User> _userManager;    
        private readonly ILogger<IdentityController> _logger;
        public readonly IConfiguration _configuration;
        private readonly IUsers _users;

        public IdentityController(UserManager<User> userManager, ILogger<IdentityController> logger, IConfiguration configuration, IUsers users)
        {
            _userManager = userManager;           
            _logger = logger;
            _configuration = configuration;          
            _users=users;
        }


        /// <summary>
        /// Obtiene todos los usuarios registrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("Users")]
        [Produces("application/json")]
        [SwaggerResponse(200, "Operacion Exitosa", typeof(ResponseUsers))]
        [SwaggerResponse(500, "Error interno del sistema", typeof(ResponseUsers))]
        [AllowAnonymous]
        public IActionResult GetAllUsers()
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;
            ResponseUsers response = new ResponseUsers();

            try
            {
                response.Usuarios = _users.GetAllUsers();
            }
            catch (DatabaseException e)
            {
                statusCode = HttpStatusCode.InternalServerError;
                response.StatusCode = (int)statusCode;
                response.IsSucessfull = false;
                response.ErrorMessage = e.Message;
                _logger.LogError(e, $"Failed to get list user");
            }
            catch (Exception e)
            {
                statusCode = HttpStatusCode.InternalServerError;
                response.StatusCode = (int)statusCode;
                response.IsSucessfull = false;
                response.ErrorMessage = e.Message;
                _logger.LogError(e, $"Failed to get list user");
            }

            return StatusCode((int)statusCode, response);
        }

        /// <summary>
        /// Obtiene un usuario particular mediante su Id unico
        /// </summary>
        /// <returns></returns>
        [HttpGet("Users/{id}")]
        [SwaggerResponse(200, "Operación exitosa", typeof(BaseResponse))]
        [SwaggerResponse(400, "Datos invalidos", typeof(BaseResponse))]
        [SwaggerResponse(404, "Recurso no encontrado", typeof(BaseResponse))]
        [SwaggerResponse(500, "Error interno del sistema", typeof(BaseResponse))]
        [Produces("application/json")]
        [AllowAnonymous]
        public IActionResult GetUserById(int id)
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;
            ResponseUsers response = new ResponseUsers();

            try
            {               
                response.Usuarios.Add(_users.GetUserById(id));
            }
            catch (UserException e)
            {
                statusCode = HttpStatusCode.NotFound;
                response.StatusCode = (int)statusCode;
                response.IsSucessfull = false;
                response.ErrorMessage = e.Message;
                _logger.LogInformation(e, $"User with id {id.ToString()} not found");
            }
            catch (Exception e)
            {
                statusCode = HttpStatusCode.InternalServerError;
                response.StatusCode = (int)statusCode;
                response.IsSucessfull = false;
                response.ErrorMessage = e.Message;
                _logger.LogError(e, $"Failed to get user with id {id.ToString()}");
            }

            return StatusCode((int)statusCode, response);
        }

        /// <summary>
        /// Obtiene un usuario particular mediante su numero de identificación
        /// </summary>
        /// <returns></returns>
        [HttpGet("Users/identification/{identification}")]
        [Produces("application/json")]
        [SwaggerResponse(200, "Operación exitosa", typeof(BaseResponse))]
        [SwaggerResponse(400, "Datos invalidos", typeof(BaseResponse))]
        [SwaggerResponse(404, "Recurso no encontrado", typeof(BaseResponse))]
        [SwaggerResponse(500, "Error interno del sistema", typeof(BaseResponse))]
        [AllowAnonymous]
        public IActionResult GetUserByidentification(string identification)
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;
            ResponseUsers response = new ResponseUsers();

            try
            {
                if (!string.IsNullOrEmpty(identification))
                {
                    response.Usuarios.Add(_users.GetUserByIdentification(identification));
                }
                else
                {
                    statusCode = HttpStatusCode.BadRequest;
                    response.StatusCode = (int)statusCode;
                    response.IsSucessfull = false;
                    response.ErrorMessage = "Datos invalidos";
                }
            }
            catch (UserException e)
            {
                statusCode = HttpStatusCode.NotFound;
                response.StatusCode = (int)statusCode;
                response.IsSucessfull = false;
                response.ErrorMessage = e.Message;
                _logger.LogInformation(e, $"User with identification {identification} not found");
            }
            catch (Exception e)
            {
                statusCode = HttpStatusCode.InternalServerError;
                response.StatusCode = (int)statusCode;
                response.IsSucessfull = false;
                response.ErrorMessage = e.Message;
                _logger.LogError(e, $"Failed to get user with identification {identification}");
            }

            return StatusCode((int)statusCode, response);
        }


        /// <summary>
        /// Registro de un nuevo usuario
        /// </summary>
        /// <param name="newUser"></param>
        /// <response code="200">Operación exitosa</response>
        /// <response code="400">Bad Request, los datos enviados presentan inconsistencias </response>
        /// <response code="500">Error interno del servidor</response>
        /// <returns></returns>
        [AllowAnonymous]
        [SwaggerResponse(200, "El usuario fúe creado", typeof(ResponseRegister))]
        [SwaggerResponse(400, "Datos invalidos", typeof(ResponseRegister))]
        [SwaggerResponse(500, "Error interno del sistema", typeof(ResponseRegister))]
        [HttpPost("RegisterUser")]
        [Produces("application/json")]
        public async Task<ObjectResult> RegisterUser (RequestIdentityRegister newUser)            
        {           
            HttpStatusCode statusCode = HttpStatusCode.OK;         
            ResponseRegister response = new ResponseRegister();

            try
            {
                var validationResultList = new List<ValidationResult>();

                if (Validator.TryValidateObject(newUser, new ValidationContext(newUser), validationResultList))
                {
                    //DocumentType documentType = _users.GetDocumentTypeByEnum(newUser.DocumentType.ToString());
                    var user = new User { UserName = newUser.Document, Email = newUser.Email , Document= newUser.Document , Name=newUser.Name, LastName=newUser.LastName , DocumentType= newUser.DocumentType };
                    var result = await _users.AddUserAsync(user, newUser.Password);

                    if (!result.Succeeded)
                    {
                        statusCode = HttpStatusCode.BadRequest;
                        response.StatusCode = (int)statusCode;
                        response.IsSucessfull = false;                     
                      
                        foreach (IdentityError error in result.Errors)
                        {
                            response.Errors.Add(error.Description);
                        }

                    }
                    else
                    {
                        var userRegistered = await _userManager.FindByEmailAsync(newUser.Email);
                        response.IdUser = userRegistered.IdUser.ToString();
                    }

                }
                else
                {
                    foreach (ValidationResult error in validationResultList)
                    {
                        response.Errors.Add(error.ErrorMessage);
                    }
                }                              
                
            }

            catch (UserException e)
            {
                statusCode = HttpStatusCode.BadRequest;
                response.StatusCode = (int)statusCode;
                response.IsSucessfull = false;
                response.ErrorMessage = e.Message;
                response.Errors.Add(e.Message);
                _logger.LogError(e, $"Failed to register user with email {newUser.Email} and identification {newUser.Document}");
            }
            catch (DatabaseException e )
            {
                statusCode = HttpStatusCode.InternalServerError;
                response.StatusCode = (int)statusCode;
                response.IsSucessfull = false;
                response.ErrorMessage = e.Message;
                _logger.LogError(e, $"Failed to register user with email {newUser.Email} and identification {newUser.Document}");
            }            
            catch (Exception e)
            {               
                statusCode = HttpStatusCode.InternalServerError;
                response.StatusCode = (int)statusCode;
                response.IsSucessfull = false;
                response.ErrorMessage = "Internal server error";
                _logger.LogError(e, $"Failed to register user with email {newUser.Email} and identification {newUser.Document}");
                
            }

            return StatusCode((int)statusCode, response);
        }


        /// <summary>
        /// Inicio de sesión y Validar las credenciales del usuario
        /// </summary>
        /// <param name="User"></param>
        /// <response code="200">Operación exitosa</response>
        /// <response code="400">Bad Request, los datos enviados presentan inconsistencias </response>
        /// <response code="400">Not found, recurso no encontrado </response>
        /// <response code="500">Error interno del servidor</response>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("SignInUser")]
        public async Task<IActionResult> SignInAsync(string document , string password)
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;
            ResponseSignInUser response = new ResponseSignInUser();          

            try
            {
                var result = await _users.LoginAsync(document, password);

                if (result.Succeeded)
                {
                    //Get JWT TOKEN
                    GenerateJWT token = new GenerateJWT(_configuration);
                    User usuario = _users.GetUserByIdentification(document);
                    var userRegistered = await _userManager.FindByEmailAsync(usuario.Email);
                    response.IdUser = userRegistered.Id.ToString();
                    response.JWT = token.GenerateTokenUser(userRegistered);
                    return StatusCode((int)statusCode, response);
                }                
                else if (result.IsNotAllowed)
                {                
                    response.ErrorMessage = $"Failed to signIn user with identificacion: {document}, signIn is not allowed";
                    response.Errors.Add(response.ErrorMessage);
                }
                else
                {
                    response.ErrorMessage = "Credenciales inválidas";
                    response.Errors.Add(response.ErrorMessage);
                }

                statusCode = HttpStatusCode.BadRequest;
                response.StatusCode = (int)statusCode;
                response.IsSucessfull = false;                             

            }

            catch (DatabaseException e)
            {
                statusCode = HttpStatusCode.InternalServerError;
                response.StatusCode = (int)statusCode;
                response.IsSucessfull = false;
                response.ErrorMessage = e.Message;
                _logger.LogError(e, $"Failed to signIn user with identification: {document}");
            }

            catch (Exception e)
            {
                statusCode = HttpStatusCode.InternalServerError;
                response.StatusCode = (int)statusCode;
                response.IsSucessfull = false;
                response.ErrorMessage = "Internal server error";
                _logger.LogError(e, $"Failed to signIn user with identification: {document}");
            }

            return StatusCode((int)statusCode, response);

        }
       

        /// <summary>
        /// Actualizar los datos de un usuario
        /// </summary>
        /// <param name="IdUser"></param>
        /// <returns></returns>
        [SwaggerResponse(200, "El usuario fúe actualizado", typeof(BaseResponse))]
        [SwaggerResponse(400, "Datos invalidos", typeof(BaseResponse))]
        [SwaggerResponse(404, "Recurso no encontrado", typeof(BaseResponse))]
        [SwaggerResponse(500, "Error interno del sistema", typeof(BaseResponse))]
        [HttpPut("UpdateUser")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateUser(UserUpdate update)
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;
            BaseResponse response = new BaseResponse();

            try
            {
                string json = JsonConvert.SerializeObject(update);
                _users.UpdateUser(json);
            }
            catch (UserException e)
            {
                statusCode = HttpStatusCode.NotFound;
                response.StatusCode = (int)statusCode;
                response.IsSucessfull = false;
                response.ErrorMessage = e.Message;
                response.Errors.Add(e.Message);
                _logger.LogError(e, $"Failed to update user with id: {update.IdUser}, user not found");
            }
            catch (Exception e)
            {
                statusCode = HttpStatusCode.InternalServerError;
                response.StatusCode = (int)statusCode;
                response.IsSucessfull = false;
                response.ErrorMessage = "Internal server error";
                _logger.LogError(e, $"Failed to delete user with id: {update.IdUser}");
            }

            return StatusCode((int)statusCode, response);
        }

        /// <summary>
        /// Borrar un usuario 
        /// </summary>
        /// <param name="IdUser"></param>        
        /// <returns></returns>
        [SwaggerResponse(200, "El usuario fúe eliminado", typeof(BaseResponse))]
        [SwaggerResponse(400, "Datos invalidos", typeof(BaseResponse))]
        [SwaggerResponse(404, "Recurso no encontrado", typeof(BaseResponse))]
        [SwaggerResponse(500, "Error interno del sistema", typeof(BaseResponse))]
        [HttpDelete("DeleteUser")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteUser(int IdUser)
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;
            BaseResponse response = new BaseResponse();

            try
            {                
                _users.DeleteUser(IdUser);            
               
            }
            catch (UserException e)
            {
                statusCode = HttpStatusCode.NotFound;
                response.StatusCode = (int)statusCode;
                response.IsSucessfull = false;
                response.ErrorMessage = e.Message;
                response.Errors.Add(e.Message);
                _logger.LogError(e, $"Failed to delete user with id: {IdUser}, user not found");
            }
            catch (DatabaseException e)
            {
                statusCode = HttpStatusCode.InternalServerError;
                response.StatusCode = (int)statusCode;
                response.IsSucessfull = false;
                response.ErrorMessage = e.Message;
                _logger.LogError(e, $"Failed to delete user with id: {IdUser}");
            }

            catch (Exception e)
            {
                statusCode = HttpStatusCode.InternalServerError;
                response.StatusCode = (int)statusCode;
                response.IsSucessfull = false;
                response.ErrorMessage = "Internal server error";
                _logger.LogError(e, $"Failed to delete user with identification: {IdUser}");
            }

            return StatusCode((int)statusCode, response);
        }

        /// <summary>
        /// Obtiene los tipos de documentos almacenados en la base de datos (CC ,TI)
        /// </summary>
        /// <returns></returns>
        [HttpGet("DocumentsType")]
        [Produces("application/json")]
        [AllowAnonymous]
        [SwaggerResponse(200, "Exitoso", typeof(List<DocumentType>))]       
        [SwaggerResponse(500, "Error interno del sistema", typeof(BaseResponse))]
        public IActionResult GetDocumentsType()
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;
            object data = new object();

            try
            {
               data = _users.GetDocumentTypes();

            }           
            catch (DatabaseException e)
            {
                statusCode = HttpStatusCode.InternalServerError;                
                _logger.LogError(e, $"Failed to get documents type");
            }
            return StatusCode((int)statusCode, data);
        }
    }
}