using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using APIGateway.Models;
using IdentityModel;
using IdentityModel.Client;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("auth/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        
        public TokenController(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        [HttpPost("")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetToken(RequestTokenModel model)
        {
            var client = _clientFactory.CreateClient();
            var authProviderConfig = _configuration.GetSection("AuthenticationProvider");
            
            var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = $"{authProviderConfig["Url"]}/connect/token",
                GrantType = GrantType.ResourceOwnerPassword,

                ClientId = authProviderConfig["ClientId"],
                ClientSecret = authProviderConfig["ClientSecret"],
                
                UserName = model.UserName,
                Password = model.Password
            });

            return CreateContentResult(response);
        }
        
        [HttpPost("invalidate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InvalidateToken(InvalidateTokenModel model)
        {
            var client = _clientFactory.CreateClient();
            var authProviderConfig = _configuration.GetSection("AuthenticationProvider");
            
            var response = await client.RevokeTokenAsync(new TokenRevocationRequest()
            {
                Address = $"{authProviderConfig["Url"]}/connect/revocation",
                
                ClientId = authProviderConfig["ClientId"],
                ClientSecret = authProviderConfig["ClientSecret"],
                
                Token = model.AccessToken,
                TokenTypeHint = OidcConstants.TokenTypes.AccessToken
            });

            return CreateContentResult(response);
        }

        private IActionResult CreateContentResult(ProtocolResponse response)
        {
            if (!response.IsError)
                return new ContentResult
                {
                    Content = response.Raw, 
                    ContentType = MediaTypeNames.Application.Json,
                    StatusCode = StatusCodes.Status200OK
                };
            return BadRequest(response.Error);
        }
    }
}