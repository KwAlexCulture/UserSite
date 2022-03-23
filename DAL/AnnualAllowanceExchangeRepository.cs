using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AnnualAllowanceExchangeRepository : BaseRepository<AnnualAllowancesExchange>
    {
        public AnnualAllowanceExchangeRepository(EntitiesDbContext db)
            : base(db)
        { }
    }
}
