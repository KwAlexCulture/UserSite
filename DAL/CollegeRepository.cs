using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CollegeRepository : BaseRepository<College>
    {
        public CollegeRepository(EntitiesDbContext db)
            : base(db)
        { }
    }
}
