using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadingIsGood.Business.Services.Abstract;
using ReadingIsGood.Core.DTOs;

namespace ReadingIsGood.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(TokenDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            var tokenDto = await _authenticationService.CreateTokenAsync(loginDto);
            return tokenDto != null ? Ok(tokenDto) : BadRequest("Kullanıcı adı veya şifre hatalı!");
        }

        [HttpPost("RefreshToken")]
        public async Task<TokenDTO> RefreshToken(string refreshToken)
        {
            return await _authenticationService.RefreshTokenAsync(refreshToken);
        }
    }

}
