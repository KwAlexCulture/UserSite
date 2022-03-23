using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ClinicalAllowanceRepository : BaseRepository<ClinicalAllowance>
    {
        public ClinicalAllowanceRepository(EntitiesDbContext db)
            : base(db)
        { }
    }
}
