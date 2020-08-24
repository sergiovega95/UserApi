using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Core.Entities.Identity;
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

        public IdentityController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<IdentityController> logger, IConfiguration configuration,RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        public async Task<ObjectResult> RegisterUser (RequestIdentityRegister newUser)
        {
           

            HttpStatusCode statusCode = HttpStatusCode.OK;         
            ResponseRegister response = new ResponseRegister();

            try
            {
                //Using email as username
                var user = new User { UserName = newUser .Email, Email = newUser.Email };
                var result = await _userManager.CreateAsync(user, newUser.Password);

                if (!result.Succeeded)
                {
                    statusCode = HttpStatusCode.BadRequest;
                    response.StatusCode = (int)statusCode;
                    response.IsSucessfull = false; 
                    response.ErrorMessage= $"Failed to register user with email {newUser.Email}";

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
            catch (Exception e)
            {               
                statusCode = HttpStatusCode.InternalServerError;
                response.StatusCode = (int)statusCode;
                response.IsSucessfull = false;
                response.ErrorMessage = "Internal server error";
                _logger.LogError(e, $"Failed to register user with email {newUser.Email}");
                
            }

            return StatusCode((int)statusCode, response);
        }


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
                    response.ErrorMessage = $"LockedOut account with email {User.Email}";
                }
                else if (result.IsNotAllowed)
                {                
                    response.ErrorMessage = $"Failed to signIn user with email {User.Email}, signIn is not allowed";
                }
                else
                {
                    response.ErrorMessage = $"Failed to signIn user with email {User.Email}, invalid credentials";
                }

                statusCode = HttpStatusCode.BadRequest;
                response.StatusCode = (int)statusCode;
                response.IsSucessfull = false;                             

            }
            catch (Exception e)
            {
                statusCode = HttpStatusCode.InternalServerError;
                response.StatusCode = (int)statusCode;
                response.IsSucessfull = false;
                response.ErrorMessage = "Internal server error";
                _logger.LogError(e, $"Failed to signIn user with email {User.Email}");
            }

            return StatusCode((int)statusCode, response);

        }
    }
}