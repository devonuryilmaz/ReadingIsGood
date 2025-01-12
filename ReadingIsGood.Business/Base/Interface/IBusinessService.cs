using AutoMapper;
using Microsoft.Extensions.Configuration;
using ReadingIsGood.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Business.Base.Interface
{
    public interface IBusinessService
    {
        IUnitOfWork UnitOfWork { get; }

        IMapper Mapper { get; }

        IConfiguration Configuration { get; }

        IServiceProvider ServiceProvider { get; }
    }
}
