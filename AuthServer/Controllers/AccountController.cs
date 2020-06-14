using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using AuthServer.Extensions;
using AuthServer.Mappers;
using AuthServer.Models;
using AuthServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthServer.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AccountController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IProfileService profileService, ILogger<AccountController> logger)
        {
            _profileService = profileService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody]RegisterUserDto model)
        {
            if (model == null) return BadRequest();
            
            var result = await _profileService.AddUserAsync(ApplicationUserMapper.MapFrom(model), model.Password);
            if (result.Succeeded)
            {
                return Created("/connect/userinfo", null);
            }
            return BadRequest(result.Errors.FirstOrDefault());
        }
        
        [Authorize]
        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody]UpdateUserDto model)
        {
            if (model == null) return BadRequest();

            var user = await _profileService.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var result = await _profileService.UpdateUserAsync(user.UpdateFrom(model));
            if (result.Succeeded)
                return NoContent();
            return BadRequest(result.Errors.FirstOrDefault());
        }
        
        [Authorize]
        [HttpDelete]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete()
        {
            var user = await _profileService.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var result = await _profileService.DeleteUserAsync(user);
            if (result.Succeeded)
                return NoContent();
            return BadRequest(result.Errors.FirstOrDefault());
        }
    }
}