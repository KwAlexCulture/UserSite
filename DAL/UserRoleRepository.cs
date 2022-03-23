﻿using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserRoleRepository : BaseRepository<UserRole>
    {
        public UserRoleRepository(EntitiesDbContext db)
            : base(db)
        { }
    }
}
