using ReadingIsGood.Business.Base.Concrete;
using ReadingIsGood.Business.Services.Abstract;
using ReadingIsGood.Business.Services.Interface;
using ReadingIsGood.Core.DTOs;

namespace ReadingIsGood.Business.Services.Concrete
{
    public class AuthenticationService : BusinessService, IAuthenticationService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public AuthenticationService(IServiceProvider sp, ITokenService tokenService, IUserService userService) : base(sp)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        public async Task<TokenDTO> CreateTokenAsync(LoginDTO loginDto)
        {
            if (loginDto == null)
                throw new ArgumentNullException(nameof(loginDto));

            if (String.IsNullOrEmpty(loginDto.UserName) || String.IsNullOrEmpty(loginDto.Password))
                throw new InvalidDataException(nameof(loginDto));

            var user = await _userService.GetUserByUsernamePassword(loginDto.UserName, loginDto.Password);

            if (user == null)
                return null;

            return await _tokenService.CreateToken(Mapper.Map<UserDTO>(user));
        }

        public async Task<TokenDTO> RefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new ArgumentNullException(nameof(refreshToken));

            return await _tokenService.RefreshToken(refreshToken);
        }
    }
}
