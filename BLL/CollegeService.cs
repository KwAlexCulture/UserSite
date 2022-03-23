using BOL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CollegeService : BaseService<College>
    {
        public CollegeService(CollegeRepository repo)
           : base(repo)
        {
        }

    }
}
