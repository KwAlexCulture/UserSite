using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SeasonRepository : BaseRepository<Season>
    {
        public SeasonRepository(EntitiesDbContext db)
            : base(db)
        { }
    }
}
