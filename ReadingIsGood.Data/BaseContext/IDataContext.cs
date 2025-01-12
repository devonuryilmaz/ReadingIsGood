using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Data.BaseContext
{
    public interface IDataContext
    {
        int SaveChanges();

        int SaveChanges(bool acceptAllChangesOnSuccess);

        IDbContextTransaction CurrentTransaction { get; }

        IDbContextTransaction BeginTransaction();
    }
}
