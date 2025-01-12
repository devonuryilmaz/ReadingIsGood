using ReadingIsGood.Business.Base.Interface;
using ReadingIsGood.Core.DTOs;
using ReadingIsGood.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Business.Services.Interface
{
    public interface IUserService : IBaseService<User, UserDTO>
    {
        Task<User?> GetUserByUsernamePassword(string username, string password);

    }
}
