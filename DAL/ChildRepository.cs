using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ChildRepository : BaseRepository<Child>
    {
        public ChildRepository(EntitiesDbContext db)
            : base(db)
        { }
    }
}
