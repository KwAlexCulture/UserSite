using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(EntitiesDbContext db)
            : base(db)
        { }
    }
}
