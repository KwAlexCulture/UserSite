using BOL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UserService : BaseService<User>
    {
        public UserService(UserRepository repo)
           : base(repo)
        {
        }

    }
}
