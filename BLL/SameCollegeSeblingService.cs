using BOL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class SameCollegeSeblingService : BaseService<SameCollegeSebling>
    {
        public SameCollegeSeblingService(SameCollegeSeblingRepository repo)
           : base(repo)
        {
        }

    }
}
