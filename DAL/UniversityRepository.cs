using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UniversityRepository : BaseRepository<University>
    {
        public UniversityRepository(EntitiesDbContext db)
            : base(db)
        { }
    }
}
