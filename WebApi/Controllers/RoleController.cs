using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Models.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {     
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<RoleController> _logger;
        
        public RoleController(RoleManager<Role> roleManager, ILogger<RoleController> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
        }

        // GET: api/<RoleController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<RoleController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RoleController>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] JsonElement body)
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;
            BaseResponse response = new BaseResponse();
            string roleName = string.Empty;

            try
            {

                roleName = System.Text.Json.JsonSerializer.Serialize(body);
                var register = await _roleManager.CreateAsync(new Role(){ Name=roleName});

                if (!register.Succeeded)
                {
                    statusCode = HttpStatusCode.BadRequest;
                    response.StatusCode = (int)statusCode;
                    response.IsSucessfull = false;
                    response.ErrorMessage = $"Failed to register Rol : {roleName}";

                    foreach (IdentityError error in register.Errors)
                    {
                        response.Errors.Add(error.Description);
                    }
                }

            }
            catch (Exception e)
            {
                statusCode = HttpStatusCode.InternalServerError;
                response.StatusCode = (int)statusCode;
                response.IsSucessfull = false;
                response.ErrorMessage = "Internal server error";
                _logger.LogError(e, $"Failed to register Rol : {roleName}");
            }

            return StatusCode((int)statusCode, response);

        }

        // PUT api/<RoleController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RoleController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
