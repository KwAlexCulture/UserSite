﻿using BOL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UniversityService : BaseService<University>
    {
        public UniversityService(UniversityRepository repo)
           : base(repo)
        {
        }

    }
}
