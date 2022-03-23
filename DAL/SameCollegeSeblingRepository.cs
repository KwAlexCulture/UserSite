using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SameCollegeSeblingRepository : BaseRepository<SameCollegeSebling>
    {
        public SameCollegeSeblingRepository(EntitiesDbContext db)
            : base(db)
        { }
    }
}
