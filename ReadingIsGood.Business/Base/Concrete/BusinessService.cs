using AutoMapper;
using Microsoft.Extensions.Configuration;
using ReadingIsGood.Business.Base.Interface;
using ReadingIsGood.Core.Entities;
using ReadingIsGood.Data.Base;
using ReadingIsGood.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ReadingIsGood.Business.Base.Concrete
{
    public class BusinessService : IBusinessService
    {
        #region Fields

        private IUnitOfWork _unitOfWork;

        private IMapper _mapper { get; set; }
        private IConfiguration _configuration { get; set; }
        private IServiceProvider _serviceProvider { get; set; }
        private IHttpContextAccessor _contextAccessor { get; set; }
        public User _user { get; set; }

        #endregion Fields

        #region Props

        public IUnitOfWork UnitOfWork { get => _unitOfWork; }
        public IMapper Mapper { get => _mapper; }
        public IConfiguration Configuration { get => _configuration; }
        public IServiceProvider ServiceProvider { get => _serviceProvider; }
        public User User { get => _user ?? CurrentUser().Result; }

        public IHttpContextAccessor contextAccessor { get => _contextAccessor; }

        public async Task<User> CurrentUser()
        {
            if (_contextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated == true)
            {
                return await UnitOfWork.Repository<User>().GetByIdAsync(Convert.ToInt64(_contextAccessor.HttpContext.User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value));
            }
            return null;
        }

        protected IUnitOfWork BaseUnitOfWork()
        {
            return UnitOfWork;
        }

        #endregion Props

        public BusinessService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _configuration = (IConfiguration)serviceProvider.GetService(typeof(IConfiguration));
            _mapper = (IMapper)serviceProvider.GetService(typeof(IMapper));
            _unitOfWork = (IUnitOfWork<AppDbContext>)serviceProvider.GetService(typeof(IUnitOfWork<AppDbContext>));

            _contextAccessor = (IHttpContextAccessor)serviceProvider.GetService(typeof(IHttpContextAccessor));

            _unitOfWork.User = User;

        }
    }
}
