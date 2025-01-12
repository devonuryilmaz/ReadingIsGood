using ReadingIsGood.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Business.Services.Abstract
{
    public interface ITokenService
    {
        Task<TokenDTO> CreateToken(UserDTO user);
        Task<TokenDTO> RefreshToken(string refreshToken);
        Task Revoke();
        int? ValidateToken(string token);
    }
}
