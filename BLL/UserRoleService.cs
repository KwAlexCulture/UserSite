using BOL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UserRoleService : BaseService<UserRole>
    {
        public UserRoleService(UserRoleRepository repo)
           : base(repo)
        {
        }

    }
}
