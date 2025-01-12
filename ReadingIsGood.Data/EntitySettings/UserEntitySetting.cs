using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadingIsGood.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Data.EntitySettings
{
    public class UserEntitySetting
    {
        public UserEntitySetting(EntityTypeBuilder<User> entity)
        {
            entity.HasData(
             new User()
             {
                 ID = 1,
                 FirstName = "Admin",
                 LastName = "Admin",
                 UserName = "admin",
                 Password = "admin"
             }
            );
        }
    }
}
