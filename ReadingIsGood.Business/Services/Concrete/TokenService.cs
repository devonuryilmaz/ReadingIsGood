using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ReadingIsGood.Business.Base.Concrete;
using ReadingIsGood.Business.Configurations;
using ReadingIsGood.Business.Services.Abstract;
using ReadingIsGood.Business.Services.Interface;
using ReadingIsGood.Core.DTOs;
using ReadingIsGood.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ReadingIsGood.Business.Services.Concrete
{
    public class TokenService : BusinessService, ITokenService
    {
        private readonly IUserService _userService;
        private readonly CustomTokenOptions _customTokenOptions;

        public TokenService(IServiceProvider sp, IUserService userService, IOptions<CustomTokenOptions> customTokenOptions) : base(sp)
        {
            _customTokenOptions = customTokenOptions.Value;
            _userService = userService;
        }

        private string CreateRefreshToken()
        {
            var numberByte = new Byte[32];

            using var random = RandomNumberGenerator.Create();

            random.GetBytes(numberByte);

            return Convert.ToBase64String(numberByte);
        }

        public async Task<TokenDTO> CreateToken(UserDTO user)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.RefreshTokenExpiration);

            // Authentication(Yetkilendirme) başarılı ise JWT token üretilir.
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_customTokenOptions.SecurityKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("ID", user.ID!.Value.ToString()),
                }),
                Expires = accessTokenExpiration,
                Issuer = "readingisgood",
                Audience = "readingisgood",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenDto = new TokenDTO
            {
                access_token = tokenHandler.WriteToken(token),
                refresh_token = CreateRefreshToken(),
                expires_in = _customTokenOptions.AccessTokenExpiration,
                refresh_expires_in = _customTokenOptions.RefreshTokenExpiration,
                token_type = "Bearer",
            };

            user.RefreshToken = tokenDto.refresh_token;
            user.RefreshTokenExpiryDate = refreshTokenExpiration;
            //tokenDto.FirstName = user.FirstName;
            //tokenDto.LastName = user.LastName;
            //tokenDto.UserId = user.ID.Value;

            await _userService.Update(user);
            return tokenDto;
        }

        public async Task<TokenDTO> RefreshToken(string refreshToken)
        {
            var user = await UnitOfWork.Repository<User>().FindAsync(u => u.RefreshToken == refreshToken);

            if (user is null || user.RefreshTokenExpiryDate <= DateTime.Now)
                return null;

            return await CreateToken(Mapper.Map<UserDTO>(user));
        }

        public async Task Revoke()
        {
            var user = await CurrentUser();
            if (user == null)
                return;

            user.RefreshToken = null;
            user.RefreshTokenExpiryDate = null;

            await _userService.Update(Mapper.Map<UserDTO>(user));

            return;
        }

        public int? ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_customTokenOptions.SecurityKey);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = "readingisgood",
                    ValidAudience = "readingisgood",
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "ID").Value);

                // return user id from JWT token if validation successful
                return userId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
    }
}
