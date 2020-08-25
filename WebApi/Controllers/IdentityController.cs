using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Core.Exceptions;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApi.Models;
using WebApi.Models.Request;
using WebApi.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<IdentityController> _logger;
        public readonly IConfiguration _configuration;
        private readonly IUsers _users;

        public IdentityController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<IdentityController> logger, IConfiguration configuration,RoleManager<Role> roleManager, IUsers users)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
            _roleManager = roleManager;
             _users=users;
        }


        /// <summary>
        /// Permite el registro de un nuevo usuario
        /// </summary>
        /// <param name="newUser"></param>
        /// <response code="200">Operación exitosa</response>
        /// <response code="400">Bad Request, los datos enviados presentan inconsistencias </response>
        /// <response code="500">Error interno del servidor</response>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        public async Task<ObjectResult> RegisterUser (RequestIdentityRegister newUser) 
           
        {
           

            HttpStatusCode statusCode = HttpStatusCode.OK;         
            ResponseRegister response = new ResponseRegister();

            try
            {
                var validationResultList = new List<ValidationResult>();

                if (Validator.TryValidateObject(newUser, new ValidationContext(newUser), validationResultList))
                {
                    var user = new User { UserName = newUser.Email, Email = newUser.Email };
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
                        response.IdUser = userRegistered.Id.ToString();
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
        /// Permite realizar el proceso de iniciar sesicón y Validar las credenciales del usuario
        /// </summary>
        /// <param name="User"></param>
        /// <response code="200">Operación exitosa</response>
        /// <response code="400">Bad Request, los datos enviados presentan inconsistencias </response>
        /// <response code="400">Not found, recurso no encontrado </response>
        /// <response code="500">Error interno del servidor</response>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("SignInUser")]
        public async Task<IActionResult> SignInAsync(RequestIdentityRegister User)
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;
            ResponseSignInUser response = new ResponseSignInUser();

            try
            {              
                                 
                var result = await _signInManager.PasswordSignInAsync(User.Email, User.Password,true, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    //Get JWT TOKEN
                    GenerateJWT token = new GenerateJWT(_configuration);
                    var userRegistered = await _userManager.FindByEmailAsync(User.Email);
                    response.IdUser = userRegistered.Id.ToString();
                    response.JWT = token.GenerateTokenUser(userRegistered);
                    return StatusCode((int)statusCode, response);
                }
                else if(result.IsLockedOut)
                {                       
                    response.ErrorMessage = $"Account with {User.Email}";
                }
                else if (result.IsNotAllowed)
                {                
                    response.ErrorMessage = $"Failed to signIn user with identificacion: {User.Document}, signIn is not allowed";
                }
                else
                {
                    response.ErrorMessage = $"Failed to signIn user with identification: {User.Document}, invalid credentials";
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
                _logger.LogError(e, $"Failed to signIn user with identification: {User.Document}");
            }

            catch (Exception e)
            {
                statusCode = HttpStatusCode.InternalServerError;
                response.StatusCode = (int)statusCode;
                response.IsSucessfull = false;
                response.ErrorMessage = "Internal server error";
                _logger.LogError(e, $"Failed to signIn user with identification: {User.Document}");
            }

            return StatusCode((int)statusCode, response);

        }

        /// <summary>
        /// Borrar un usuario de la base de datos
        /// </summary>
        /// <param name="IdUser"></param>
        /// <returns></returns>
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser (string IdUser)
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;
            ResponseSignInUser response = new ResponseSignInUser();
            return StatusCode((int)statusCode, response);
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(string IdUser)
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;
            ResponseSignInUser response = new ResponseSignInUser();
            return StatusCode((int)statusCode, response);
        }
    }
}