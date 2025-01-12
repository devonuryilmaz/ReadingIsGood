using ReadingIsGood.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Business.Services.Abstract
{
    public interface IAuthenticationService
    {
        Task<TokenDTO> CreateTokenAsync(LoginDTO loginDto);
        Task<TokenDTO> RefreshTokenAsync(string refreshToken);
    }
}
