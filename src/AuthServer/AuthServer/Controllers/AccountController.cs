using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using AuthServer.BL.Exceptions;
using AuthServer.BL.Interfaces;
using AuthServer.Extensions;
using AuthServer.Models;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AuthServer.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;

        public AccountController(IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto model)
        {
            if (model == null) return BadRequest();
            try
            {
                var user = await _accountService.CreateUserAsync(model.MapToUserCreateModel());
                if (user != null)
                    return Created($"{Request.Path}/{user.Id}", user);
                return BadRequest();
            }
            catch (UserCreateException ex)
            {
                return BadRequest(ex.Errors);
            }
        }
        
        [Authorize]
        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Get(long id)
        {
            var userId = User.GetSubjectId();
            if (!userId.Equals(id.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                return Forbid();
            }

            var user = await _accountService.GetUserAsync(userId);
            if (user != null)
                return Ok(user.MapToUserDto());
            return NotFound();
        }
        
        [Authorize]
        [HttpPut("{id:long}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateUserDto model)
        {
            if (!User.GetSubjectId().Equals(id.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                return Forbid();
            }
            
            if (model == null) return BadRequest();
            try
            {
                var result = await _accountService.UpdateUserAsync(User, model.MapToUserUpdateModel());
                if (result.Succeeded)
                    return NoContent();
                return BadRequest(result.Errors);
            }
            catch (UserNotFoundException)
            {
                return Forbid();
            }
        }
        
        [Authorize]
        [HttpDelete]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete()
        {
            try
            {
                var result = await _accountService.DeleteUserAsync(User);
                if (result.Succeeded)
                    return NoContent();
                return BadRequest(result.Errors.FirstOrDefault());
            }
            catch (UserNotFoundException)
            {
                return Forbid();
            }
        }
    }
}