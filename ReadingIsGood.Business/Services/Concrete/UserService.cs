using Microsoft.EntityFrameworkCore;
using ReadingIsGood.Business.Base.Concrete;
using ReadingIsGood.Business.Services.Interface;
using ReadingIsGood.Core.DTOs;
using ReadingIsGood.Core.Entities;

namespace ReadingIsGood.Business.Services.Concrete
{
    public class UserService : BaseService<User, UserDTO>, IUserService
    {
        public UserService(IServiceProvider sp) : base(sp)
        {

        }

        public async Task<User?> GetUserByUsernamePassword(string username, string password)
        {
            var user = await UnitOfWork.Repository<User>().Query()
              .AsNoTracking().FirstOrDefaultAsync(p => p.UserName == username && p.Password == password);

            return user;
        }
    }
}
